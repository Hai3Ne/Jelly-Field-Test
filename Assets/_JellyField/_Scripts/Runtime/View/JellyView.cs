using System;
using System.Collections.Generic;
using Core.Utils;
using Runtime.Model;
using UnityEngine;

namespace Runtime.View
{
    public class JellyView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject objJelly;
        
        private JellyNodeView[,] _gridNode;
        private JellyModel _dataJelly;
        public  JellyNodeView[,] GridNode => _gridNode; 
        public void OnInit()
        {
            objJelly.SetActive(false);
            CreateJelly();
            SetDataJelly();
        }
        public void CreateJelly()
        {
            _gridNode = new JellyNodeView[2, 2];
            _dataJelly = new JellyModel(2, 2);
        }

        public void CheckMergeColor(JellySlot slot)
        {
            
        }
        
        private void SetDataJelly()
        {
            if (_dataJelly != null)
            {
                GameObjectUtils.Instance.ClearAllChild(content.gameObject);
                for (int y = 0; y < _dataJelly.InsideRows; y++)
                {
                    for (int x = 0; x < _dataJelly.InsideColumns; x++)
                    {
                        GameObject obj = GameObjectUtils.Instance.SpawnGameObject(content, objJelly);
                        JellyNodeView script = obj.GetComponent<JellyNodeView>();
                        _gridNode[y, x] = script;
                        script.SetData(_dataJelly.InsideGrid[y,x]);
                        script.X = (short)x;
                        script.Y = (short)y;
                        obj.name = $"node: y:{y},x:{x}";
                        obj.SetActive(true);
                    }
                }
            }
            else
            {
                Debug.Log("Can not spawn Jelly Because data is null");
            }
        }
        public void CheckAndGetNeighbors(JellyColor color,int row, int col)
        {
            List<(int, int)> neighbors = new List<(int, int)>();
            
            if (row - 1 >= 0)
                neighbors.Add((row - 1, col));
            if (row + 1 < 2)
                neighbors.Add((row + 1, col));
            if (col - 1 >= 0)
                neighbors.Add((row, col - 1));
            if (col + 1 < 2)
                neighbors.Add((row, col + 1));
        }
    
    }
}