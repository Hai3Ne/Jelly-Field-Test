using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Utils;
using Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controller
{
    public class JellySlotController : ManualSingletonMono<JellySlotController>
    {
        [SerializeField] private RectTransform rectTransformSlot;
        [SerializeField] private GameObject objSlot;
        [SerializeField] private GridLayoutGroup slotsLayoutGrid;
        [SerializeField] private Structure.SettingGridSlot settingGrid;
        [SerializeField] private List<Structure.LockedSlot> lockedSlot;
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
        private void Start()
        {
            GameObjectUtils.Instance.ClearAllChild(rectTransformSlot.gameObject);
            objSlot.SetActive(false);
            InitSettingSlot();
            CreateSlot();
        }

        private void InitSettingSlot()
        {
            _gridSlots = new JellySlot[settingGrid.rows,settingGrid.columns];
            _slotArray = new Structure.SlotType[settingGrid.rows, settingGrid.columns];
        }

        private void CreateSlot()
        {
            for (int y = 0; y < settingGrid.rows; y++)
            {
                for (int x = 0; x < settingGrid.columns; x++)
                {
                    var checkLock = false;//lockedSlot.Any(type => type.lockX == x && type.lockY == y);
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
            
            List<JellySlot> checkCurrentSlot = new List<JellySlot>();
            
            // above 
            if (slot.XMatrix - 1 >= 0)
            {
                if(IsSlotCurrent((short)(slot.XMatrix - 1),slot.YMatrix))
                {
                    JellySlot slotAbove = _gridSlots[slot.YMatrix, slot.XMatrix - 1];
                    var gridNode = slotAbove.JellyView.GridNode;
                    var getBotLeft = gridNode[1, 0];
                    var currentSlot = slot.JellyView.GridNode[0, 0];
                    if (getBotLeft.Data.Color == currentSlot.Data.Color)
                    {
                        // slotAbove.JellyView.CheckAndGetNeighbors();
                        getBotLeft.ClearColor();
                        currentSlot.ClearColor();
                    }
                    // var getBotRight = gridNode[1, 1].Data.Color;
                    // checkCurrentSlot.Add(_gridSlots[slot.YMatrix, slot.XMatrix - 1]);
                    // slot.JellyView.CheckMergeColorAbove(_gridSlots[slot.YMatrix, slot.XMatrix - 1]);

                }
            }
            // bottom
            if (slot.XMatrix + 1 < settingGrid.rows)
            {
                if(IsSlotCurrent((short)(slot.XMatrix + 1),slot.YMatrix))
                    checkCurrentSlot.Add(_gridSlots[slot.YMatrix, slot.XMatrix + 1]);
            }
            // left
            if (slot.YMatrix - 1 >= 0)
            {
                if(IsSlotCurrent(slot.XMatrix,(short)(slot.YMatrix - 1)))
                    checkCurrentSlot.Add(_gridSlots[slot.YMatrix - 1, slot.XMatrix]);
            }
            // right
            if (slot.YMatrix + 1 < settingGrid.columns)
            {
                if(IsSlotCurrent(slot.XMatrix,(short)(slot.YMatrix + 1)))
                    checkCurrentSlot.Add(_gridSlots[slot.YMatrix + 1, slot.XMatrix]);
            }
        }

        private bool IsSlotCurrent(short x, short y)
        {
            return  this[y,x].JellyView;
        }
    }
}
