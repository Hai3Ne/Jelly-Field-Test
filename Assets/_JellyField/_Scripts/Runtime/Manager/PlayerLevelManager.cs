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
            if(loadLevel > levelSos.Count)
                PlayerPrefs.SetInt(levelKey, 1);
            else
                PlayerPrefs.SetInt(levelKey, loadLevel);
            PlayerPrefs.Save();
        }
        public int LoadLevel()
        {
            int level = PlayerPrefs.GetInt(levelKey, 1);
            Debug.Log("Loaded level: " + level);
            if (level > levelSos.Count)
            {
                SaveLevel(1);
            }
            return level > levelSos.Count ? 1 : level;
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
                int savedLevel = LoadLevel();
                Debug.Log("Loaded existing player level: " + savedLevel);
                return levelSos.FirstOrDefault(x => x.level == savedLevel);
            }
            
        }
        
    }
}