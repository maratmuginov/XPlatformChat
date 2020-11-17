using System;
using System.Threading.Tasks;

namespace XPlatformChat.Client.Wpf.Commands
{
    public class AsyncRelayCmd : AsyncCmdBase
    {
        private readonly Func<Task> _callback;
        public AsyncRelayCmd(Func<Task> callback, Action<Exception> onException) : base(onException)
        {
            _callback = callback;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            await _callback();
        }
    }

    public class AsyncRelayCmd<T> : AsyncCmdBase
    {
        private readonly Func<T, Task> _callback;
        public AsyncRelayCmd(Func<T, Task> callback, Action<Exception> onException) :
            base(onException) =>
            _callback = callback;

        protected override async Task ExecuteAsync(object parameter)
        {
            if (parameter is T t)
                await _callback(t);
            else
                throw new ArgumentException();
        }
    }
}
