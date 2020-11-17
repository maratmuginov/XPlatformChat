using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using XPlatformChat.Client.Wpf.Helpers;
using XPlatformChat.Client.Wpf.Properties;
using XPlatformChat.Client.Wpf.ViewModels;
using XPlatformChat.Client.Wpf.Views;
using XPlatformChat.Lib.Services;
using XPlatformChat.Lib.Stores;

namespace XPlatformChat.Client.Wpf
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = ConfigureServices();
            var window = new ShellView 
            {
                DataContext = services.GetRequiredService<ShellViewModel>()
            };
            window.Show();
        }

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ShellViewModel>()
                .AddSingleton<LoginViewModel>()
                .AddSingleton<SettingsViewModel>()
                .AddSingleton<Navigator>()
                .AddSingleton<ChatService>()
                .AddSingleton<TokenStore>()
                .AddScoped(GetHubConnection)
                .AddScoped(GetConnectedChatViewModel)
                .AddScoped<HttpClient>()
                .BuildServiceProvider();
        }

        private static HubConnection GetHubConnection(IServiceProvider services)
        {
            var tokenStore = services.GetRequiredService<TokenStore>();
            return new HubConnectionBuilder().WithUrl($"{Settings.Default.ApiUrl}/chat", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(tokenStore.GetTokenGrant().Token);
            }).Build();
        }

        private static ChatViewModel GetConnectedChatViewModel(IServiceProvider services)
        {
            var chatService = services.GetRequiredService<ChatService>();
            var client = services.GetRequiredService<HttpClient>();
            var tokenStore = services.GetRequiredService<TokenStore>();
            return ChatViewModel.GetConnectedChatViewModel(chatService, client, tokenStore);
        }
    }
}
