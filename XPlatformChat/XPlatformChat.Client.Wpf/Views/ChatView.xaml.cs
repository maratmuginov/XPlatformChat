using System.Windows;
using System.Windows.Input;

namespace XPlatformChat.Client.Wpf.Views
{
    public partial class ChatView
    {
        public ChatView() => InitializeComponent();

        public static readonly DependencyProperty OnLoadedCommandProperty = DependencyProperty.Register(
            nameof(OnLoadedCommand), 
            typeof(ICommand), 
            typeof(ChatView), 
            new PropertyMetadata(null));

        public ICommand OnLoadedCommand
        {
            get => (ICommand) GetValue(OnLoadedCommandProperty);
            set => SetValue(OnLoadedCommandProperty, value);
        }


        private void ChatView_OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLoadedCommand?.Execute(null);
        }
    }
}
