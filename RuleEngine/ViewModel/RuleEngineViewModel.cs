using System.Windows.Input;
using RuleEngine.Command;
using RuleEngine.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace RuleEngine.ViewModel
{
    internal class RuleEngineViewModel : INotifyPropertyChanged
    {
        #region [Properties and Fields]
        private string _streamingData;
        private bool canExecute = true;
        public string Signal { get; set; }
        public string Value { get; set; }
        public string SelectedCondition { get; set; }
        public ICommand ExceuteRuleCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> _valueTypeCollection;
        public List<string> ValueTypeCollection
        {
            get { return _valueTypeCollection; }
            set
            {
                _valueTypeCollection = value;
                OnPropertyChange("ValueTypeCollection");
            }
        }

        private string _valueTypeSelected;
        public string ValueTypeSelected
        {
            get { return _valueTypeSelected; }
            set
            {
                _valueTypeSelected = value;
                SetConditionItemSource();
                OnPropertyChange("ValueType");
            }
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChange("Result");
            }
        }

        private List<string> _conditionCollection;
        public List<string> ConditionCollection
        {
            get { return _conditionCollection; }
            set
            {
                _conditionCollection = value;
                OnPropertyChange("ConditionCollection");
            }
        }

        private bool _conditionIsEnabled;
        public bool ConditionIsEnabled
        {
            get { return _conditionIsEnabled; }
            set
            {
                _conditionIsEnabled = value;
                OnPropertyChange("ConditionIsEnabled");
            }
        }

        private Visibility _messageLabelVisibility;
        public Visibility MessageLabelVisibility
        {
            get { return _messageLabelVisibility; }
            set
            {
                _messageLabelVisibility = value;
                OnPropertyChange("MessageLabelVisibility");
            }
        }
        #endregion

        #region [Constructor]
        public RuleEngineViewModel()
        {
            ExceuteRuleCommand = new RelayCommand(OnExecuteRule, param => canExecute);
            LoadCommand = new RelayCommand(OnLoad, param => canExecute);
            ConditionIsEnabled = false;
            ValueTypeCollection = new List<string>() { "Integer", "String", "Datetime" };
            MessageLabelVisibility = Visibility.Collapsed;
        }
        #endregion

        #region [Methods]
        private void OnLoad(object obj)
        {
            MessageLabelVisibility = Visibility.Collapsed;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                _streamingData = File.ReadAllText(openFileDialog.FileName);

            MessageLabelVisibility = Visibility.Visible;
        }

        private void OnExecuteRule(object obj)
        {
            RuleEngineModel model = new RuleEngineModel
            {
                Signal = Signal,
                Value = Value,
                ValueType = ValueTypeSelected,
                SelectedCondition = SelectedCondition
            };
            var result = model.GetDataThatViolatesRule(_streamingData);
            Result = string.IsNullOrEmpty(result) ? "No Data" : result;
        }

        private void SetConditionItemSource()
        {
            ConditionIsEnabled = true;
            ConditionCollection = ValueTypeSelected.ToLower() == "string" ? new List<string>() { "=", "!=" } : new List<string>() { "<=", ">=", "=", "!=", "<", ">" };
        }

        private void OnPropertyChange(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
