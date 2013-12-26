using System.Collections.ObjectModel;
using System.Text;

namespace Orc.SolutionTool.Core
{
    public class Target : NotificationObject
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private Target _parent;
        public Target Parent
        {
            get
            {
                return _parent;
            }
            private set
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

        public ObservableCollection<IRule> Rules { get; private set; }

        public ObservableCollection<Target> Children { get; private set; }

        public Target(string name, Target parent)
        {
            Name = name;
            Parent = parent;

            Rules = new ObservableCollection<IRule>();
            Children = new ObservableCollection<Target>();
        }
    }
}
