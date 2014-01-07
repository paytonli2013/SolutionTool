using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Model.Rules;
using Orc.SolutionTool.Mvvm;

namespace ManageRule
{
    public class ManageRuleViewmodel : ViewmodelBase
    {
        IRuleManager _ruleManager;

        private ObservableCollection<XRule> _rules;
        public ObservableCollection<XRule> Rules
        {
            get
            {
                return _rules;
            }
            set
            {
                if (_rules != value)
                {
                    _rules = value;
                    RaisePropertyChanged(() => Rules);
                }
            }
        }

        private XRule _selectedRule;
        public XRule SelectedRule
        {
            get
            {
                return _selectedRule;
            }
            set
            {
                if (_selectedRule != value)
                {
                    _selectedRule = value;
                    RaisePropertyChanged(() => SelectedRule);

                    if (_selectedRule != null)
                    {
                        var sb = new StringBuilder();

                        using (var sw = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true, }))
                        {
                            var xs = new XmlSerializer(_selectedRule.GetType());
                            var ns = new XmlSerializerNamespaces();

                            ns.Add("", "");
                            xs.Serialize(sw, _selectedRule, ns);
                        }

                        SelectedRuleXmlContent = sb.ToString();
                    }
                    else
                    {
                        SelectedRuleXmlContent = null;
                    }
                }
            }
        }

        private string _selectedRuleXmlContent;
        public string SelectedRuleXmlContent
        {
            get
            {
                return _selectedRuleXmlContent;
            }
            set
            {
                if (_selectedRuleXmlContent != value)
                {
                    _selectedRuleXmlContent = value;
                    RaisePropertyChanged(() => SelectedRuleXmlContent);
                }
            }
        }

        public ManageRuleViewmodel(IShellService shellService, IRuleManager ruleManager):base(shellService)
        {
            _ruleManager = ruleManager;

            Rules = new ObservableCollection<XRule>();

            foreach (var i in _ruleManager.DefaultRuleSet)
            {
                Rules.Add(i);
            }
        }
    }
}
