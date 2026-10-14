using UnityEngine;

namespace GM_024
{
    public class CubeSelect : MonoBehaviour
    {
        enum State
        {
            Red,
            Green,
            Blue
        }

        [SerializeField] private MeshRenderer targetCube;
        [SerializeField] private MeshRenderer[] cubes = new MeshRenderer[3];

        Color[] colors = { Color.red, Color.green, Color.blue };
        int[] cubeColors = new int[3];

        State currentState = State.Red;
        int score = 0;

        void Start()
        {
            if (targetCube == null || cubes == null || cubes.Length != 3)
            {
                Debug.LogWarning("목표 큐브와 선택 큐브 3개를 연결하세요.");
                enabled = false;
                return;
            }

            for (int i = 0; i < cubes.Length; i++)
            {
                if (cubes[i] == null)
                {
                    Debug.LogWarning($"{i + 1}번 큐브를 연결하세요.");
                    enabled = false;
                    return;
                }
            }

            ColorInit();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) InputAnswer(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) InputAnswer(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) InputAnswer(2);
        }

        void ColorInit()
        {
            targetCube.material.color = colors[(int)currentState];

            int answerIndex = Random.Range(0, cubeColors.Length);

            for (int i = 0; i < cubeColors.Length; i++)
            {
                if (i == answerIndex) cubeColors[i] = (int)currentState;
                else cubeColors[i] = Random.Range(0, colors.Length);

                cubes[i].material.color = colors[cubeColors[i]];
            }

            Debug.Log("왼쪽 위의 색깔을 눌러주세요. 숫자 1, 2, 3으로 진행합니다.");
        }

        void InputAnswer(int index)
        {
            if (cubeColors[index] != (int)currentState)
            {
                currentState = State.Red;
                Debug.Log($"오답. 빨강 단계로 돌아갑니다. 현재 점수 : {score}");
                ColorInit();
                return;
            }

            switch (currentState)
            {
                case State.Red:
                    currentState = State.Green;
                    Debug.Log("정답. 다음은 초록입니다.");
                    break;

                case State.Green:
                    currentState = State.Blue;
                    Debug.Log("정답. 다음은 파랑입니다.");
                    break;

                case State.Blue:
                    score++;
                    currentState = State.Red;
                    Debug.Log($"RGB 성공. 1점 획득. 현재 점수 : {score}");
                    break;
            }

            ColorInit();
        }
    }

}
