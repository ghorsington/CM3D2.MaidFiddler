using System.ComponentModel;
using System.Windows;
using CM3D2.MaidFiddler.WPF.Model;
using CM3D2.MaidFiddler.WPF.Translations;

namespace CM3D2.MaidFiddler.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public const string TITLE = "Maid Fiddler";
        public const string VERSION = "WPF BETA 0.1";
        public const string TITLE_NULL = TITLE + " " + VERSION;
        public const string TITLE_FORMAT = TITLE + " " + VERSION + " - {0}";

        public MainWindow()
        {
            SelectedMaid = new Maid();
            InitializeComponent();
            DataContext = this;

            LanguageChangedEventManager.AddHandler(TranslationManager.Instance,
                                                   (sender, args) => { OnPropertyChanged(nameof(TitleText)); });
            TranslationManager.Instance.LoadTranslation("ENG");
        }

        public Maid SelectedMaid { get; set; }

        public string TitleText => (string) TranslationManager.Instance.Translate("TITLE_TEXT");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}