using System;
using System.ComponentModel;
using System.Windows;

namespace CM3D2.MaidFiddler.WPF.Translations
{
    public class TranslationData : IWeakEventListener, IDisposable, INotifyPropertyChanged
    {
        private readonly string key;

        public TranslationData(string key)
        {
            this.key = key;
            LanguageChangedEventManager.AddListener(TranslationManager.Instance, this);
        }

        public object Value => TranslationManager.Instance.Translate(key);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(LanguageChangedEventManager))
            {
                OnLanguageChanged(sender, e);
                return true;
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                LanguageChangedEventManager.RemoveListener(TranslationManager.Instance, this);
        }

        private void OnLanguageChanged(object sender, EventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

        ~TranslationData()
        {
            Dispose(false);
        }
    }
}