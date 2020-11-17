using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using XPlatformChat.Client.Wpf.Commands;
using XPlatformChat.Lib.Extensions;
using XPlatformChat.Lib.Models;
using XPlatformChat.Lib.Services;
using XPlatformChat.Lib.Stores;

namespace XPlatformChat.Client.Wpf.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        #region Backing Fields

        private string _errorMessage;
        private bool _hasErrors;
        private string _newMessageText = string.Empty;

        #endregion

        #region Services

        private readonly ChatService _chatService;
        private readonly HttpClient _client;
        private readonly TokenStore _tokenStore;

        #endregion

        public string CurrentUserName;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                Set(ref _errorMessage, value);
                HasErrors = !string.IsNullOrEmpty(value);
            }
        }
        public bool HasErrors
        {
            get => _hasErrors;
            set => Set(ref _hasErrors, value);
        }
        public string NewMessageText
        {
            get => _newMessageText;
            set => Set(ref _newMessageText, value);
        }
        public ObservableCollection<Message> Messages { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand OnLoadedCommand { get; }

        #region Constructors

        public ChatViewModel(ChatService chatService, HttpClient client, TokenStore tokenStore)
        {
            _chatService = chatService;
            _client = client;
            _tokenStore = tokenStore;
            SendMessageCommand = new AsyncRelayCmd(SendMessage, OnException);
            OnLoadedCommand = new AsyncRelayCmd(OnLoaded, OnException);
            Messages = new ObservableCollection<Message>();
            chatService.MessageReceived += OnMessageReceived;
        }
        public static ChatViewModel GetConnectedChatViewModel(ChatService chatService, HttpClient client, TokenStore tokenStore)
        {
            var viewModel = new ChatViewModel(chatService, client, tokenStore);

            chatService.Connect().ContinueWith(task => 
                viewModel.ErrorMessage = task.Exception?.Message);
            
            viewModel.CurrentUserName = tokenStore.GetTokenGrant().Username;

            return viewModel;
        }

        #endregion

        private async Task SendMessage()
        {
            var message = new Message
            {
                Text = NewMessageText, 
                Sent = DateTime.Now, 
                Username = CurrentUserName
            };

            NewMessageText = string.Empty;

            await _chatService.SendMessageAsync(message);
        }

        #region Events

        private void OnMessageReceived(Message message)
        {
            Messages.Add(message);
        }

        private async Task OnLoaded()
        {
            if (_tokenStore.GetTokenGrant()?.Token is { } token)
            {
                _client.SetBearerToken(token);
                var response = await _client.GetAsync($"{Properties.Settings.Default.ApiUrl}/messages");
                var messages = await response.Content.ReadFromJsonAsync<List<Message>>();
                messages?.ForEach(Messages.Add);
            }
        }

        #endregion
    }
}
