using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnimationFiredTooOftenIssue.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    { 
        public List<Item> Items { get; } = Enumerable.Range(0, 1000).Select(x => new Item { IsSelected = x % 1 == 0, Name = "Person " + x }).ToList();
    }

    public class Item
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; } = true;
    } 
}
