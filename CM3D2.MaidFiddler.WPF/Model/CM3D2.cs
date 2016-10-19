using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CM3D2.MaidFiddler.WPF.Translations;

namespace CM3D2.MaidFiddler.WPF.Model
{
    public class CM3D2 : INotifyPropertyChanged
    {
        private static CM3D2 _instance;
        public static CM3D2 Instance => _instance ?? (_instance = new CM3D2());

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

        private List<TranslationData> _personaList;
        public List<TranslationData> PersonaList
        {
            get { return _personaList;}
            set
            {
                _personaList = value;
                OnPropertyChanged(nameof(PersonaList));
            }
        }

        private List<TranslationData> _contractType;
        public List<TranslationData> ContractType
        {
            get { return _contractType; }
            set
            {
                _contractType = value;
                OnPropertyChanged(nameof(ContractType));
            }
        }

        private List<TranslationData> _condition;
        public List<TranslationData> Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
                OnPropertyChanged(nameof(Condition));
            }
        }

        private List<TranslationData> _conditionSpecal;
        public List<TranslationData> ConditionSpecial
        {
            get { return _conditionSpecal; }
            set
            {
                _conditionSpecal = value;
                OnPropertyChanged(nameof(ConditionSpecial));
            }
        }

        private List<TranslationData> _seikeiken;
        public List<TranslationData> Seikeiken
        {
            get { return _seikeiken; }
            set
            {
                _seikeiken = value;
                OnPropertyChanged(nameof(Seikeiken));
            }
        }

        private List<TranslationData> _maidClassData;
        public List<TranslationData> MaidClassData
        {
            get { return _maidClassData; }
            set
            {
                _maidClassData = value;
                OnPropertyChanged(nameof(MaidClassData));
            }
        }

        private List<TranslationData> _yotogiClassData;
        public List<TranslationData> YotogiClassData
        {
            get { return _yotogiClassData; }
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
