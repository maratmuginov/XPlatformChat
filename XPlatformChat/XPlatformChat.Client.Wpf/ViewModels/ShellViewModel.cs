using XPlatformChat.Client.Wpf.Helpers;

namespace XPlatformChat.Client.Wpf.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public Navigator Navigator { get; }

        public ShellViewModel(Navigator navigator, 
            LoginViewModel loginViewModel, 
            SettingsViewModel settingsViewModel) {
            Navigator = navigator;
            Navigator.TryAddViewModel(ViewType.Login, loginViewModel);
            Navigator.TryAddViewModel(ViewType.Settings, settingsViewModel);
            Navigator.NavigateTo(ViewType.Login);
        }

        public string WindowTitle { get; set; } = "XPlatformChat";
    }
}
