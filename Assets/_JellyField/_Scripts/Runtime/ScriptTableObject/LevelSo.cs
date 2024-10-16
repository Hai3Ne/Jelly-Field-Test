using System;
using System.Collections.Generic;
using Runtime.Model;
using UnityEngine;

namespace Runtime.ScriptTableObject
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Jelly/LevelData", order = 1)]
    public class LevelSo : ScriptableObject
    {
        public uint level;
        public List<Structure.LockedSlot> lockedSlot;
        public List<LevelMission> mission = new List<LevelMission>();
        public Structure.SettingGridSlot settingGrid;
    }

    [Serializable]
    public class LevelMission
    {
        public int countColorMission;
        public JellyColor colorMission;
    }
}
