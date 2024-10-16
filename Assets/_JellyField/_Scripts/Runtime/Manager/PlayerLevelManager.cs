using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Runtime.ScriptTableObject;
using UnityEngine;

namespace Runtime.Manager
{
    public class PlayerLevelManager : ManualSingletonMono<PlayerLevelManager>
    {
        [SerializeField]
        List<LevelSo> levelSos = new List<LevelSo>();
        private const string levelKey = "PlayerLevel";

        public string LevelKey => levelKey;
        
        public void SaveLevel(int level)
        {
            PlayerPrefs.SetInt(LevelKey, level);
            PlayerPrefs.Save();
            Debug.Log("Level saved: " + level);
        }
        public void NextLevel()
        {
            var loadLevel = LoadLevel() + 1;
            PlayerPrefs.SetInt(LevelKey, loadLevel);
            PlayerPrefs.Save();
        }
        public int LoadLevel()
        {
            int level = PlayerPrefs.GetInt(LevelKey, 1);
            Debug.Log("Loaded level: " + level);
            return level;
        }
        public void ResetLevel()
        {
            PlayerPrefs.DeleteKey(LevelKey);
            Debug.Log("Level data reset.");
        }
        
        public LevelSo GetCurrentLevelSo()
        {
            if (!PlayerPrefs.HasKey(levelKey))
            {
                Debug.Log("New player detected. Starting at level 1.");
                SaveLevel(1);
                return levelSos[0];
            }
            else
            {
                int savedLevel = PlayerPrefs.GetInt(levelKey);
                Debug.Log("Loaded existing player level: " + savedLevel);
                return levelSos.FirstOrDefault(x => x.level == savedLevel);
            }
            
        }
        
    }
}