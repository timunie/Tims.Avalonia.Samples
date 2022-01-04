using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ReactiveUI;

namespace CalendarDatePickerValidationIssue.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DateTime? _RequiredDate;

        /// <summary>
        ///    A required Date with Validation support
        /// </summary>
        [Required]
        public DateTime? RequiredDate
        {
            get => _RequiredDate;
            set => this.RaiseAndSetIfChanged(ref _RequiredDate, value);
        }
    }
}