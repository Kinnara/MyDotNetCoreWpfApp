﻿using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MyDotNetCoreWpfApp.Services
{
    public interface INavigationService
    {
        event EventHandler<string> Navigated;

        bool CanGoBack { get; }

        void Initialize(Frame shellFrame);

        void Configure(string key, Type pageType);

        bool IsNavigated();
        
        bool Navigate(string pageKey, object extraData = null);

        void GoBack();

    }
}
