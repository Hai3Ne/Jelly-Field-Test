using System;
using System.Collections.Generic;
using Core;
using Core.Utils;
using Runtime.View;
using UnityEngine;

namespace Runtime.Manager
{
    public class JellyManager : ManualSingletonMono<JellyManager>
    {
        [SerializeField] private RectTransform rectTransformParent;
        [SerializeField] private GameObject objSlot;
        [SerializeField] private short numberSpawn;
        private List<JellySlot> _listJellySlot = new List<JellySlot>();
        private void Start()
        {
            objSlot.SetActive(false);
            CreateSlot();
        }
        void CreateSlot()
        {
            GameObjectUtils.Instance.ClearAllChild(rectTransformParent.gameObject);
            for (int i = 0; i < numberSpawn; i++)
            {
                GameObject slot = GameObjectUtils.Instance.SpawnGameObject(rectTransformParent, objSlot);
                JellySlot script = slot.GetComponent<JellySlot>();
                script.SetOwnerSlotData();
                slot.SetActive(true);
                if(!_listJellySlot.Contains(script))
                    _listJellySlot.Add(script);
            }
        }

        public void CheckOwnerSlot()
        {
            foreach (var jelly in _listJellySlot)
            {
                if(!jelly.JellyView)
                    jelly.SpawnObjectJelly();
            }
        }
    }
}