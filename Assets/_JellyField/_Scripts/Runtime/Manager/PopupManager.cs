using System;
using Core;
using TMPro;
using UnityEngine;

namespace Runtime.Manager
{
    public class PopupManager : ManualSingletonMono<PopupManager>
    {
        [SerializeField] private GameObject popupWin;
        [SerializeField] private TextMeshProUGUI textState;
        [SerializeField] private GameObject objClose;
        [SerializeField] private GameObject objNextLevel;
        [SerializeField] private GameObject objReplayLevel;
        private void Start()
        {
            popupWin.SetActive(false);
        }

        public void ShowPopup()
        {
            popupWin.SetActive(true);
        }
        public void HidePopup()
        {
            popupWin.SetActive(false);
        }
        public void OnClickNext()
        {
           PlayerLevelManager.Instance.NextLevel();
           GamePlayManager.Instance.InitLevel();
           HidePopup();
        }

        public void OnClickReplay()
        {
            GamePlayManager.Instance.InitLevel();
            HidePopup();
        }

        public void OnWin()
        {
            ShowPopup();
            textState.text = $"You Win";
            objReplayLevel.SetActive(true);
            objNextLevel.SetActive(true);
        }
        public void OnLose()
        {
            ShowPopup();
            textState.text = $"You Lose";
            objReplayLevel.SetActive(true);
            objNextLevel.SetActive(false);
        }
    }
}