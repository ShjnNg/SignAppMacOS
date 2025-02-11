using iText.Commons.Bouncycastle.Asn1.X509;
using System.Collections.ObjectModel;

namespace SignCAAppMacOS.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public CertificateSelectionMacOSViewModel CertificateViewModel { get; set; }

    public MainWindowViewModel(string baseUrl, string token, long fileId)
    {
        CertificateViewModel = new CertificateSelectionMacOSViewModel(baseUrl, token, fileId);
    }
}
