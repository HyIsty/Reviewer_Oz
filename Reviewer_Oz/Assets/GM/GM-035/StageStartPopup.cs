using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM_035
{
    public class StageStartPopup : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private RoomData roomData;
        [SerializeField] private GameObject popupRoot;

        [Header("팝업 텍스트")]
        [SerializeField] private TMP_Text stageText;
        [SerializeField] private TMP_Text enemyCountText;
        [SerializeField] private TMP_Text startGoldText;

        void Start()
        {
            OpenPopup();
        }

        public void OpenPopup()
        {
            if (roomData == null || popupRoot == null || stageText == null || enemyCountText == null || startGoldText == null)
            {
                Debug.LogWarning("RoomData와 팝업 UI를 연결하세요.");
                return;
            }

            stageText.text = $"Stage : {roomData.stageNumber:00}";
            enemyCountText.text = $"Enemy Count : {roomData.spawnCount}";
            startGoldText.text = $"Start Gold : {roomData.requiredGold}";

            popupRoot.SetActive(true);
        }

        public void StartGame()
        {
            if (popupRoot == null) return;

            popupRoot.SetActive(false);

            Debug.Log("게임을 시작합니다.");
        }

        public void SelectWeapon(string weaponName)
        {
            Debug.Log($"{weaponName} 버튼을 눌렀습니다.");
        }
    }
}
