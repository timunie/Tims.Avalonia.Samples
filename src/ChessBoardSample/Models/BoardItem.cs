using Avalonia.Media;

namespace ChessBoardSample.Models
{
    
    /// <summary>
    /// Represents a single rectangle on the chess board
    /// </summary>
    public class BoardItem
    {
        public BoardItem(int column, int row)
        {
            // Alternate the color based on position. 

            // even rows
            if (row % 2 == 0)
            {
                Color = column % 2 == 0 ? Colors.Aqua : Colors.Blue;
            }
            // uneven rows
            else
            {
                Color = column % 2 == 0 ? Colors.Blue : Colors.Aqua;
            }

            
        }
        
        /// <summary>
        ///  Must have a public getter
        /// </summary>
        public Color Color { get; }
    }
}