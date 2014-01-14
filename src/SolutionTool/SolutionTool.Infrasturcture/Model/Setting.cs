using Orc.SolutionTool.Mvvm;

namespace Orc.SolutionTool.Model
{
    public class Setting : NotificationObject
    {
        private string _key;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    RaisePropertyChanged(() => Key);
                }
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged(() => Description);
                }
            }
        }

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged(() => Value);
                }
            }
        }

        public Setting()
        {

        }

        public Setting(string key, string description = null, string value = null)
        {
            Key = key;
            Description = description;
            Value = value;
        }
    }
}
