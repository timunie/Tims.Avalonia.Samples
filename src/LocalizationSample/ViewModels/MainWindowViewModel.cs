using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Platform;

namespace LocalizationSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public void Translate(string targetLanguage)
        {
            var translations = App.Current.Resources.MergedDictionaries.OfType<ResourceInclude>().FirstOrDefault(x => x.Source?.OriginalString?.Contains("/Lang/") ?? false);

            if (translations != null)
                App.Current.Resources.MergedDictionaries.Remove(translations);

            // var resource = AssetLoader.Open(new Uri($"avares://LocalizationSample/Assets/Lang/{targetLanguage}.axaml"));
            
            App.Current.Resources.MergedDictionaries.Add(
                new ResourceInclude(new Uri($"avares://LocalizationSample/Assets/Lang/{targetLanguage}.axaml"))
                {
                    Source = new Uri($"avares://LocalizationSample/Assets/Lang/{targetLanguage}.axaml")
                });
        }
    }
}
