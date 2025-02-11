using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ColorTextBlock.Avalonia;
using SignCAApp.ViewModels;

namespace SignCAApp.Views;

public partial class CertificateSelectionView : UserControl
{
    public CertificateSelectionView()
    {
        InitializeComponent();
        DataContext = new CertificateSelectionViewModel();
    }
    public CertificateSelectionView(string baseUrl, string token, long fileId)
    {
        InitializeComponent();
        DataContext = new CertificateSelectionViewModel(baseUrl, token, fileId);
    }
}
