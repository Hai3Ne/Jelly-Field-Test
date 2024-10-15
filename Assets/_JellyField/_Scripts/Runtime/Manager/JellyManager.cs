using System;
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
            }
        }
            
    }
}