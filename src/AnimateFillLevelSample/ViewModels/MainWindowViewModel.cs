using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimateFillLevelSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private double _FillLevel = 75;

        /// <summary>
        /// Gets or sets the current FillLevel in a Range [0..199]
        /// </summary>
        public double FillLevel
        {
            get { return _FillLevel; }
            set { this.RaiseAndSetIfChanged(ref _FillLevel, value); }
        }
    }
}
