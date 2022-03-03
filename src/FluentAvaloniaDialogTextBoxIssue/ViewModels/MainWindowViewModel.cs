using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace FluentAvaloniaDialogTextBoxIssue.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public async Task ShowDialogAsync()
        {
            
            var dialog = new ContentDialog()
            {
                Title = "Type some Text", Content = new TextBox()
            };

            await dialog.ShowAsync();
        }
    }
}