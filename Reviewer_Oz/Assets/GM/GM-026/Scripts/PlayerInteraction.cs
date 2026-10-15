using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace GM_026
{
    public class PlayerInteraction : MonoBehaviour
    {
        public InputAction InteractAction;
        private IInteractable currentInteractable;
        private PlayerInventory inventory;

        public AltarInteract[] altars;
        public List<string> purifyAltars = new List<string>();
        public int index = 0;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
            altars[0].altarReq.state = AltarState.Active;
            altars[0].OnActive();
        }
        private void Start()
        {
            InteractAction.Enable();
        }
        private void Update()
        {
            if (InteractAction.WasPressedThisFrame())
            {
                if (currentInteractable != null)
                {
                    currentInteractable.Interact(this);
                }
                else
                    Debug.Log("상호작용 가능한 상대가 없습니다");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interact = other.GetComponent<IInteractable>();
            if (interact == null) return;

            Debug.Log($"주변에 상호작용 가능한 상대가 있습니다. : {other.gameObject.name}");
            currentInteractable = interact;
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interact = other.GetComponent<IInteractable>();
            if (interact == null) return;

            Debug.Log($"상호작용 가능한 상대를 떠났습니다. : {other.gameObject.name}");
            currentInteractable = null;
        }
        public void ActiveAltarText()
        {
            Debug.Log($"현재 {altars[index]}가 활성화 중이다.");
        }

        public void JudgeClear()
        {
            if (purifyAltars.Count != altars.Length) return;

            for (int i = 0; i < altars.Length; i++)
            {
                if (altars[i].ToString() != purifyAltars[i])
                {
                    Debug.LogError("뭔가 잘못되었다.");
                    purifyAltars.Clear();
                    return;
                }
            }
            Debug.Log("[Clear] 유적을 정화하는 것에 성공했다.");
        }
    }

}