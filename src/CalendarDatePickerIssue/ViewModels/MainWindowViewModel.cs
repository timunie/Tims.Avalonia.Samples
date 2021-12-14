using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarDatePickerIssue.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private DateTime? _MinDisplayDate;

        /// <summary>
        /// Gets or sets the minimum display date
        /// </summary>
        public DateTime? MinDisplayDate
        {
            get { return _MinDisplayDate; }
            set { this.RaiseAndSetIfChanged(ref _MinDisplayDate, value); }
        }


        private DateTime? _MaxDisplayDate;

        /// <summary>
        /// Gets or sets the maximum display date
        /// </summary>
        public DateTime? MaxDisplayDate
        {
            get { return _MaxDisplayDate; }
            set { this.RaiseAndSetIfChanged(ref _MaxDisplayDate, value); }
        }


        private DateTime? _SelectedDate;

        /// <summary>
        /// Gets or sets the selected Date
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return _SelectedDate; }
            set { this.RaiseAndSetIfChanged(ref _SelectedDate, value); }
        }
    }
}
