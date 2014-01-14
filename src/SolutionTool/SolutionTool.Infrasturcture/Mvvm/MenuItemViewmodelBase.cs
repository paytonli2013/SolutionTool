using System;
using Microsoft.Practices.Prism.Regions;

namespace Orc.SolutionTool.Mvvm
{
    public abstract class MenuItemViewmodelBase : ViewmodelBase, IMenuViewmodel
    {
        protected IRegionManager _regionManager;

        public MenuItemViewmodelBase(IRegionManager regionManager, IShellService shellService):base(shellService)
        {
            _regionManager = regionManager;
            RegionName = "ContentRegion";
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

        bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                RaisePropertyChanged("IsActive");
                OnMenuActived(_isActive);
                FireIsActiveChanged();
            }
        }

        string _viewName;

        public string ViewName
        {
            get { return _viewName; }
            protected set
            {
                _viewName = value;
                RaisePropertyChanged("ViewName");
            }
        }

        string _regionName;

        public string RegionName
        {
            get { return _regionName; }
            protected set { _regionName = value; }
        }

        protected virtual void OnMenuActived(bool isActive)
        {
            if (isActive)
            {
                Navigate(RegionName,"\\"+ ViewName, (isSuccess, error) =>
                {
                    if (!isSuccess && error != null)
                    {
                        ShowMessage(error);
                    }
                });
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
