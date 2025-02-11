using iText.Commons.Bouncycastle.Asn1.X509;
using System.Collections.ObjectModel;

namespace SignCAApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Messenger { get; set; } = "Đang thực hiện ký số";
}

