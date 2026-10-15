using UnityEngine;


namespace GM_026
{
    [System.Serializable]
    public struct AltarRequirement
    {
        public AltarState state;
        public KeyType requireKey;
    }

    public class AltarInteract : MonoBehaviour, IInteractable
    {
        public AltarRequirement altarReq;
        public Material onActiveAltar;
        public Material onPurifiedAltar;

        public void Interact(PlayerInteraction player)
        {
            PlayerInventory pl = player.GetComponent<PlayerInventory>();
            if (altarReq.state == AltarState.Inactive)
            {
                Debug.LogWarning("그저 낡은 제단이다.");
                return;
            }
            else if (altarReq.state == AltarState.Purified)
            {
                Debug.LogWarning("이미 정화된 제단이다.");
                return;
            }


            if (pl.HasKey(altarReq.requireKey))
            {
                if (player.altars[player.index] != this)
                {
                    Debug.LogError("순서가 잘못되었다...");
                    player.purifyAltars.Clear();
                    player.index = 0;
                    return;
                }

                pl.UseKey(altarReq.requireKey);
                Debug.Log($"[AltarInteraction] {altarReq.requireKey}를 사용하여 제단을 활성화시켰다.");
                altarReq.state = AltarState.Purified;
                player.index++;
                if (player.index < player.altars.Length)
                {
                    player.altars[player.index].altarReq.state = AltarState.Active;
                    player.altars[player.index].OnActive();
                    player.ActiveAltarText();
                }
                OnPurify();
                player.purifyAltars.Add(this.ToString());
                player.JudgeClear();
            }
            else
            {
                Debug.LogWarning($"이 제단을 활성화시키기 위해 열쇠가 필요하다. : {altarReq.requireKey}");
            }
        }

        public string GetInteractionDescription()
        {
            return "";
        }

        private void OnPurify()
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = onPurifiedAltar;
        }

        public void OnActive()
        {
            if (onActiveAltar == null) return;
            this.GetComponent<MeshRenderer>().sharedMaterial = onActiveAltar;
        }
    }

}