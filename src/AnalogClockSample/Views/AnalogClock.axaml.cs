using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

// ReSharper disable InconsistentNaming

namespace AnalogClockSample.Views
{
    public class AnalogClock : TemplatedControl
    {
        /// <summary>
        ///     Defines the <see cref="SelectedDateTime" /> property.
        /// </summary>
        public static readonly StyledProperty<DateTime> SelectedDateTimeProperty =
            AvaloniaProperty.Register<AnalogClock, DateTime>(nameof(SelectedDateTime), DateTime.Now);

        private IControl? PART_HourHand;
        private IControl? PART_MinuteHand;
        private IControl? PART_SecondHand;

        /// <summary>
        ///     The selected date and time to display
        /// </summary>
        public DateTime SelectedDateTime
        {
            get => GetValue(SelectedDateTimeProperty);
            set => SetValue(SelectedDateTimeProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            PART_HourHand = e.NameScope.Find<IControl>(nameof(PART_HourHand));
            PART_MinuteHand = e.NameScope.Find<IControl>(nameof(PART_MinuteHand));
            PART_SecondHand = e.NameScope.Find<IControl>(nameof(PART_SecondHand));

            UpdateAngles();

            base.OnApplyTemplate(e);
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change) 
        {
            base.OnPropertyChanged(change);
            
            if (change.Property == SelectedDateTimeProperty) UpdateAngles();
        }
        
        private void UpdateAngles()
        {
            if (PART_HourHand is not null)
                PART_HourHand.RenderTransform = new RotateTransform(SelectedDateTime.TimeOfDay.TotalMinutes * 0.5);

            if (PART_MinuteHand is not null)
                PART_MinuteHand.RenderTransform = new RotateTransform(SelectedDateTime.TimeOfDay.TotalMinutes * 6);

            if (PART_SecondHand is not null)
                PART_SecondHand.RenderTransform = new RotateTransform(SelectedDateTime.TimeOfDay.Seconds * 6);
        }
    }
}