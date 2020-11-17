using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XPlatformChat.Client.Wpf.Commands
{
    public abstract class AsyncCmdBase : ICommand
    {
        private bool _isExecuting;
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        private readonly Action<Exception> _onException;

        protected AsyncCmdBase(Action<Exception> onException)
        {
            _onException = onException;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => !IsExecuting;

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
