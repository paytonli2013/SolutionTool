using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;
using Orc.SolutionTool.Model.Rules;
using Orc.SolutionTool.Mvvm;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("target")]
    [XmlInclude(typeof(CheckCsprojOutputPathRule))]
    [XmlInclude(typeof(CheckWithInspectCodeRule))]
    [XmlInclude(typeof(CheckWithStyleCopRule))]
    [XmlInclude(typeof(FileMustExistsRule))]
    [XmlInclude(typeof(FolderMustExistsRule))]
    public class Target : NotificationObject
    {
        private string _name;
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private Target _parent;
        [XmlIgnore]
        public Target Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    RaisePropertyChanged(() => Parent);
                }
            }
        }

        public string Path
        {
            get
            {
                var p = Parent;

                if (p == null)
                {
                    return ".";
                }

                var sb = new StringBuilder(Name);

                while (p.Parent != null)
                {
                    sb.Insert(0, "\\");
                    sb.Insert(0, p.Name);

                    p = p.Parent;
                }

                sb.Insert(0, ".\\");

                var s = sb.ToString();

                return s;
            }
        }

        [XmlArray("rules")]
        [XmlArrayItem("rule")]
        public ObservableCollection<Rule> Rules { get; private set; }

        [XmlElement("target")]
        public ObservableCollection<Target> Children { get; private set; }

        public Target()
        {

        }

        public Target(string name, Target parent)
        {
            Name = name;
            Parent = parent;

            Rules = new ObservableCollection<Rule>();
            Children = new ObservableCollection<Target>();
        }
    }
}
