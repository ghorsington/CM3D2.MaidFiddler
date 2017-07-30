using System.Collections.Generic;
using System.ComponentModel;
using CM3D2.MaidFiddler.WPF.Translations;

namespace CM3D2.MaidFiddler.WPF.Model
{
    public class CM3D2 : INotifyPropertyChanged
    {
        private static CM3D2 _instance;

        private List<TranslationData> _condition;

        private List<TranslationData> _conditionSpecal;

        private List<TranslationData> _contractType;

        private List<TranslationData> _maidClassData;

        private List<TranslationData> _personaList;

        private List<TranslationData> _seikeiken;

        private List<TranslationData> _yotogiClassData;

        private CM3D2()
        {
            // TODO: Remove
            PersonaList = new List<TranslationData>
            {
                new TranslationData("Pure"),
                new TranslationData("Pride"),
                new TranslationData("Yandere"),
                new TranslationData("Anesan")
            };

            ContractType = new List<TranslationData>
            {
                new TranslationData("Nurture"),
                new TranslationData("Free"),
                new TranslationData("Exclusive")
            };

            Condition = new List<TranslationData>
            {
                new TranslationData("Tonus"),
                new TranslationData("Contact"),
                new TranslationData("Trust"),
                new TranslationData("Lover"),
                new TranslationData("Slave")
            };

            ConditionSpecial = new List<TranslationData>
            {
                new TranslationData("Null"),
                new TranslationData("Drunk"),
                new TranslationData("Osioki")
            };

            Seikeiken = new List<TranslationData>
            {
                new TranslationData("No_No"),
                new TranslationData("Yes_No"),
                new TranslationData("No_Yes"),
                new TranslationData("Yes_Yes")
            };

            MaidClassData = new List<TranslationData>
            {
                new TranslationData("Maid_Novice")
            };

            YotogiClassData = new List<TranslationData>
            {
                new TranslationData("Yotogi_Debut")
            };
        }

        public List<TranslationData> Condition
        {
            get => _condition;
            set
            {
                _condition = value;
                OnPropertyChanged(nameof(Condition));
            }
        }

        public List<TranslationData> ConditionSpecial
        {
            get => _conditionSpecal;
            set
            {
                _conditionSpecal = value;
                OnPropertyChanged(nameof(ConditionSpecial));
            }
        }

        public List<TranslationData> ContractType
        {
            get => _contractType;
            set
            {
                _contractType = value;
                OnPropertyChanged(nameof(ContractType));
            }
        }

        public static CM3D2 Instance => _instance ?? (_instance = new CM3D2());

        public List<TranslationData> MaidClassData
        {
            get => _maidClassData;
            set
            {
                _maidClassData = value;
                OnPropertyChanged(nameof(MaidClassData));
            }
        }

        public List<TranslationData> PersonaList
        {
            get => _personaList;
            set
            {
                _personaList = value;
                OnPropertyChanged(nameof(PersonaList));
            }
        }

        public List<TranslationData> Seikeiken
        {
            get => _seikeiken;
            set
            {
                _seikeiken = value;
                OnPropertyChanged(nameof(Seikeiken));
            }
        }

        public List<TranslationData> YotogiClassData
        {
            get => _yotogiClassData;
            set
            {
                _yotogiClassData = value;
                OnPropertyChanged(nameof(YotogiClassData));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}