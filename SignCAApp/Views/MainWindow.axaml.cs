using Avalonia.Controls;
using Avalonia.Interactivity;
using SignCAApp.ViewModels;

namespace SignCAApp.Views;

public partial class MainWindow : Window
{
    //public string Token;
    //public string BaseUrl;
    //public long FileId;
    public MainWindow(string baseUrl, string token, long fileId)
    {
        InitializeComponent();
        //DataContext = new MainWindowViewModel();  // Set ViewModel
        // Set MainWindow DataContext
        var mainViewModel = new MainWindowViewModel(baseUrl, token, fileId);
        DataContext = mainViewModel;

        // Set CertificateSelectionView DataContext in Code-Behind
        CertificateView.DataContext = mainViewModel.CertificateViewModel;
    }
}
