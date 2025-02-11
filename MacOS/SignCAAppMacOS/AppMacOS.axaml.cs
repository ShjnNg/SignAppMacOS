using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Meadow;
using Meadow.Pinouts;
using SignCAAppMacOS.Views;
using NLog;
using System.IO;
using static System.Net.Mime.MediaTypeNames;


namespace SignCAAppMacOS;

public partial class AppMacOS : AvaloniaMeadowApplication<Linux<RaspberryPi>>
{
    private TrayIcon _trayIcon;
    public string _baseUrl;
    public string _token;
    public long _fileId;
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    public AppMacOS()
    {
    }
    public AppMacOS(string baseUrl, string token, string fileId)
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
    //public override bool OpenUrls(NSApplication application, NSUrl[] urls)
    //{
    //    foreach (var url in urls)
    //    {
    //        if (url.Scheme == "signcaapp-desktop")
    //        {
    //            string query = url.AbsoluteString; // signcaapp-desktop:baseurl=aaa
    //            HandleCustomUrl(query);
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    private void HandleCustomUrl(string url)
    {
        try
        {
            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "favicon.ico")),
                ToolTipText = "SignCAAppMacOS",
                IsVisible = true
            };
            if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
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
                if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
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
    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "favicon.ico")),
                ToolTipText = "SignCAAppMacOS",
                IsVisible = true
            };
           if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
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
                if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
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
