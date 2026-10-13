using UnityEngine;

namespace GM_023
{
    public class PatternManager : MonoBehaviour
    {
        Material cubeMaterial;

        Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
        int[] pattern = new int[4];

        int round = 1;
        int patternIndex = 0;
        int inputIndex = 0;

        float timer = 0f;
        float showTime = 1.5f;

        bool isWrong = false;

        void Start()
        {
            cubeMaterial = GetComponent<MeshRenderer>().material;
            RoundInit();
        }

        void Update()
        {
            if (patternIndex < pattern.Length)
            {
                timer += Time.deltaTime;

                if (timer >= showTime)
                {
                    timer = 0f;
                    patternIndex++;

                    if (patternIndex < pattern.Length)
                    {
                        cubeMaterial.color = colors[pattern[patternIndex]];
                    }
                    else
                    {
                        cubeMaterial.color = Color.gray;
                        Debug.Log("입력 시작! 1 : 빨강 / 2 : 초록 / 3 : 파랑 / 4 : 노랑");
                    }
                }

                return;
            }

            if (!isWrong && inputIndex < pattern.Length)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) InputAnswer(0);
                else if (Input.GetKeyDown(KeyCode.Alpha2)) InputAnswer(1);
                else if (Input.GetKeyDown(KeyCode.Alpha3)) InputAnswer(2);
                else if (Input.GetKeyDown(KeyCode.Alpha4)) InputAnswer(3);

                return;
            }

            if (!isWrong && round < 3)
            {
                timer += Time.deltaTime;

                if (timer >= 1f)
                {
                    round++;
                    RoundInit();
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                round = 1;
                RoundInit();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }

        void RoundInit()
        {
            patternIndex = 0;
            inputIndex = 0;
            timer = 0f;
            isWrong = false;

            if (round == 1) 
                showTime = 1.5f;
            else if (round == 2) 
                showTime = 1f;
            else 
                showTime = 0.5f;

            for (int i = 0; i < pattern.Length; i++)
            {
                pattern[i] = i;
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                int randomIndex = Random.Range(i, pattern.Length);
                int temp = pattern[i];
                pattern[i] = pattern[randomIndex];
                pattern[randomIndex] = temp;
            }

            cubeMaterial.color = colors[pattern[patternIndex]];
            Debug.Log($"{round}라운드 시작! 색상 하나당 {showTime}초 동안 표시합니다.");
        }

        void InputAnswer(int answer)
        {
            cubeMaterial.color = colors[answer];

            if (answer != pattern[inputIndex])
            {
                isWrong = true;
                cubeMaterial.color = Color.red;
                Debug.Log($"{round}라운드 실패. 재시작하려면 R, 종료하려면 Q를 누르세요.");
                return;
            }

            inputIndex++;
            Debug.Log($"정답! 현재 입력 : {inputIndex}/{pattern.Length}");

            if (inputIndex < pattern.Length) return;

            cubeMaterial.color = Color.green;
            timer = 0f;

            if (round < 3)
            {
                Debug.Log($"{round}라운드 성공. 1초 뒤 다음 라운드를 시작합니다.");
            }
            else
            {
                Debug.Log("모든 라운드 성공. 재시작하려면 R, 종료하려면 Q를 누르세요.");
            }
        }
    }

}
