using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SignCAAppMacOS.ViewModels;

namespace SignCAAppMacOS.Views;

public partial class CertificateSelectionMacOSView : UserControl
{
    public CertificateSelectionMacOSView()
    {
        InitializeComponent();
        DataContext = new CertificateSelectionMacOSViewModel();
    }
    public CertificateSelectionMacOSView(string baseUrl, string token, long fileId)
    {
        InitializeComponent();
        DataContext = new CertificateSelectionMacOSViewModel(baseUrl, token, fileId);
    }
}
