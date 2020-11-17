using System;
using System.Collections.Generic;
using System.Windows.Input;
using XPlatformChat.Client.Wpf.Commands;
using XPlatformChat.Client.Wpf.Properties;

namespace XPlatformChat.Client.Wpf.ViewModels
{
    // TODO : Implement INotifyDataErrorInfo
    public class SettingsViewModel : BaseViewModel
    {
        private string _apiUrl = Settings.Default.ApiUrl;
        public string ApiUrl
        {
            get => _apiUrl;
            set => Set(ref _apiUrl, value);
        }

        public Dictionary<string, List<string>> PropertyErrors;

        public ICommand ApplySettingsCommand { get; }

        public SettingsViewModel()
        {
            ApplySettingsCommand = new RelayCmd(ApplySettings, OnException);
            PropertyErrors = new Dictionary<string, List<string>>();
        }

        private void ApplySettings()
        {
            if (Uri.TryCreate(ApiUrl, UriKind.Absolute, out _))
            {
                Settings.Default.ApiUrl = ApiUrl;
                Settings.Default.Save();
            }
        }
    }
}
