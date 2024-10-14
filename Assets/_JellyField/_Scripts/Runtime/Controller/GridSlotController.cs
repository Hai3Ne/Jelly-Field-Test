using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controller
{
    // structure
   
    
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransformSlot;
        [SerializeField] private GameObject objSlot;
        
        [SerializeField] private GridLayoutGroup slotsLayoutGrid;
        [SerializeField] private Structure.SettingGridSlot settingGrid;
        [SerializeField] private List<Structure.LockedSlot> lockedSlot;
        private GridSlot[,] _slotView;
        private void Start()
        {
            GameObjectUtils.Instance.ClearAllChild(rectTransformSlot.gameObject);
            objSlot.SetActive(false);
            InitSettingSlot();
            CreateSlot();
        }

        private void InitSettingSlot()
        {
            _slotView = new GridSlot[settingGrid.rows,settingGrid.columns];
        }

        private void CreateSlot()
        {
            for (int y = 0; y < settingGrid.rows; y++)
            {
                for (int x = 0; x < settingGrid.columns; x++)
                {
                    var checkLock = lockedSlot.Any(type => type.lockX == x && type.lockY == y);
                    GameObject slotObject = GameObjectUtils.Instance.SpawnGameObject(rectTransformSlot,objSlot);
                    GridSlot slot = slotObject.GetComponent<GridSlot>();
                    slot.SetData(checkLock);
                    _slotView[y, x] = slot;
                    slotObject.name = $"Slot [{y},{x}]";
                    slotObject.SetActive(true);
                    
                }
            }
        }
    }
}
