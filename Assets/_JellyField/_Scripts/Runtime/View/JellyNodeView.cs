using System.Collections.Generic;
using Runtime.Animation;
using Runtime.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.View
{
    public class JellyNodeView : MonoBehaviour
    {
        [SerializeField] private Image colorNode;
        [SerializeField] private Renderer matNode;
        [SerializeField] private SoftCube softCube;
        private List<(int, int)> _neighborNode = new List<(int, int)>();
        public short X { get; set; }
        public short Y { get; set; }
        
        private JellyNode _data;
        public JellyNode Data => _data;
        public List<(int, int)> NeighborNode => _neighborNode;
        public void SetData(JellyNode data, List<(int, int)> neighbor)
        {
            _data = data;
            colorNode.color = ColorForJelly(_data.Color);
            _neighborNode = neighbor;
            matNode.material.color = ColorForJelly(_data.Color);
        }
        
        public void ChangeColor(JellyColor color)
        {
            _data.Color = color;
            softCube.ScaleUp();
            UpdateState();
        }
        public void ClearColor()
        {
            _data.Color = JellyColor.None;
            softCube.ScaleDown();
            UpdateState();
        }
        private Color ColorForJelly(JellyColor color)
        {
            switch (color)
            {
                case JellyColor.Red:
                    return Color.red;
                case JellyColor.Green:
                    return Color.green;
                case JellyColor.Blue:
                    return Color.blue;
                case JellyColor.Yellow:
                    return Color.yellow;
                case JellyColor.Purple:
                    return new Color(0.5f, 0, 0.5f);  // Màu tím
                default:
                    return Color.clear;
            }
        }

        private void UpdateState()
        {
            colorNode.color = ColorForJelly(_data.Color);
            matNode.material.color = ColorForJelly(_data.Color);
        }
        
        
    }
}