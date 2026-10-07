using UnityEngine;

namespace Gm_015
{
    public class MonsterCardDeckManager : MonoBehaviour
    {
        public int[] monsterDeck = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) Swap(monsterDeck);
            if (Input.GetKeyDown(KeyCode.S)) Shuffle(monsterDeck);
            if (Input.GetKeyDown(KeyCode.Space)) Game();
        }

        void Swap(int[] arr)
        {
            if (arr == null || arr.Length <= 1) return;

            int temp = arr[0];
            arr[0] = arr[arr.Length - 1];
            arr[arr.Length - 1] = temp;
        }

        void Swap_Shuffle(int[] arr, int a, int b)
        {
            if (arr == null || arr.Length <= 1 ||
                a > arr.Length || a < 0 || b > arr.Length || b < 0) return;

            int temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }

        void Shuffle(int[] arr)
        {
            if (arr == null || arr.Length <= 1) return;

            for (int i = 0; i < arr.Length; i++)
            {
                int randomIndex = Random.Range(i, arr.Length);
                Swap_Shuffle(arr, i, randomIndex);
            }
        }

        void Game()
        {
            int bossPower = Random.Range(1, 11);
            if (monsterDeck[0] > bossPower)
            {
                Debug.Log($"플레이어 : {monsterDeck[0]} / {bossPower} : 보스 | 플레이어가 승리하였습니다!");
            }
            else if (monsterDeck[0] == bossPower)
            {
                Debug.Log($"플레이어 : {monsterDeck[0]} / {bossPower} : 보스 | 비겼습니다.");
            }
            else
            {
                Debug.Log($"플레이어 : {monsterDeck[0]} / {bossPower} : 보스 | 플레이어가 패배하였습니다..");
            }
            Debug.Log("게임이 끝나 덱을 셔플합니다.");
            Shuffle(monsterDeck);
        }
    }
}
