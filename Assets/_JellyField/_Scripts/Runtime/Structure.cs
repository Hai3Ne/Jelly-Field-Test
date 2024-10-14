using System;

namespace Runtime
{
    public class Structure
    {
        [Serializable]
        public class SettingGridSlot
        {
            public short rows;
            public short columns;
        }
        
        [Serializable]
        public class LockedSlot
        {
            public short lockX;
            public short lockY;
        }
    }
}