using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SignCAAppMacOS.Service;
public class KySoService : BaseApiService
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();

    public KySoService(string baseUrl, string token) : base(baseUrl, token)
    {
    }

    public X509Certificate2 CertificateSelectorMacOS()
    {
        // Mở store chứng chỉ hệ thống (Keychain trên macOS)
        X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);

        // Lấy danh sách chứng chỉ
        X509Certificate2Collection certificates = store.Certificates;
        //// Hiển thị thông tin về các chứng chỉ
        //foreach (X509Certificate2 cert in certificates)
        //{
        //    Console.WriteLine($"Chứng chỉ: {cert.Subject}");
        //}
        if (certificates.Count > 0)
        {
            return certificates[0];
        }
        else return null;
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

    public async Task<string> SendCertToFile(long fileId, X509Certificate2 cert)
    {
        try
        {
            string base64Certificate = Convert.ToBase64String(cert.RawData);
            string pemCertificate = $"-----BEGIN CERTIFICATE-----\n{base64Certificate}\n-----END CERTIFICATE-----";
            //thêm api call gửi cert lên file
            //this._request.Resource = $"api/ky-so";
            this._request.AddQueryParameter("file_id", fileId);

            var signInfo = new SignInfoModel
            {
                FileId = fileId,
                Cert = cert
            };
            this._request.AddJsonBody(JsonConvert.SerializeObject(signInfo));
            this._request.Method = RestSharp.Method.Post;
            var response = this._restClient.Execute(this._request);
            _logger.Info($"Kết quả nhận được từ api/xu-ly-ky-so: {response.StatusCode}\r\nresponse.Content:{response.Content}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return "Success";
            }
            else return "Fail: " + response.ErrorException?.Message;
        }
        catch (Exception ex)
        {
            return "ERROR GetCertificate:" + ex;
        }
    }

    public class SignInfoModel
    {
        [JsonProperty("fileId")]
        public long FileId { get; set; }

        [JsonProperty("cert")]
        public X509Certificate2 Cert;
    }
}
