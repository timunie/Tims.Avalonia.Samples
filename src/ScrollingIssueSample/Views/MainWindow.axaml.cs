using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tims.Avalonia.Samples.ViewModels;

namespace Tims.Avalonia.Samples.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? ViewModel => this.DataContext as MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void DoSpin(object sender, SpinEventArgs e)
        {
            if (ViewModel is null) return;

            ViewModel!.SpinValue += (int)e.Direction;
        }
    }
}
