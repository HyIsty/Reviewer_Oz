using System.Collections.Generic;
using UnityEngine;

namespace GM_027
{
    public class MainGenerator : MonoBehaviour, ICoilInteractable
    {
        [SerializeField] private int totalPower = 0;
        private int targetPower = 100;
        public List<CoilGenerator> gens = new List<CoilGenerator>();

        public int TotalPower
        {
            get { return totalPower; }
            set { totalPower = value; }
        }
        public int TargetPower => targetPower;

        public void Interact(CoilPlayerInteraction player)
        {
            if (totalPower < targetPower)
                Debug.Log($"현재 {totalPower}만큼의 에너지가 모여있다.\n목표치는 {targetPower}이다. 넘치면 좋지 않은 일이...");
            else if (totalPower > targetPower)
                Debug.Log("에너지가 넘쳐버려 고장나버렸다...");
            else
                Debug.Log("에너지가 충분히 모여 발전기가 작동하고 있다.");
        }

        public void IsOverflow()
        {
            if (totalPower > targetPower)
            {
                Debug.LogError("에너지가 넘쳐흘러 발전기가 고장났다...");
            }
            else if (totalPower == targetPower && gens.Count == 3)
            {
                Debug.Log("에너지가 충분히 모여 발전기가 작동하기 시작했다!");
            }
        }
    }
}
