using System;
using System.Collections.Generic;
using System.Text;
using ChessBoardSample.Models;

namespace ChessBoardSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            // init the items. outer loop for rows, inner loop for columns
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardItems.Add(new BoardItem(j, i));
                }
            }
        }

        /// <summary>
        /// Gets a List of Available BoardItems
        /// </summary>
        public List<BoardItem> BoardItems { get; } = new List<BoardItem>();
    }
}