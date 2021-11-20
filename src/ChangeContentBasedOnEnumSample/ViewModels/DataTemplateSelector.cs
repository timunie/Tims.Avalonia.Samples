using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeContentBasedOnEnumSample.ViewModels
{
    public class DataTemplateSelector : IDataTemplate
    {
        [Content]
        public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new ();


        public IControl Build(object param)
        {
            return AvailableTemplates[param.ToString() ?? throw new ArgumentNullException(nameof(param))].Build(param);
        }

        public bool Match(object data)
        {
            var key = data.ToString();
            return !string.IsNullOrEmpty(key) && AvailableTemplates.ContainsKey(key);
        }
    }
}
