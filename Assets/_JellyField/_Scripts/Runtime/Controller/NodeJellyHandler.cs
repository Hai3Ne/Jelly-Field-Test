using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Animation;
using Runtime.Manager;
using Runtime.View;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Runtime.Controller
{
    public class NodeJellyHandler : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private JellyView jellyView;
        [SerializeField]
        private List<SoftCube> listSoftCube = new List<SoftCube>();
        
        private Vector2 _dragOffset;
        private Vector2 _dragVelocity;
        private Vector2 _mousePosition;
        private Vector2 _mouseDelta;
        
        public bool isSelect = false;
        [HideInInspector] 
        public bool isOver = false;
        [HideInInspector] 
        public bool isMoving;
        

        private JellySlot _oldSlot;
        [Header("Debug")]
        [ReadOnly] public Vector2Int currentSlot = new Vector2Int(2,2);
        [ReadOnly] public Vector2Int lastSlot;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            isSelect = false;
            isOver = true;
            isMoving = false;
            listSoftCube = gameObject.GetComponentsInChildren<SoftCube>().ToList();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // if (!CheckAnyItemMoving())
            // {
            //     isOver = true;
            // }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            JellySlot script = transform.GetComponentInParent<JellySlot>();
            if ((!isOver || CheckAnyItemMoving()) || script.TypeSlot == JellySlot.TypeSlotEnum.PlaySlot)
                return;
            isSelect = true;
            isMoving = true;
            script.RemoveJellyView();
            _oldSlot = script;
            listSoftCube.ForEach(x=>x.ShakeLightOnce());
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            JellySlot slot = JellySlotController.Instance[currentSlot.y, currentSlot.x];
            JellySlot script = transform.GetComponentInParent<JellySlot>();
            if ((!isOver || CheckAnyItemMoving()) || script.TypeSlot == JellySlot.TypeSlotEnum.PlaySlot)
                return;
            if (slot.JellyView != null)
            {
                _oldSlot.SetNewParentAndData(jellyView);
                ClearAll();
                return;
            }
            slot.SetNewParentAndData(jellyView);
            JellyManager.Instance.CheckOwnerSlot();
            JellySlotController.Instance.CheckAllNode();
            listSoftCube.ForEach(x=>x.KillDoTween());
            ClearAll();

        }

        private void ClearAll()
        {
            for (int y = 0; y < JellySlotController.Instance.MaxSlotXY.y; y++)
            {
                for (int x = 0; x < JellySlotController.Instance.MaxSlotXY.x; x++)
                {
                    JellySlot s = JellySlotController.Instance[y, x];
                    s.ChangeBackGround(JellySlot.ColorBackgroundSlot.None);
                }
            }
            isSelect = false;
            isOver = false;
            isMoving = false;
        }
        private bool CheckAnyItemMoving()
        {
            return false;
        }
        private void Update()
        {
            if (isMoving && _mouseDelta.magnitude > 0)         
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform,_mousePosition,_camera,out Vector2 localPoint);
                Vector2 targetPosition = localPoint;//- _dragOffset;
                Vector2 newPosition = Vector2.SmoothDamp(rectTransform.localPosition,targetPosition,ref _dragVelocity, 0.03f);
                rectTransform.localPosition = newPosition;
                // for highlight
                Vector2Int dimensions = new Vector2Int(1,1);
                Vector2 dragPos = rectTransform.position;
                Vector2Int newSlotPosition = currentSlot;
                float distance = Mathf.Infinity;

                for (int y = 0; y < JellySlotController.Instance.MaxSlotXY.y; y++)
                {
                    for (int x = 0; x < JellySlotController.Instance.MaxSlotXY.x; x++)
                    {
                        JellySlot slot = JellySlotController.Instance[y, x];
                        slot.ChangeBackGround(JellySlot.ColorBackgroundSlot.None);
                        if (slot.IsLock) continue;
                        Vector2 slotPosition = slot.transform.position;

                        float slotDistance = Vector2.Distance(dragPos, slotPosition);

                        if (slotDistance < distance)
                        {
                            if (!JellySlotController.Instance.IsCoordsValid(x, y, dimensions.x, dimensions.y))
                                continue;
                            newSlotPosition = new Vector2Int(x, y);
                            distance = slotDistance;
                        }
                    }
                }
                currentSlot = newSlotPosition;
            }
            GetInput();
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void GetInput()
        {
            if (isOver || isMoving)
            {
                // get mouse position and mouse delta
                _mousePosition = Input.mousePosition;
                _mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                // item movement input
                if (isSelect)
                {
                    if (!isMoving)
                        isMoving = true;
                    CheckHighLightInventorySlot();
                    transform.SetAsLastSibling();
                }
            }
        }
        private void CheckHighLightInventorySlot()
        {
            JellySlot slot = JellySlotController.Instance[currentSlot.y, currentSlot.x];
            if (slot.IsLock) return;
            slot.ChangeBackGround(!slot.JellyView ? JellySlot.ColorBackgroundSlot.Green : JellySlot.ColorBackgroundSlot.Red);
        }
    }
}