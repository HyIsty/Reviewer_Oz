using UnityEngine;
using System.Collections.Generic;

namespace GM_017
{
    public class DungeonExplorer : MonoBehaviour
    {
        private string[] monsterPool = new string[] { "슬라임", "오크", "스켈레톤", "좀비", "구울", "플랜트" };
        private string[] roomPool = new string[] { "평지", "언덕", "악지", "던전", "숲" };

        Queue<string> monsterQueue = new Queue<string>();
        Stack<string> travelHistory = new Stack<string>();

        public List<string> monsterDebug = new List<string>();
        public List<string> historyDebug = new List<string>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) SearchMonster();
            if (Input.GetKeyDown(KeyCode.A)) Combat();
            if (Input.GetKeyDown(KeyCode.M)) MoveForward();
            if (Input.GetKeyDown(KeyCode.B)) MoveBackward();
            if (Input.GetKeyDown(KeyCode.D)) Draw();
            if (Input.GetKeyDown(KeyCode.U)) Undo();
        }

        void SearchMonster()
        {
            if (monsterPool == null || monsterPool.Length <= 0) return;

            string tempMon = monsterPool[Random.Range(0, monsterPool.Length)];
            monsterQueue.Enqueue(tempMon);
            Debug.Log($"[조우 예고] 저 멀리서 {tempMon}(이)가 나타났습니다!");
            RefreshDebug();
        }

        void Combat()
        {
            if (monsterQueue == null || monsterQueue.Count <= 0)
            {
                Debug.LogWarning("보이는 적이 없습니다.");
                return;
            }

            string temp = monsterQueue.Dequeue();
            Debug.Log($"[전투] {temp}을/를 처치했습니다! 남은 적 : {monsterQueue.Count}");
            RefreshDebug();
        }

        void MoveForward()
        {
            if (roomPool == null || roomPool.Length <= 0) return;

            string tempRoom = roomPool[Random.Range(0, roomPool.Length)];
            travelHistory.Push(tempRoom);
            Debug.Log($"[이동] {tempRoom}에 진입했습니다. 총 이동 거리 : {travelHistory.Count}");
            RefreshDebug();
        }

        void MoveBackward()
        {
            if (travelHistory.Count <= 0)
            {
                Debug.LogWarning("던전 입구까지 돌아왔습니다. 더 이상 퇴각할 수 없습니다.");
                return;
            }

            string backroom = travelHistory.Pop();
            string temp = travelHistory.Count <= 0 ? "첫 지점" : travelHistory.Peek();
            Debug.Log($"[퇴각] {backroom}에서 퇴각해 {temp}(으)로 돌아왔습니다.");
            RefreshDebug();
        }

        void RefreshDebug()
        {
            monsterDebug.Clear();
            historyDebug.Clear();
            lootDebug.Clear();

            foreach (string m in monsterQueue)
            {
                monsterDebug.Add(m);
            }
            foreach (string t in travelHistory)
            {
                historyDebug.Add(t);
            }
            foreach (string i in lootHistory)
            {
                lootDebug.Add(i);
            }

        }

        string[] itemNames = { "쓰레기", "일반 검", "희귀 방패", "전설의 보석" };
        int[] weights = { 700, 200, 90, 10 }; // 총합: 1000
        int undoLeft = 3;

        Stack<string> lootHistory = new Stack<string>();

        public List<string> lootDebug = new List<string>();

        void Draw()
        {
            int total = 0;
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                total += weights[i];
            }
            int r = Random.Range(0, total);

            for (int i = 0; i < itemNames.Length; i++)
            {
                sum += weights[i];
                if (sum > r)
                {
                    Debug.Log($"{itemNames[i]}를 뽑았습니다! r = {r}");
                    lootHistory.Push(itemNames[i]);
                    break;
                }
            }
            RefreshDebug();
        }

        void Undo()
        {
            if (lootHistory == null) return;
            if (lootHistory.Count <= 0)
            {
                Debug.Log("뽑은 아이템이 없습니다.");
                return;
            }
            if (undoLeft <= 0)
            {
                Debug.Log("다시뽑기 횟수를 모두 소진했습니다.");
                return;
            }

            string leftItem = lootHistory.Pop();
            Debug.Log($"{leftItem}를 제거하고 다시 뽑습니다.");
            Draw();
            undoLeft--;
        }
    }
}
