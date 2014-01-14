using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Orc.SolutionTool;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Mvvm;

namespace Options
{
    public class OptionsViewModel : ViewmodelBase
    {
        private ObservableCollection<Setting> _settings;
        public ObservableCollection<Setting> Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    RaisePropertyChanged(() => Settings);
                }
            }
        }

        private Setting _selectedSetting;
        public Setting SelectedSetting
        {
            get
            {
                return _selectedSetting;
            }
            set
            {
                if (_selectedSetting != value)
                {
                    _selectedSetting = value;
                    RaisePropertyChanged(() => SelectedSetting);
                }
            }
        }

        #region Save Command

        public DelegateCommand<object> _saveCommand;
        public DelegateCommand<object> SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new DelegateCommand<object>(Save, CanSave));
            }
            set
            {
                _saveCommand = value;
            }
        }

        private bool CanSave(object arg)
        {
            return true;
        }

        private void Save(object arg)
        {
            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var ro = cfg.AppSettings.IsReadOnly();

            if (!ro)
            {
                foreach (var i in Settings)
                {
                    cfg.AppSettings.Settings.Remove(i.Key);
                    cfg.AppSettings.Settings.Add(i.Key, i.Value);
                }

                cfg.Save();
            }
        }

        #endregion

        public OptionsViewModel(IShellService shellService)
            : base(shellService)
        {
            Settings = new ObservableCollection<Setting>();
            //Settings.Add(new Setting(
            //    "DefaultFontSize",
            //    "Default font size on GUI. ",
            //    "18"
            //    ));
            Settings.Add(new Setting(
                "InspectCodeExePath",
                "InspectCode Executable Path", 
                "x:\\{some_dir}\\inspectcode.exe"
                ));

            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var i in ConfigurationManager.AppSettings.AllKeys)
            {
                var setting = Settings.FirstOrDefault(x => x.Key == i);

                if (setting != null && !string.IsNullOrWhiteSpace(cfg.AppSettings.Settings[i].Value))
                {
                    setting.Value = cfg.AppSettings.Settings[i].Value;
                }
            }

        }
    }
}
