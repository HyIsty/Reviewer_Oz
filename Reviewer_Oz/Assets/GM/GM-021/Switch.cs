using UnityEngine;

public class Switch : MonoBehaviour
{
    private int round = 1;
    private int correctRound = 0;
    private float correctRate = 0.0f;
    public int answer = 0;

    [Header("Switches")]
    [SerializeField] private GameObject[] cubes = new GameObject[3];
    private Material[] mats = new Material[3];

    void Start()
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            mats[i] = cubes[i].GetComponent<MeshRenderer>().material; 
        }
        RoundInit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            InputAnswer(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            InputAnswer(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            InputAnswer(3);
        if (Input.GetKeyDown(KeyCode.C))
            GetAnswer();
    }

    void RoundInit()
    {
        foreach (var mat in mats)
            mat.color = Color.gray;

        if (round >= 11)
        {
            correctRate = (float)correctRound / (float)round;
            Debug.Log("총 정답률 : " + correctRate + "%");
            return;
        }

        Debug.Log("라운드 시작. 현재 라운드 : " + round);
        answer = Random.Range(1, 4);
    }

    void InputAnswer(int index)
    {
        if(index == answer)
        {
            mats[index - 1].color = Color.green;
            correctRound++;
        }
        else
        {
            mats[index -1].color = Color.red;
        }

        round++;
        Invoke("RoundInit", 2.0f);
    }

    void GetAnswer()
    {
        Debug.Log($"치트 : 현재 라운드 정답 - {answer}");
    }
}
