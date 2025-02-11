using iText.Commons.Bouncycastle.Asn1.X509;
using System.Collections.ObjectModel;

namespace SignCAApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public CertificateSelectionViewModel CertificateViewModel { get; set; }

    public MainWindowViewModel(string baseUrl, string token, long fileId)
    {
        CertificateViewModel = new CertificateSelectionViewModel(baseUrl, token, fileId);
    }
}
