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

        public void Translate()
        {
            var translations = App.Current.Resources.MergedDictionaries.OfType<ResourceInclude>().FirstOrDefault(x => x.Source.OriginalString.Contains("Translations."));

            App.Current.Resources.MergedDictionaries.Remove(translations);


            App.Current.Resources.MergedDictionaries.Add(
                new ResourceInclude()
                {
                    Source = new Uri("avares://LocalizationSample/Assets/Lang/Translations.en-US.axaml")
                });
        }
    }
}
