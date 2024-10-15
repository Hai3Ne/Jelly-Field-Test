using System;

namespace Runtime.Model
{
    public class JellyModel
    {
        public short InsideRows { get; private set; }
        public short InsideColumns { get; private set;}
        public JellyNode[,] InsideGrid { get; private set; }

        public JellyModel(short rows, short columns){
            InsideRows = rows;
            InsideColumns = columns;
            InsideGrid = new JellyNode[rows, columns];
            InitializeInsideGrid();
        }
        
        
        private void InitializeInsideGrid()
        {
            for (int y = 0; y < InsideRows; y++)
            {
                for (int x = 0; x < InsideColumns; x++)
                {
                    InsideGrid[y, x] = new JellyNode(GetRandomColor());
                }
            }
        }
        private JellyColor GetRandomColor()
        {
            var colors = Enum.GetValues(typeof(JellyColor));
            return (JellyColor)colors.GetValue(UnityEngine.Random.Range(1, colors.Length));
        }

    }
    public class JellyNode
    {
        public JellyColor Color { get; set; }
        public JellyNode(JellyColor color)
        {
            Color = color;
        }
        
        
    }
    public enum JellyColor
    {
        None = 0,
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }

    
}