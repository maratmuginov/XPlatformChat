using System;
using System.Diagnostics;
using XPlatformChat.Lib.Helpers;

namespace XPlatformChat.Client.Wpf.ViewModels
{
    public class BaseViewModel : Observable
    {
        public static void OnException(Exception ex) => Debug.Print(ex.Message);
    }
}
