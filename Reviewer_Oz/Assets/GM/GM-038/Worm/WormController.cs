using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_038
{
    public class WormController : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private Camera viewCamera;
        [SerializeField] private WormSegment segmentPrefab;
        [SerializeField] private LineRenderer border;

        [Header("지렁이")]
        [SerializeField] private int segmentCount = 8;
        [SerializeField] private float segmentSpacing = 0.55f;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float turnSpeed = 150f;
        [SerializeField] private Color headColor = Color.cyan;
        [SerializeField] private Color bodyColor = new Color(0.2f, 0.5f, 1f);

        [Header("영역")]
        [SerializeField] private float wallInset = 0.4f;
        [SerializeField] private float spawnOffset = 1.2f;

        List<WormSegment> segments = new List<WormSegment>();
        List<Vector2> path = new List<Vector2>();

        Vector2 headPosition;
        Vector2 direction;

        float turnInput = 0f;
        float entryRemaining = 0f;
        bool entered = false;

        public bool IsReady { get; private set; }
        public int SegmentCount => segments.Count;
        public Vector2 HeadPosition => headPosition;
        public float HeadRadius => segments[0].Radius;
        public Vector2 MinBounds { get; private set; }
        public Vector2 MaxBounds { get; private set; }

        void Start()
        {
            if (viewCamera == null || !viewCamera.orthographic ||
                segmentPrefab == null || border == null)
            {
                Debug.LogWarning("Orthographic Camera, Segment Prefab, Border를 연결하세요.");
                enabled = false;
                return;
            }

            segmentCount = Mathf.Max(2, segmentCount);
            segmentSpacing = Mathf.Max(0.1f, segmentSpacing);
            moveSpeed = Mathf.Max(0.1f, moveSpeed);

            CreateWorm();
            IsReady = true;
        }

        void CreateWorm()
        {
            float depth = -viewCamera.transform.position.z;

            Vector2 screenMin = viewCamera.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
            Vector2 screenMax = viewCamera.ViewportToWorldPoint(new Vector3(1f, 1f, depth));

            MinBounds = screenMin + Vector2.one * wallInset;
            MaxBounds = screenMax - Vector2.one * wallInset;

            border.useWorldSpace = true;
            border.loop = true;
            border.positionCount = 4;

            border.SetPositions(new Vector3[]
            {
                new Vector3(MinBounds.x, MinBounds.y, 0f),
                new Vector3(MaxBounds.x, MinBounds.y, 0f),
                new Vector3(MaxBounds.x, MaxBounds.y, 0f),
                new Vector3(MinBounds.x, MaxBounds.y, 0f)
            });

            switch (Random.Range(0, 4))
            {
                case 0:
                    headPosition = new Vector2(screenMin.x - spawnOffset, Random.Range(MinBounds.y + 1f, MaxBounds.y - 1f));
                    break;

                case 1:
                    headPosition = new Vector2(screenMax.x + spawnOffset, Random.Range(MinBounds.y + 1f, MaxBounds.y - 1f));
                    break;

                case 2:
                    headPosition = new Vector2(Random.Range(MinBounds.x + 1f, MaxBounds.x - 1f), screenMin.y - spawnOffset);
                    break;

                default:
                    headPosition = new Vector2(Random.Range(MinBounds.x + 1f, MaxBounds.x - 1f), screenMax.y + spawnOffset);
                    break;
            }

            Vector2 target = new Vector2(
                Random.Range(Mathf.Lerp(MinBounds.x, MaxBounds.x, 0.3f), Mathf.Lerp(MinBounds.x, MaxBounds.x, 0.7f)),
                Random.Range(Mathf.Lerp(MinBounds.y, MaxBounds.y, 0.3f), Mathf.Lerp(MinBounds.y, MaxBounds.y, 0.7f)));

            direction = (target - headPosition).normalized;

            for (int i = 0; i < segmentCount; i++)
            {
                Vector2 position = headPosition - direction * segmentSpacing * i;

                WormSegment segment = Instantiate(segmentPrefab, position, Quaternion.identity, transform);
                segment.Initialize(i + 1, i == 0 ? headColor : bodyColor);

                segments.Add(segment);
            }

            for (int i = segmentCount; i >= 0; i--)
            {
                path.Add(headPosition - direction * segmentSpacing * i);
            }

            Vector2 min = MinBounds + Vector2.one * HeadRadius;
            Vector2 max = MaxBounds - Vector2.one * HeadRadius;

            if (headPosition.x < min.x)
                entryRemaining = (min.x - headPosition.x) / direction.x;
            else if (headPosition.x > max.x)
                entryRemaining = (max.x - headPosition.x) / direction.x;

            if (headPosition.y < min.y)
                entryRemaining = Mathf.Max(entryRemaining, (min.y - headPosition.y) / direction.y);
            else if (headPosition.y > max.y)
                entryRemaining = Mathf.Max(entryRemaining, (max.y - headPosition.y) / direction.y);
        }

        public void OnTurn(InputValue value)
        {
            turnInput = value.Get<float>();
        }

        void Update()
        {
            if (!IsReady) return;

            if (entered)
            {
                direction = ((Vector2)(Quaternion.Euler(0f, 0f, -turnInput * turnSpeed * Time.deltaTime) * direction)).normalized;
            }

            float remaining = moveSpeed * Time.deltaTime;

            if (!entered)
            {
                float step = Mathf.Min(remaining, entryRemaining);

                headPosition += direction * step;
                entryRemaining -= step;
                remaining -= step;

                RecordPosition();

                if (entryRemaining <= 0.0001f) entered = true;
            }

            if (entered && remaining > 0f) MoveInside(remaining);

            segments[0].transform.position = headPosition;
            FollowPath();
        }

        void MoveInside(float remaining)
        {
            Vector2 min = MinBounds + Vector2.one * HeadRadius;
            Vector2 max = MaxBounds - Vector2.one * HeadRadius;

            if (min.x >= max.x || min.y >= max.y) return;

            headPosition.x = Mathf.Clamp(headPosition.x, min.x, max.x);
            headPosition.y = Mathf.Clamp(headPosition.y, min.y, max.y);

            RecordPosition();

            while (remaining > 0f)
            {
                float xDistance = float.PositiveInfinity;
                float yDistance = float.PositiveInfinity;

                if (direction.x > 0f)
                    xDistance = (max.x - headPosition.x) / direction.x;
                else if (direction.x < 0f)
                    xDistance = (min.x - headPosition.x) / direction.x;

                if (direction.y > 0f)
                    yDistance = (max.y - headPosition.y) / direction.y;
                else if (direction.y < 0f)
                    yDistance = (min.y - headPosition.y) / direction.y;

                float step = Mathf.Min(remaining, Mathf.Min(xDistance, yDistance));

                bool hitX = xDistance <= remaining && xDistance <= yDistance;
                bool hitY = yDistance <= remaining && yDistance <= xDistance;

                headPosition += direction * step;

                if (hitX) headPosition.x = direction.x > 0f ? max.x : min.x;
                if (hitY) headPosition.y = direction.y > 0f ? max.y : min.y;

                RecordPosition();

                if (hitX) direction = Vector2.Reflect(direction, Vector2.right);
                if (hitY) direction = Vector2.Reflect(direction, Vector2.up);

                remaining -= step;
            }
        }

        void RecordPosition()
        {
            if ((headPosition - path[path.Count - 1]).sqrMagnitude > 0f)
            {
                path.Add(headPosition);
            }
        }

        void FollowPath()
        {
            int pathIndex = path.Count - 1;
            float travelled = 0f;

            for (int i = 1; i < segments.Count; i++)
            {
                float targetDistance = i * segmentSpacing;

                while (pathIndex > 0)
                {
                    float length = Vector2.Distance(path[pathIndex], path[pathIndex - 1]);

                    if (travelled + length >= targetDistance)
                    {
                        float ratio = (targetDistance - travelled) / length;

                        segments[i].transform.position = Vector2.Lerp(path[pathIndex], path[pathIndex - 1], ratio);
                        break;
                    }

                    travelled += length;
                    pathIndex--;
                }

                if (pathIndex == 0) segments[i].transform.position = path[0];
            }

            if (pathIndex > 1) path.RemoveRange(0, pathIndex - 1);
        }

        public void GrowSegment(int number)
        {
            if (number < 1 || number > segments.Count) return;

            segments[number - 1].Grow();
            Debug.Log($"{number}번 구슬이 커졌습니다!");
        }
    }
}