using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using XPlatformChat.Client.Wpf.Commands;
using XPlatformChat.Client.Wpf.Helpers;
using XPlatformChat.Lib.Models;
using XPlatformChat.Lib.Stores;

namespace XPlatformChat.Client.Wpf.ViewModels
{
    // TODO: Implement INotifyDataErrorInfo (?)
    public class LoginViewModel : BaseViewModel
    {

        #region Services

        private readonly Navigator _navigator;
        private readonly HttpClient _client;
        private readonly TokenStore _tokenStore;
        private readonly IServiceProvider _services;

        #endregion
        
        private readonly Uri _loginUri;
        public LoginViewModel(IServiceProvider services)
        {
            _services = services;
            _navigator = _services.GetRequiredService<Navigator>();
            _tokenStore = _services.GetRequiredService<TokenStore>();
            _client = _services.GetRequiredService<HttpClient>();

            _loginUri = new Uri($"{Properties.Settings.Default.ApiUrl}/api/authentication/login");
            LoginCommand = new AsyncRelayCmd(Login, OnLoginFailed);
            OnLoadedCommand = new AsyncRelayCmd(OnLoadedAsync, OnException);
            LoginModel = new LoginModel();
        }

        private static void OnLoginFailed(Exception ex)
        {
            // TODO : Handle login failures here.
            Debug.Print(ex.Message);
        }

        private async Task Login()
        {
            string jsonBody = JsonSerializer.Serialize(LoginModel);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_loginUri, body);
            if (response.StatusCode is HttpStatusCode.OK)
            {
                //Add the authorization token to the tokenStore
                var tokenGrant = await response.Content.ReadFromJsonAsync<TokenGrant>();
                _tokenStore.TryAddTokenGrant(tokenGrant);

                //Build a chat viewModel if the login is successful.
                var chatViewModel = _services.GetRequiredService<ChatViewModel>();
                _navigator.TryAddViewModel(ViewType.Chat, chatViewModel);
                _navigator.NavigateTo(ViewType.Chat);
            }
        }

        private async Task OnLoadedAsync() => await Task.Run(OnLoaded);

        private void OnLoaded()
        {
            if (_tokenStore.GetTokenGrant()?.Token is not null) 
                _navigator.NavigateTo(ViewType.Chat);
        }

        public LoginModel LoginModel { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand OnLoadedCommand { get; }
    }
}
