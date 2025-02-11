using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SignCAApp.ViewModels;

namespace SignCAApp.Views;

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
