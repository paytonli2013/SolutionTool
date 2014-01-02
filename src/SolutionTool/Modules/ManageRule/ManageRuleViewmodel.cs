using Orc.SolutionTool;
using Orc.SolutionTool.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ManageRule
{
    public class ManageRuleViewmodel : ViewmodelBase
    {
        IRuleManager _ruleManager;

        private ObservableCollection<string> _ruleSetNames;
        public ObservableCollection<string> RuleSetNames
        {
            get
            {
                return _ruleSetNames;
            }
            set
            {
                if (_ruleSetNames != value)
                {
                    _ruleSetNames = value;
                    RaisePropertyChanged(() => RuleSetNames);
                }
            }
        }

        private string _selectedRuleSetName;
        public string SelectedRuleSetName
        {
            get
            {
                return _selectedRuleSetName;
            }
            set
            {
                if (_selectedRuleSetName != value)
                {
                    _selectedRuleSetName = value;

                    var rules = _ruleManager.RuleSets[_selectedRuleSetName];

                    SelectedRuleSet = new ObservableCollection<IRule>();
                    foreach (var i in rules)
                    {
                        SelectedRuleSet.Add(i);
                    }

                    RaisePropertyChanged(() => SelectedRuleSetName);
                }
            }
        }

        private ObservableCollection<IRule> _selectedRuleSet;
        public ObservableCollection<IRule> SelectedRuleSet
        {
            get
            {
                return _selectedRuleSet;
            }
            set
            {
                if (_selectedRuleSet != value)
                {
                    _selectedRuleSet = value;
                    RaisePropertyChanged(() => SelectedRuleSet);
                }
            }
        }

        public ManageRuleViewmodel(IShellService shellService, IRuleManager ruleManager):base(shellService)
        {
            _ruleManager = ruleManager;

            RuleSetNames = new ObservableCollection<string>();
            SelectedRuleSet = new ObservableCollection<IRule>();

            foreach (var i in _ruleManager.RuleSets.Keys)
            {
                RuleSetNames.Add(i);
            }
        }
    }
}
