using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


            App.Current.Resources.MergedDictionaries.Add(
                new ResourceInclude()
                {
                    Source = new Uri($"avares://LocalizationSample/Assets/Lang/{targetLanguage}.axaml")
                });
        }
    }
}
