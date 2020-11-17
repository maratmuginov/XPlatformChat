using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using XPlatformChat.Lib.Models;

namespace XPlatformChat.Lib.Services
{
    public class ChatService
    {
        private readonly HubConnection _hubConnection;
        public event Action<Message> MessageReceived;

        public ChatService(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            _hubConnection.On<Message>("ReceiveMessage", message => MessageReceived?.Invoke(message));
        }

        public async Task Connect() =>
            await _hubConnection.StartAsync();

        public async Task SendMessageAsync(Message message) =>
            await _hubConnection.InvokeAsync("SendMessage", message);
    }
}
