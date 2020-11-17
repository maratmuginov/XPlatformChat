using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using XPlatformChat.Client.Wpf.Commands;
using XPlatformChat.Client.Wpf.ViewModels;
using XPlatformChat.Lib.Helpers;

namespace XPlatformChat.Client.Wpf.Helpers
{
    public class Navigator : Observable
    {
        private readonly Dictionary<ViewType, BaseViewModel> _viewModels;
        private BaseViewModel _activeViewModel;

        public BaseViewModel ActiveViewModel
        {
            get => _activeViewModel;
            set => Set(ref _activeViewModel, value);
        }

        public Navigator()
        {
            _viewModels = new Dictionary<ViewType, BaseViewModel>();
            NavigateToCommand = new RelayCmd<ViewType>(NavigateTo, CanNavigateTo, ex => Debug.Print(ex.Message));
        }

        public void NavigateTo(ViewType viewType)
        {
            ActiveViewModel = _viewModels[viewType];
        }

        private bool CanNavigateTo(ViewType viewType)
        {
            return _viewModels.ContainsKey(viewType);
        }

        public bool TryAddViewModel(ViewType viewType, BaseViewModel viewModel)
        {
            return _viewModels.TryAdd(viewType, viewModel);
        }
        public ICommand NavigateToCommand { get; }
    }

    public enum ViewType
    {
        Chat,
        Login,
        Settings
    }
}
