﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MyDotNetCoreWpfApp.MVVMLight.Contracts.Services;
using MyDotNetCoreWpfApp.MVVMLight.Contracts.ViewModels;

namespace MyDotNetCoreWpfApp.MVVMLight.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IPageService _pageService;
        private Frame _frame;
        private object _lastParameterUsed;

        public event EventHandler<string> Navigated;

        public bool CanGoBack
            => _frame != null && _frame.CanGoBack;

        public string CurrentPageKey
        {
            get
            {
                if (_frame.Content is FrameworkElement element)
                {
                    return element.DataContext.GetType().FullName;
                }

                return string.Empty;
            }
        }

        public NavigationService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public void Initialize(Frame shellFrame)
        {
            if (_frame == null)
            {
                _frame = shellFrame;
                _frame.Navigated += OnNavigated;
            }
        }

        public void GoBack()
            => _frame.GoBack();

        public void NavigateTo(string pageKey)
            => NavigateTo(pageKey, null, false);

        public void NavigateTo(string pageKey, object parameter)
            => NavigateTo(pageKey, parameter, false);

        public void NavigateTo(string pageKey, object parameter, bool clearNavigation)
        {
            var pageType = _pageService.GetPageType(pageKey);

            if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
            {
                var dataContext = _frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatingFrom();
                }

                _frame.Tag = clearNavigation;
                var page = _pageService.GetPage(pageKey);
                var navigated = _frame.Navigate(page, parameter);
                if (navigated)
                {
                    _lastParameterUsed = parameter;
                }
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                bool clearNavigation = (bool)frame.Tag;
                if (clearNavigation)
                {
                    frame.CleanNavigation();
                }

                var dataContext = frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.ExtraData);
                }

                Navigated?.Invoke(sender, dataContext.GetType().FullName);
            }
        }
    }
}
