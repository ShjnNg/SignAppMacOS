using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Meadow;
using Meadow.Pinouts;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using SignCAApp.Service;
using SignCAApp.ViewModels;
using SignCAApp.Views;
using Avalonia.Media.Imaging;
using NLog;
using System.IO;


namespace SignCAApp;

public partial class App : AvaloniaMeadowApplication<Linux<RaspberryPi>>
{
    private TrayIcon _trayIcon;
    public string _baseUrl;
    public string _token;
    public long _fileId;
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    public App()
    {
    }
    public App(string baseUrl, string token, string fileId)
    {
        _fileId = Convert.ToInt64(fileId);
        _baseUrl = baseUrl;
        _token = token;
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        LoadMeadowOS();
    }

    public override Task InitializeMeadow()
    {
        var r = Resolver.Services.Get<IMeadowDevice>();

        if (r == null)
        {
            Resolver.Log.Info("IMeadowDevice is null");
        }
        else
        {
            Resolver.Log.Info($"IMeadowDevice is {r.GetType().Name}");
        }

        return Task.CompletedTask;

    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "favicon.ico")),
                ToolTipText = "SignCAApp",
                IsVisible = true
            };
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(_baseUrl, _token, _fileId)
                {
                    DataContext = new MainWindowViewModel(_baseUrl, _token, _fileId)
                };
                desktop.MainWindow.Hide();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new CertificateSelectionMacOSView
                {
                    DataContext = new CertificateSelectionMacOSView(_baseUrl, _token, _fileId)
                };
                singleViewPlatform.MainView.IsVisible = true;
            }
            // Xử lý khi click vào icon system tray
            _trayIcon.Clicked += (_, _) =>
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (desktop.MainWindow.IsVisible)
                    {
                        desktop.MainWindow.Hide();
                    }
                    else
                    {
                        desktop.MainWindow.Show();
                        desktop.MainWindow.Activate();
                    }
                }
                else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                {
                    if (singleViewPlatform.MainView.IsVisible)
                    {
                        singleViewPlatform.MainView.IsVisible = false;
                    }
                    else
                    {
                        singleViewPlatform.MainView.IsVisible = true;
                    }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi: [{ex}]");
            throw;
        }


        base.OnFrameworkInitializationCompleted();
    }
}
