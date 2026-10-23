using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GM_035;

namespace GM_036
{
    public enum GameState
    {
        INIT,
        STAGE_INTRO,
        WAVE_READY,
        WAVE_PROGRESS,
        WAVE_INTERVAL,
        STAGE_CLEAR,
        STAGE_FAILED,
        PAUSE
    }

    public class StageUIController : MonoBehaviour
    {
        [Header("방 데이터")]
        [SerializeField] private RoomData roomData;
        [SerializeField, Min(1)] private int totalWaves = 3;

        [Header("팝업")]
        [SerializeField] private GameObject popupRoot;
        [SerializeField] private TMP_Text popupTitleText;
        [SerializeField] private TMP_Text popupContentText;
        [SerializeField] private Button popupButton;
        [SerializeField] private TMP_Text popupButtonText;

        [Header("HUD")]
        [SerializeField] private TMP_Text stateText;
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private Button failButton;
        [SerializeField] private Button pauseButton;

        [Header("현재 상태")]
        [SerializeField] private GameState currentState;

        GameState beforePauseState;

        int currentWave = 1;
        float timer = 0f;

        void Start()
        {
            if (roomData == null || popupRoot == null || popupTitleText == null ||
                popupContentText == null || popupButton == null || popupButtonText == null ||
                stateText == null || waveText == null || failButton == null || pauseButton == null)
            {
                Debug.LogWarning("RoomData와 UI 참조를 모두 연결하세요.");
                return;
            }

            totalWaves = Mathf.Max(1, totalWaves);
            currentWave = 1;

            ChangeState(GameState.INIT);
        }

        void Update()
        {
            if (currentState == GameState.PAUSE) return;

            timer += Time.deltaTime;

            switch (currentState)
            {
                case GameState.INIT:
                    if (timer >= 2f) 
                        ChangeState(GameState.STAGE_INTRO);
                    break;

                case GameState.WAVE_READY:
                    popupContentText.text = Mathf.Max(1, Mathf.CeilToInt(3f - timer)).ToString();

                    if (timer >= 3f) 
                        ChangeState(GameState.WAVE_PROGRESS);
                    break;

                case GameState.WAVE_PROGRESS:
                    if (timer >= 5f) 
                        ChangeState(GameState.WAVE_INTERVAL);
                    break;

                case GameState.WAVE_INTERVAL:
                    if (timer >= 1f)
                    {
                        if (currentWave >= totalWaves)
                        {
                            ChangeState(GameState.STAGE_CLEAR);
                        }
                        else
                        {
                            currentWave++;
                            ChangeState(GameState.WAVE_READY);
                        }
                    }
                    break;
            }
        }

        void ChangeState(GameState state)
        {
            currentState = state;
            timer = 0f;

            RefreshUI();

            Debug.Log($"게임 상태 : {currentState} / Wave : {currentWave}");
        }

        void RefreshUI()
        {
            stateText.text = $"State : {currentState}";
            waveText.text = $"Wave : {currentWave} / {totalWaves}";

            failButton.interactable = currentState == GameState.WAVE_PROGRESS;
            pauseButton.interactable = currentState != GameState.PAUSE;

            switch (currentState)
            {
                case GameState.STAGE_INTRO:
                    ShowPopup($"Stage : {roomData.stageNumber:00}", $"Enemy Count : {roomData.spawnCount}\nStart Gold : {roomData.requiredGold}", "Start");
                    break;

                case GameState.WAVE_READY:
                    ShowPopup("Wave Ready", Mathf.Max(1, Mathf.CeilToInt(3f - timer)).ToString(), "");
                    break;

                case GameState.STAGE_CLEAR:
                    ShowPopup("Stage Clear", "모든 웨이브를 완료했습니다.", "");
                    break;

                case GameState.STAGE_FAILED:
                    ShowPopup("Stage Failed", "스테이지에 실패했습니다.", "");
                    break;

                case GameState.PAUSE:
                    ShowPopup("Pause", "일시정지 중입니다.", "Resume");
                    break;

                default:
                    popupRoot.SetActive(false);
                    break;
            }
        }

        void ShowPopup(string title, string content, string buttonName)
        {
            popupTitleText.text = title;
            popupContentText.text = content;
            popupButtonText.text = buttonName;

            popupButton.gameObject.SetActive(buttonName != "");
            popupRoot.SetActive(true);
        }

        public void OnPopupButton()
        {
            switch (currentState)
            {
                case GameState.STAGE_INTRO:
                    ChangeState(GameState.WAVE_READY);
                    break;

                case GameState.PAUSE:
                    currentState = beforePauseState;
                    RefreshUI();
                    Debug.Log($"게임 재개 : {currentState}");
                    break;
            }
        }

        public void FailStage()
        {
            if (currentState != GameState.WAVE_PROGRESS) return;

            ChangeState(GameState.STAGE_FAILED);
        }

        public void PauseGame()
        {
            if (currentState == GameState.PAUSE) return;

            beforePauseState = currentState;
            currentState = GameState.PAUSE;

            RefreshUI();

            Debug.Log($"일시정지 : {beforePauseState}");
        }
    }
}