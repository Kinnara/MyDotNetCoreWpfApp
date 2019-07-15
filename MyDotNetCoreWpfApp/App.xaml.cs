﻿using Microsoft.Extensions.DependencyInjection;
using MyDotNetCoreWpfApp.Activation;
using MyDotNetCoreWpfApp.Services;
using MyDotNetCoreWpfApp.ViewModels;
using MyDotNetCoreWpfApp.Views;
using System.Windows;
using System.Windows.Threading;

namespace MyDotNetCoreWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        internal static App CurrentApp = (App)Current;

        private ActivationService _activationService;

        public App()
        {
            _serviceProvider = ConfigureServices()
                                .BuildServiceProvider();
            _activationService = _serviceProvider.GetService<ActivationService>();
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ActivationService>();
            services.AddSingleton<NavigationService>();
            services.AddSingleton<ThemeSelectorService>();

            // Handlers
            services.AddTransient<DefaultActivationHandler>();

            // Register Views
            services.AddTransient<ShelWindow>();
            services.AddTransient<MainPage>();
            services.AddTransient<SecondaryPage>();

            // Register ViewModels
            services.AddTransient<ShelWindowViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<SecondaryViewModel>();
            return services;
        }

        private async void OnStartup(object sender, StartupEventArgs e)
            => await _activationService.ActivateAsync(e);

        private void OnExit(object sender, ExitEventArgs e)
            => _activationService.Exit();

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }
    }
}