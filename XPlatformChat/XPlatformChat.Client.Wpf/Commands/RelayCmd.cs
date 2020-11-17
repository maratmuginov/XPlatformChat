using System;
using System.Diagnostics;
using System.Windows.Input;

namespace XPlatformChat.Client.Wpf.Commands
{
    public class RelayCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return _isExecuting == false && _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                _callback();
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
            IsExecuting = false;
        }

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
        private readonly Action _callback;
        private readonly Func<bool> _canExecute;

        public RelayCmd(Action callback, Action<Exception> onException) : 
            this(callback, onException, () => true) {
            
        }

        public RelayCmd(Action callback, 
            Action<Exception> onException, 
            Func<bool> canExecute) {
            _onException = onException;
            _callback = callback;
            _canExecute = canExecute;
        }
    }

    public class RelayCmd<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecute is not null && parameter is T t)
                return _isExecuting == false && _canExecute.Invoke(t);
            
            return _isExecuting == false;
        }

        public void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                _callback.Invoke((T)parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
            IsExecuting = false;
        }

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
        private readonly Action<T> _callback;
        private readonly Func<T, bool> _canExecute;

        public RelayCmd(Action<T> callback, Func<T, bool> canExecute, Action<Exception> onException)
        {
            _onException = onException;
            _callback = callback;
            _canExecute = canExecute;
        }

        public RelayCmd(Action<T> callback) : this(callback, null, ex => Debug.Print(ex.Message)) {
            
        }
    }
}
