using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using ReactiveUI;
using System.Reactive;
using SignCAApp.Service;
using NLog;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Runtime.CompilerServices;
using Avalonia.Threading;

namespace SignCAApp.ViewModels;
public class CertificateSelectionViewModel : ViewModelBase, INotifyPropertyChanged
{
    public ObservableCollection<X509Certificate2> Certificates { get; } = new();

    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private X509Certificate2? _selectedCertificate;
    public string Token;
    public string BaseUrl;
    public long FileId;
    private Timer _timer;
    private KySoService kySoService;
    public X509Certificate2? SelectedCertificate
    {
        get => _selectedCertificate;
        set => this.RaiseAndSetIfChanged(ref _selectedCertificate, value);
    }

    public string Message { get; set; } = "Click Sign to sign";
    public string _displayText { get; set; }
    public string DisplayText
    {
        get => _displayText;
        set
        {
            if (_displayText != value)
            {
                _displayText = value;
                OnPropertyChanged(nameof(DisplayText)); // Notify that the property has changed
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public ReactiveCommand<Unit, Unit> SignCommand { get; }
    public CertificateSelectionViewModel()
    {
        LoadCertificates();
        SignCommand = ReactiveCommand.Create(SignCert);
    }
    public CertificateSelectionViewModel(string baseUrl, string token, long fileId)
    {
        LoadCertificates();
        SignCommand = ReactiveCommand.Create(SignCert);
        BaseUrl = baseUrl;
        Token = token;
        FileId = fileId;
        kySoService = new KySoService(BaseUrl, Token);
        _timer = new Timer(UpdateTextAutomatically, null, 0, 100);
    }

    private void UpdateTextAutomatically(object state)
    {
        DisplayText = Message;
    }
    private void SignCert()
    {
        Message = "Signing in progress";
        if (PlatformHelper.IsWindows())
        {
            //_logger.Info("Running on Windows");
            SelectedCertificate = GetCertificateWindows();
        }
        else if (PlatformHelper.IsMacOS())
        {
            //_logger.Info("Running on macOS");
        }
        mess = kySoService.SendCertToFile(FileId, SelectedCertificate).GetAwaiter().GetResult();
        if(mess == "Success")
        {
            mess += ". Auto close in";
            RemainingSeconds = 3;
            _timer1 = new Timer(UpdateCountdown, null, 0, 1000);
            Task.Delay(3000).ContinueWith(t => Environment.Exit(0));
        }
        else Message = mess;
    }
    private int _remainingSeconds;  // Số giây còn lại
    private Timer _timer1;
    private string mess;
    public int RemainingSeconds
    {
        get => _remainingSeconds;
        set
        {
            if (_remainingSeconds != value)
            {
                _remainingSeconds = value;
                OnPropertyChanged();
            }
        }
    }
    private void UpdateCountdown(object state)
    {
        Message = mess + " " + RemainingSeconds + "s";
        if (RemainingSeconds > 0)
        {
            Dispatcher.UIThread.Post(() => RemainingSeconds--);  // Cập nhật trên UI Thread
        }
        else
        {
            _timer1.Dispose(); // Dừng Timer khi hết giờ
        }
    }
    public X509Certificate2 GetCertificateWindows()
    {
        //Chung thu so nguoi ky
        X509Certificate2 cert = null;

        // Select a certificate to signature
        X509Certificate2Collection keyStore = new X509Certificate2Collection();
        X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine); // sửa theo hướng dẫn của Huy
        store.Open(OpenFlags.ReadOnly);
        keyStore.AddRange(store.Certificates);
        store.Close();

        try
        {
            var lst = X509Certificate2UI.SelectFromCollection(keyStore, "Chứng thư số ký", "Chọn chứng thư số ký", X509SelectionFlag.SingleSelection);
            if (lst.Count > 0)
            {
                cert = lst[0];
                return cert;
            }
            else
                return null;
        }
        catch (Exception ex)
        {
            _logger.Error("ERROR GetCertificate:" + ex);
            throw ex;
        }
    }
    private void LoadCertificates()
    {
        try
        {
            Certificates.Clear();
            using X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            foreach (var cert in store.Certificates.OfType<X509Certificate2>())
            {
                Certificates.Add(cert);
            }

            store.Close();
            if (!Certificates.Any())
            {
                Certificates.Clear();
                Certificates.Add(new X509Certificate2());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading certificates: {ex.Message}");
        }
    }
}
