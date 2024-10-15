using Runtime.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.View
{
    public class JellyNodeView : MonoBehaviour
    {
        [SerializeField] private Image colorNode;
        public short X { get; set; }
        public short Y { get; set; }
        
        private JellyNode _data;
        public JellyNode Data => _data;
        public void SetData(JellyNode data)
        {
            _data = data;
            colorNode.color = ColorForJelly(_data.Color);
        }

        public void MergeColor()
        {
           
        }

        public void ClearColor()
        {
            _data.Color = JellyColor.None;
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
        }
    }
}