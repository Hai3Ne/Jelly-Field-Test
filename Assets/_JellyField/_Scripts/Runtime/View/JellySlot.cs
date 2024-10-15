using System;
using Core.Utils;
using Runtime.Controller;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.View
{
    public class JellySlot : MonoBehaviour
    {
        public enum ColorBackgroundSlot
        {
            None = 0,
            Black = 1, 
            Red = 2,
            Green = 3,
        }
        public enum TypeSlotEnum
        {
            None = 0,
            OwnerSlot,
            PlaySlot
        }

        [SerializeField] private Image imgSlot;
        [SerializeField] private Color colorBlack;
        [SerializeField] private Color colorWhite;
        [SerializeField] private Color colorRed;
        [SerializeField] private Color colorGreen;
        [SerializeField] 
        private GameObject _jellyView;
        private TypeSlotEnum _typeSlot;
        public TypeSlotEnum TypeSlot => _typeSlot;
        public short XMatrix { get; set; }
        public short YMatrix { get; set; }
        public JellyView JellyView
        {
            get
            {
                if (_jellyView != null)
                    return _jellyView.GetComponent<JellyView>();
                return null;
            }
        }

        private ColorBackgroundSlot _colorBackgroundSlot;
        private bool _isLock;

        private void Start()
        {
            if(_jellyView !=null)
                _jellyView.SetActive(false);
        }

        public void SetPlaySlotData(bool isLock)
        {
            _isLock = isLock;
            imgSlot.enabled = !isLock;
            _typeSlot = TypeSlotEnum.PlaySlot;
        }

        public void SetOwnerSlotData()
        {
            // GameObjectUtils.Instance.ClearAllChild(this.gameObject);
            if (_jellyView == null)
                return;
            GameObject objJelly = GameObjectUtils.Instance.SpawnGameObject(this.transform, _jellyView);
            var script = objJelly.GetComponent<JellyView>();
            script.OnInit();
            objJelly.SetActive(true);
            _typeSlot = TypeSlotEnum.OwnerSlot;
        }
        public void ChangeBackGround(ColorBackgroundSlot colorE)
        {
            _colorBackgroundSlot = colorE;
            UpdateBackgroundColor();
        }
        public void SetNewParentAndData(JellyView data)
        {
            _jellyView = data.gameObject;
            _jellyView.transform.SetParent(this.transform);
            _jellyView.transform.localPosition = Vector3.zero;
            _jellyView.transform.localRotation = Quaternion.identity;
            // setPosition
            JellySlotController.Instance.CheckNodeJelly(this);
        }
        public void RemoveJellyView()
        {
            _jellyView = null;
            // if (_typeSlot == TypeSlotEnum.OwnerSlot && jellyView == null)
            // {
            //     SetOwnerSlotData();
            // }
        }
        private void UpdateBackgroundColor()
        {
            imgSlot.color = _colorBackgroundSlot switch
            {
                ColorBackgroundSlot.Black => colorBlack,
                ColorBackgroundSlot.Red => colorRed,
                ColorBackgroundSlot.Green => colorGreen,
                ColorBackgroundSlot.None => colorWhite,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
    }
}
