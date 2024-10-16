using System;
using Core;
using UnityEngine;

namespace Runtime.Manager
{
    public class PopupManager : ManualSingletonMono<PopupManager>
    {
        [SerializeField] private GameObject popupWin;
        private void Start()
        {
            popupWin.SetActive(false);
        }

        public void ShowPopup()
        {
            popupWin.SetActive(true);
        }
        public void OnClickNext()
        {
           PlayerLevelManager.Instance.NextLevel();
           GamePlayManager.Instance.InitLevel();
        }

        public void OnClickReplay()
        {
            GamePlayManager.Instance.InitLevel();
        }
    }
}