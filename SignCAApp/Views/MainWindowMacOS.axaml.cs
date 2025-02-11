using Avalonia.Controls;
using SignCAApp.ViewModels;

namespace SignCAApp.Views;

public partial class MainWindowMacOS : Window
{
    //public string Token;
    //public string BaseUrl;
    //public long FileId;
    public MainWindowMacOS()
    {
        InitializeComponent();
    }
    public MainWindowMacOS(string baseUrl, string token, long fileId)
    {
        InitializeComponent();
        //DataContext = new MainWindowViewModel();  // Set ViewModel
        // Set MainWindowMacOS DataContext
        var mainViewModel = new MainWindowViewModel(baseUrl, token, fileId);
        DataContext = mainViewModel;
        // Set CertificateSelectionView DataContext in Code-Behind
        CertificateView.DataContext = mainViewModel.CertificateViewModel;
    }
}
