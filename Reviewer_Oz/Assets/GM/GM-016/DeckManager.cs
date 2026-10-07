using UnityEngine;
using System.Collections.Generic;

namespace GM_016
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField]
        private string[] masterCards = new string[] { "용사", "마법사", "궁수", "도적", "기병",
        "사제", "용기사", "슬라임", "전투기계", "연금술사" };
        [SerializeField]
        private List<string> playerDeck = new List<string>();
        [SerializeField]
        private List<string> hands = new List<string>();


        void RefillDeck(string[] arr, List<string> list)
        {
            if (arr == null || arr.Length <= 0 || list == null) return;

            list.Clear();
            int i = 0;
            while (list.Count < 10)
            {
                list.Add(arr[i++ % 10]);
            }

            if (playerDeck.Count == 10) Debug.Log("덱을 리필했습니다.");
            else Debug.LogWarning("뭔가 잘못되었습니다...");
        }

        void ShuffleDeck(List<string> list)
        {
            if (list == null || list.Count <= 1) return;

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i + 1);
                Swap(list, i, j);
            }

            if (playerDeck.Count == 10) Debug.Log("덱을 셔플했습니다.");
            else Debug.LogWarning("뭔가 잘못되었습니다...");
        }
        void Swap(List<string> list, int a, int b)
        {
            if (list == null || list.Count <= 1 || a == b ||
                list[a] == null || list[b] == null || a >= list.Count ||
                b >= list.Count || a < 0 || b < 0) return;

            string temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        void PrintDeck(List<string> list)
        {
            if (list == null) return;
            if (list.Count <= 0)
            {
                Debug.Log("현재 덱이 비었습니다.");
                return;
            }

            string printArr = "";
            foreach (string item in list)
            {
                printArr += $"{list.IndexOf(item) + 1}. {item} | ";
            }

            Debug.Log($"현재 덱 상황 : {printArr}");
        }

        void Draw(List<string> list)
        {
            if (list == null) return;
            if (list.Count <= 0)
            {
                Debug.Log("덱이 비었습니다.");
                return;
            }

            string drawCard = list[0];
            list.RemoveAt(0);
            hands.Add(drawCard);
            Debug.Log($"{drawCard} 카드를 드로우했습니다. 현재 덱 크기 : {list.Count}");
        }

        void Delete(List<string> list, string del)
        {
            if (list == null || list.Count <= 0) return;

            int count = 0;
            for (int i = list.Count - 1; i > 0; i--)
            {
                if (list[i] == del)
                {
                    list.RemoveAt(i);
                    count++;
                }
            }
            if (count > 0) Debug.Log($"{del} 카드를 덱에서 {count} 장 삭제했습니다. 현재 덱 크기 : {list.Count}");
            else Debug.Log($"{del} 카드가 덱에 없습니다.");
        }

        void Insert(List<string> list)
        {
            if (list == null) return;

            list.Insert(0, "EmergencyHeal");
            Debug.Log($"긴급회복을 덱 맨 위에 넣었습니다. 현재 덱 크기 : {list.Count}");
        }

        void ReShuffle(string[] arr, List<string> list)
        {
            if (arr == null || arr.Length <= 0 || list == null) return;
            if (list.Count > 0)
            {
                Debug.Log($"아직 덱에 {list.Count}장의 카드가 남아있다...");
                return;
            }

            RefillDeck(arr, list);
            ShuffleDeck(list);
            PrintDeck(list);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) RefillDeck(masterCards, playerDeck);
            if (Input.GetKeyDown(KeyCode.S)) ShuffleDeck(playerDeck);
            if (Input.GetKeyDown(KeyCode.P)) PrintDeck(playerDeck);
            if (Input.GetKeyDown(KeyCode.Space)) Draw(playerDeck);
            if (Input.GetKeyDown(KeyCode.B)) Delete(playerDeck, "슬라임");
            if (Input.GetKeyDown(KeyCode.I)) Insert(playerDeck);
            if (Input.GetKeyDown(KeyCode.R)) ReShuffle(masterCards, playerDeck);
        }
    }
}
