using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Utils;
using Runtime.Manager;
using Runtime.Model;
using Runtime.ScriptTableObject;
using Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controller
{
    public class JellySlotController : ManualSingletonMono<JellySlotController>
    {
        //structure Process JellySLot
        private class ProcessJellySlot
        {
            public JellyColor TargetColor;
            public JellySlot TargetSlot;
            public JellySlot CurrentSlot;
            public short XTargetSlot;
            public short YTargetSlot;
            public short XCurrentSlot;
            public short YCurrentSlot;
        }
        [SerializeField] private RectTransform rectTransformSlot;
        [SerializeField] private GameObject objSlot;
        [SerializeField] private GridLayoutGroup slotsLayoutGrid;
        private Structure.SettingGridSlot _settingGrid;
        private List<Structure.LockedSlot> _lockedSlot;
        private Structure.SlotType[,] _slotArray;
        private JellySlot[,] _gridSlots;
        public JellySlot[,] GridSlots => _gridSlots;
        public Vector2Int MaxSlotXY
        {
            get
            {
                int x = _slotArray.GetLength(1);
                int y = _slotArray.GetLength(0);
                return new(x, y);
            }
        }
        public JellySlot this[int y, int x]
        {
            get
            {
                try
                {
                    return _gridSlots[y, x];
                }
                catch
                {
                    return null;
                }
            }
        }
        public void OnStartGame()
        {
            GameObjectUtils.Instance.ClearAllChild(rectTransformSlot.gameObject);
            objSlot.SetActive(false);
            InitSettingSlot();
            CreateSlot();
        }

        private void InitSettingSlot()
        {
            LevelSo getSoLevel = PlayerLevelManager.Instance.GetCurrentLevelSo();
            _settingGrid = getSoLevel.settingGrid;
            _lockedSlot = getSoLevel.lockedSlot;
            _gridSlots = new JellySlot[_settingGrid.rows,_settingGrid.columns];
            _slotArray = new Structure.SlotType[_settingGrid.rows, _settingGrid.columns];
        }

        private void CreateSlot()
        {
            for (int y = 0; y < _settingGrid.rows; y++)
            {
                for (int x = 0; x < _settingGrid.columns; x++)
                {
                    var checkLock = _lockedSlot.Any(type => type.lockX == x && type.lockY == y);
                    GameObject slotObject = GameObjectUtils.Instance.SpawnGameObject(rectTransformSlot,objSlot);
                    JellySlot slot = slotObject.GetComponent<JellySlot>();
                    slot.SetPlaySlotData(checkLock);
                    slot.XMatrix = (short)x;
                    slot.YMatrix = (short)y;
                    _gridSlots[y, x] = slot;
                    slotObject.name = $"Slot [{y},{x}]";
                    slotObject.SetActive(true);
                    _slotArray[y, x] = checkLock ? Structure.SlotType.Locked : Structure.SlotType.Inventory;
                }
            }
        }
        public bool IsCoordsValid(int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 ||
                x >= MaxSlotXY.x || y >= MaxSlotXY.y ||
                x + (width - 1) >= MaxSlotXY.x || y + (height - 1) >= MaxSlotXY.y)
                return false;
            return true;
        }

        public void CheckNodeJelly(JellySlot slot)
        {
            
            // above 
            if (slot.YMatrix - 1 >= 0)
            {
               
                if(IsSlotCurrent((slot.XMatrix),(short)(slot.YMatrix -1)))
                {
                    ProcessJellySlot process = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix - 1, slot.XMatrix],
                        CurrentSlot = slot,
                        XTargetSlot = 0,
                        YTargetSlot = 1,
                        XCurrentSlot = 0,
                        YCurrentSlot = 0,
                    };
                    ProcessJellySlot process2 = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix - 1, slot.XMatrix],
                        CurrentSlot = slot,
                        XTargetSlot = 1,
                        YTargetSlot = 1,
                        XCurrentSlot = 1,
                        YCurrentSlot = 0,
                    };
                    ProcessMatrixJelly(process);
                    ProcessMatrixJelly(process2);
                }
            }
            // bottom
            if (slot.YMatrix + 1 < _settingGrid.rows)
            {
                if(IsSlotCurrent((slot.XMatrix),(short)(slot.YMatrix + 1)))
                {
                    ProcessJellySlot process = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix + 1, slot.XMatrix],
                        CurrentSlot = slot,
                        XTargetSlot = 0,
                        YTargetSlot = 0,
                        XCurrentSlot = 0,
                        YCurrentSlot = 1,
                    };
                    ProcessJellySlot process2 = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix + 1, slot.XMatrix],
                        CurrentSlot = slot,
                        XTargetSlot = 1,
                        YTargetSlot = 0,
                        XCurrentSlot = 1,
                        YCurrentSlot = 1,
                    };
                    ProcessMatrixJelly(process);
                    ProcessMatrixJelly(process2);
                }
            }
            // // left
            if (slot.XMatrix - 1 >= 0)
            {
                if (IsSlotCurrent((short)(slot.XMatrix - 1),slot.YMatrix))
                {
                    ProcessJellySlot process = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix, slot.XMatrix - 1],
                        CurrentSlot = slot,
                        XTargetSlot = 1,
                        YTargetSlot = 0,
                        XCurrentSlot = 0,
                        YCurrentSlot = 0,
                    };
                    ProcessJellySlot process2 = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix, slot.XMatrix - 1],
                        CurrentSlot = slot,
                        XTargetSlot = 1,
                        YTargetSlot = 1,
                        XCurrentSlot = 0,
                        YCurrentSlot = 1,
                    };
                    ProcessMatrixJelly(process);
                    ProcessMatrixJelly(process2);
                }
                   
            }
            // // right
            if (slot.XMatrix + 1 < _settingGrid.columns)
            {
                if (IsSlotCurrent((short)(slot.XMatrix + 1), slot.YMatrix))
                {
                    ProcessJellySlot process = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix, slot.XMatrix + 1],
                        CurrentSlot = slot,
                        XTargetSlot = 0,
                        YTargetSlot = 0,
                        XCurrentSlot = 1,
                        YCurrentSlot = 0,
                    };
                    ProcessJellySlot process2 = new ProcessJellySlot()
                    {
                        TargetSlot = _gridSlots[slot.YMatrix, slot.XMatrix + 1],
                        CurrentSlot = slot,
                        XTargetSlot = 0,
                        YTargetSlot = 1,
                        XCurrentSlot = 1,
                        YCurrentSlot = 1,
                    };
                    ProcessMatrixJelly(process);
                    ProcessMatrixJelly(process2);
                }
                
            }
        }

        private void ProcessMatrixJelly(ProcessJellySlot process)
        {
            if (!process.TargetSlot.JellyView || !process.CurrentSlot.JellyView)
                return;
            JellyNodeView[,] gridNodeTarget = process.TargetSlot.JellyView.GridNode;
            JellyNodeView getTargetJelly = gridNodeTarget[process.YTargetSlot, process.XTargetSlot];
            JellyNodeView currentSlot = process.CurrentSlot.JellyView.GridNode[process.YCurrentSlot, process.XCurrentSlot];
            if (getTargetJelly.Data.Color == currentSlot.Data.Color)
            {
                MissionManager.Instance.UpdateStateMission(getTargetJelly.Data.Color);
                process.TargetSlot.JellyView.DealWithColor(getTargetJelly.Data.Color, process.XTargetSlot, process.YTargetSlot);
                process.CurrentSlot.JellyView.DealWithColor(currentSlot.Data.Color, process.XCurrentSlot, process.YCurrentSlot);
            }
        }
        private bool IsSlotCurrent(short x, short y)
        {
            return  this[y,x].JellyView;
        }
    }
}
