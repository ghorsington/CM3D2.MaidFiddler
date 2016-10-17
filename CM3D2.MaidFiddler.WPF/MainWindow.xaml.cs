using System.Windows;
using CM3D2.MaidFiddler.WPF.Translations;

namespace CM3D2.MaidFiddler.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string VERSION = "WPF BETA 0.1";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ToolTitle = $"Maid Fiddler";

            LanguageChangedEventManager.AddHandler(TranslationManager.Instance,
                                                   (sender, args) =>
                                                   {
                                                       ToolTitle =
                                                           $"Maid Fiddler {VERSION} {TranslationManager.Instance.Translate("TITLE_TEXT")}";
                                                   });
            TranslationManager.Instance.LoadTranslation("ENG");
        }

        public string ToolTitle { get; set; }
    }
}