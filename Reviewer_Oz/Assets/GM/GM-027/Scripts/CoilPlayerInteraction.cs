using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_027
{
    public class CoilPlayerInteraction : MonoBehaviour
    {
        public InputAction InteractAction;
        private ICoilInteractable currentInteractable;

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
            ICoilInteractable interact = other.GetComponent<ICoilInteractable>();
            if (interact == null) return;

            string log = $"주변에 상호작용 가능한 상대가 있습니다. : {other.gameObject.name} ";
            if (other.GetComponent<CoilGenerator>() != null)
                log += $"발전기 배율 : {other.GetComponent<CoilGenerator>().genReq.coe}";
            Debug.Log(log);
            currentInteractable = interact;
        }

        private void OnTriggerExit(Collider other)
        {
            ICoilInteractable interact = other.GetComponent<ICoilInteractable>();
            if (interact == null) return;

            Debug.Log($"상호작용 가능한 상대를 떠났습니다. : {other.gameObject.name}");
            currentInteractable = null;
        }
    }

}
