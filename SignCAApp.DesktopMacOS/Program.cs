using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.ReactiveUI;
using NLog;

namespace SignCAApp.Desktop;

class Program
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    public static string _baseURL;
    public static string _token;
    public static string _fileId;
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        try
        {
            var builder = BuildAvaloniaApp();

            _logger.Info("Start program argrs.length=" + args.Length);
            if (args.Length > 0)
            {
                _logger.Info("Tham số truyền vào chương trình:");
                for (int i = 0; i < args.Length; i++)
                    _logger.Info($"\t{i}\t{args[i]}");
            }

            if (args.Contains("--drm"))
            {
                SilenceConsole();
                // If Card0, Card1 and Card2 all don't work. You can also try:                 
                // return builder.StartLinuxFbDev(args);
                // return builder.StartLinuxDrm(args, "/dev/dri/card1");
                return builder.StartLinuxDrm(args, "/dev/dri/card1", 1D);
            }
            var arrayArgs = ReadDataInput(args[0]).ToArray();

            if (arrayArgs != null && arrayArgs.Any())
            {
                if (arrayArgs.Count() >= 0) _baseURL = arrayArgs[0];
                if (arrayArgs.Count() >= 1) _token = arrayArgs[1];
                if (arrayArgs.Count() >= 2) _fileId = arrayArgs[2];
            }
            return builder.StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            return 0;
            // Bạn có thể ghi lại lỗi vào một tệp hoặc hệ thống giám sát
        }
       
    }


    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure(() => new SignCAAppMacOS.AppMacOS(_baseURL, _token, _fileId))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

    private static void SilenceConsole()
    {
        new Thread(() =>
            {
                Console.CursorVisible = false;
                while (true)
                    Console.ReadKey(true);
            })
        { IsBackground = true }.Start();
    }
    /// <summary>
    /// Get list of data from string
    /// </summary>
    /// <param name="arg">list of data as string</param>
    /// <returns>List of data</returns>
    public static List<string> ReadDataInput(string arg)
    {
        List<string> result = new List<string>(); ;

        var strSubTitle = arg.Substring(17);

        var arrStr = strSubTitle.Split(';');

        for (int i = 0; i < arrStr.Length; i++)
        {
            var index = arrStr[i].IndexOf('=');

            if (index >= 0)
            {
                result.Add(arrStr[i].Substring(index + 1));
            }
            else
            {
                result.Add(null);
            }
        }

        return result;
    }
}
