using System;
using UnityEngine;

namespace GM_027
{
    [Serializable]
    public struct GeneratorReq
    {
        public bool isActive;
        public int coe;

        public GeneratorReq(bool isActive,  int coe)
        {
            this.isActive = isActive;
            this.coe = coe;
        }
    }

    public class CoilGenerator : MonoBehaviour, ICoilInteractable
    {
        public GeneratorReq genReq = new GeneratorReq(false, 0);        //coe는 인스펙터에서 조정
        public MainGenerator mainGen;
        public Material activeGenMat;

        public void Interact(CoilPlayerInteraction player)
        {
            CoilPlayer pl = player.GetComponent<CoilPlayer>();
            if (genReq.isActive)
            {
                Debug.LogWarning("발전기가 이미 작동중이다..");
                return;
            }

            if (pl.HasCoil())
            {
                int value = pl.UseCoil();
                string log = $"[Generator Active] 코일을 사용하여 제단을 활성화시켰다.\n";
                mainGen.TotalPower += value * genReq.coe;
                log += $"현재 발전기 에너지 양 : {mainGen.TotalPower}";
                Debug.Log(log);
                OnActive();
                mainGen.gens.Add(this);
                mainGen.IsOverflow();
            }
            else
            {
                Debug.LogWarning("발전기를 작동시키기 위해 코일이 필요하다.");
            }
        }

        public void OnActive()
        {
            genReq.isActive = true;
            GetComponent<MeshRenderer>().material = activeGenMat;
        }
    }

}
