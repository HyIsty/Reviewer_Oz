using UnityEngine;

namespace GM_038
{
    public class RacingRoad : MonoBehaviour
    {
        [SerializeField] private RacingStage stage;
        [SerializeField] private Transform[] roadPieces;
        [SerializeField] private float pieceHeight = 16f;

        float offset = 0f;
        Vector3[] startPositions;

        void Start()
        {
            startPositions = new Vector3[roadPieces.Length];

            for (int i = 0; i < roadPieces.Length; i++)
            {
                startPositions[i] = roadPieces[i].position;
            }
        }

        void Update()
        {
            if (stage.IsGameOver) return;

            offset = Mathf.Repeat(offset + stage.ScrollSpeed * Time.deltaTime, pieceHeight);

            for (int i = 0; i < roadPieces.Length; i++)
            {
                roadPieces[i].position = startPositions[i] + Vector3.down * offset;
            }
        }
    }
}