using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Utils;
using Runtime.Model;
using Runtime.ScriptTableObject;
using Runtime.View;
using UnityEngine;

namespace Runtime.Manager
{
    public class MissionManager : ManualSingletonMono<MissionManager>
    {
        [SerializeField] private GameObject objPrefabMission;
        [SerializeField] private RectTransform rectTransformMission;
        private List<MissionVo> _listMission = new List<MissionVo>();
        private void Start()
        {
            objPrefabMission.SetActive(false);
        }

        public void LoadLevel()
        {
            LevelSo getSoLevel = PlayerLevelManager.Instance.GetCurrentLevelSo();
            GameObjectUtils.Instance.ClearAllChild(rectTransformMission.gameObject);
            foreach (var mission in getSoLevel.mission)
            {
                GameObject obj = GameObjectUtils.Instance.SpawnGameObject(rectTransformMission, objPrefabMission);
                MissionVo script = obj.GetComponent<MissionVo>();
                script.SetData(mission);
                obj.SetActive(true);
                if(!_listMission.Contains(script))
                    _listMission.Add(script);
            }
        }

        public void UpdateStateMission(JellyColor color)
        {
            var mission = _listMission.FirstOrDefault(x => x.Data.colorMission == color);
            if (mission != null) mission.UpdateStateMission();
        }

    }
}
