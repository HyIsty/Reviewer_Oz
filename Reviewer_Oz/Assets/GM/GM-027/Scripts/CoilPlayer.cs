using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace GM_027
{
    public class CoilPlayer : MonoBehaviour
    {
        private string[] validItemIds = { "coil" };

        public InputAction InventoryCheckAction;

        public List<EnergyCoil> coils = new List<EnergyCoil>();

        private void Start()
        {
            InventoryCheckAction.Enable();
        }
        private void Update()
        {
            if (InventoryCheckAction.WasPressedThisFrame())
            {
                CheckCoil();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnergyCoil>() == null) return;

            EnergyCoil coil = other.GetComponent<EnergyCoil>();

            if (coil != null)
            {
                AddAtInventory(coil);
                Destroy(coil.gameObject);
            }

        }

        void AddAtInventory(EnergyCoil coil)
        {
            if (!IsValidItemId(coil.itemId))
            {
                Debug.LogWarning($"이게 코일이 맞나...? : {coil.itemId}");
                return;
            }
            EnergyCoil tmp = coil;
            coils.Add(coil);
            Debug.Log($"[AddInventory] 코일을 주웠습니다. 코일 전력 : {coil.powerValue}" +
                $"\n현재 가진 코일 갯수 : {coils.Count}");
            PrintInventory();
        }

        void PrintInventory()
        {
            string log = "==== 현재 인벤토리 ====\n";
            for (int i = 0; i < coils.Count; i++)
            {
                log += $"{i}. {coils[i].itemId} 전력 : {coils[i].powerValue}\n";
            }
            log += $"총 아이템 수 : {coils.Count}";
            Debug.Log(log);
        }

        private bool IsValidItemId(string targetId)
        {
            for (int i = 0; i < validItemIds.Length; i++)
            {
                if (validItemIds[i] == targetId)
                {
                    return true;
                }
            }
            return false;
        }

        void CheckCoil()
        {
            if (coils.Count > 0)
            {
                Debug.Log($"코일 {coils.Count}개를 가지고 있습니다.");
            }
            else
            {
                Debug.Log("코일이 없습니다.");
            }
        }

        public bool HasCoil()
        {
            return coils.Count > 0;
        }

        public int UseCoil()
        {
            if (coils != null)
            {
                EnergyCoil coil = coils[0];
                coils.RemoveAt(0);
                Debug.Log($"[UseItem] {coil.itemId}을(를) 사용했습니다.");
                PrintInventory();
                return coil.powerValue;
            }
            return 0;
        }
    }

}