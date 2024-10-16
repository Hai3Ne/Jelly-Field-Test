using System;
using Core;
using Runtime.Controller;
using Unity.VisualScripting;

namespace Runtime.Manager
{
    public class GamePlayManager : ManualSingletonMono<GamePlayManager>
    {
        private void Start()
        {
            MissionManager.Instance.LoadLevel();
            JellySlotController.Instance.OnStartGame();
        }


        public void InitLevel()
        {
            MissionManager.Instance.LoadLevel();
            JellySlotController.Instance.OnStartGame();
        }
    }
}