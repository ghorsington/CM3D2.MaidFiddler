using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace CM3D2.MaidFiddler.WPF.Translations
{
    public class TranslateExtension : MarkupExtension
    {
        public TranslateExtension(string key)
        {
            Key = key;
        }

        [ConstructorArgument("Key")]
        public string Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Binding bindng = new Binding("Value")
            {
                Source = new TranslationData(Key)
            };
            return bindng.ProvideValue(serviceProvider);
        }
    }
}