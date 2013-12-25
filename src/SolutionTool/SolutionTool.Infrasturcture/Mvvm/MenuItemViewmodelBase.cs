using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.SolutionTool.Mvvm
{
    public abstract class MenuItemViewmodelBase : ViewmodelBase, IMenuViewmodel
    {
        protected IRegionManager _regionManager;

        public MenuItemViewmodelBase(IRegionManager regionManager, IShellService shellService):base(shellService)
        {
            _regionManager = regionManager;
        }

        public void Navigate(string region, string view, Action<bool, Exception> onComplete)
        {
            try
            {
                _regionManager.RequestNavigate(region, view,
                    (result) =>
                    {
                        if (result.Result.HasValue && result.Result.Value)
                        {
                            if (onComplete != null)
                            {
                                onComplete.Invoke(true, null);
                            }
                        }
                        else if (result.Error != null)
                        {
                            if (onComplete != null)
                            {
                                onComplete.Invoke(false, result.Error);
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                if (onComplete != null)
                {
                    onComplete.Invoke(false, ex);
                }
            }
        }

        string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        bool _isEnable;
        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                _isEnable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        bool _isAvtive;
        public bool IsActive
        {
            get
            {
                return _isAvtive;
            }
            set
            {
                _isAvtive = value;
                RaisePropertyChanged("IsActive");
                FireIsActiveChanged();
            }
        }

        public event EventHandler IsActiveChanged;

        protected void FireIsActiveChanged()
        {
            var handler = IsActiveChanged;
            if (handler != null)
            {
                handler.Invoke(this, new EventArgs());
            }
        }
    }
}
