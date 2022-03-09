using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Threading;
using ReactiveUI;

namespace AnalogClockSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            DispatcherTimer timer =
                new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Background, Timer_Tick);
            
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged(nameof(CurrentDateTime));
        }

        public DateTime CurrentDateTime => DateTime.Now;
    }
}