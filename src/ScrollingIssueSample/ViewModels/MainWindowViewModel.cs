using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tims.Avalonia.Samples.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ItemsList = new List<string>();

            for (int i = 0; i < 20; i++)
            {
                ItemsList.Add($"Item number {i+1:00}");
            }
        }

        public string Greeting => "Welcome to Avalonia!";


        private int _SpinValue;

        /// <summary>
        /// Gets or sets a value for the button spinner demo.
        /// </summary>
        public int SpinValue
        {
            get { return _SpinValue; }
            set { this.RaiseAndSetIfChanged(ref _SpinValue, value); }
        }

        public List<string> ItemsList { get; private set; }
    }
}
