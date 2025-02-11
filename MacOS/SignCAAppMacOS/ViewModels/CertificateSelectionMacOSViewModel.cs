using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using ReactiveUI;
using System.Reactive;
using SignCAAppMacOS.Service;
using NLog;
using System.Threading.Tasks;

namespace SignCAAppMacOS.ViewModels;
public class CertificateSelectionMacOSViewModel : ViewModelBase
{
    public ObservableCollection<X509Certificate2> Certificates { get; } = new();

    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private X509Certificate2? _selectedCertificate;
    public string Token;
    public string BaseUrl;
    public long FileId;
    public X509Certificate2? SelectedCertificate
    {
        get => _selectedCertificate;
        set => this.RaiseAndSetIfChanged(ref _selectedCertificate, value);
    }

    private KySoService kySoService;
    public string Messenger { get; set; } = "Click Sign to sign";
    public ReactiveCommand<Unit, Unit> SignCommand { get; }
    public string Message { get; set; } = "Click Sign to sign";
    public CertificateSelectionMacOSViewModel()
    {
        LoadCertificates();
        SignCommand = ReactiveCommand.Create(SignCert);
    }
    public CertificateSelectionMacOSViewModel(string baseUrl, string token, long fileId)
    {
        LoadCertificates();
        SignCommand = ReactiveCommand.Create(SignCert);
        BaseUrl = baseUrl;
        Token = token;
        FileId = fileId;
        kySoService = new KySoService(BaseUrl, Token);
    }

    private void SignCert()
    {
        Message = "Signing in progress";
        Message = kySoService.SendCertToFile(FileId, SelectedCertificate).GetAwaiter().GetResult();
        Task.Delay(5000).ContinueWith(t => Environment.Exit(0));
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
