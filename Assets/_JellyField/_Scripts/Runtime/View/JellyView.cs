using System;
using System.Collections.Generic;
using Core.Utils;
using Runtime.Model;
using UnityEngine;
using System.Linq;
using Runtime.Controller;
using Runtime.Manager;
using Random = UnityEngine.Random;

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
                        script.SetData(_dataJelly.InsideGrid[y,x], GetNeighbors(y,x));
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

        public void DealWithColor(JellyColor color,short x, short y)
        {
            MissionManager.Instance.UpdateStateMission(color);
            _gridNode[y,x].ClearColor();
            bool allPositive = _gridNode.Cast<JellyNodeView>().All(node => node.Data.Color == color);
            //check same color
            if (allPositive)
            {
                foreach (var node in _gridNode)
                    node.ClearColor();
                CheckClearJellyColor();
                return;
            }
            var checkNeighbors = _gridNode[y,x].NeighborNode;
            foreach (var node in checkNeighbors)
            {
                var colorNeighbors = _gridNode[node.Item1, node.Item2];
                if (colorNeighbors.Data.Color == color)
                {
                    var valueTuples = colorNeighbors.NeighborNode.Where(n => _gridNode[n.Item1, n.Item2].Data.Color == color)
                        .ToList();
                    valueTuples.ForEach(n =>_gridNode[n.Item1,n.Item2].ClearColor());
                    colorNeighbors.ClearColor();
                }
            }
            if(!CheckClearJellyColor())
             ProcessEmptyColor();
        }

        private bool CheckClearJellyColor()
        {
            bool allPositive = _gridNode.Cast<JellyNodeView>().All(node => node.Data.Color == JellyColor.None);
            if (!allPositive)
                return false;
            foreach (var slot in JellySlotController.Instance.GridSlots)
            {
                if (slot.JellyView == this)
                    slot.RemoveJellyView();
            }
            this.gameObject.SetActive(false);
            return true;
        }

        private void ProcessEmptyColor()
        {
            var countActiveColor = _gridNode.Cast<JellyNodeView>().Where(node => node.Data.Color != JellyColor.None).ToList();
            if (countActiveColor.Count() == 1)
            {
                foreach (var node in _gridNode)
                {
                    if(node.Data.Color == JellyColor.None)
                        node.ChangeColor(countActiveColor.FirstOrDefault()!.Data.Color);
                }
                return;
            }
            foreach (var node in _gridNode)
            {
                if (node.Data.Color == JellyColor.None)
                {
                    var neighborCurrent = node.NeighborNode
                        .Where(n => _gridNode[n.Item1, n.Item2].Data.Color != JellyColor.None).ToList();
                    (int randomRow, int randomCol) = neighborCurrent.OrderBy(x => Random.value).FirstOrDefault();
                    var valueColor = _gridNode[randomRow, randomCol].Data.Color;
                    node.ChangeColor(valueColor);
                }
            }
        }
        private List<(int, int)> GetNeighbors(int row, int col)
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
            return neighbors;

        }
    
    }
}