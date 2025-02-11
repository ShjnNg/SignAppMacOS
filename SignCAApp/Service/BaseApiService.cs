using RestSharp;
using System.Net;

namespace SignCAApp.Service;

public class BaseApiService
{
    protected RestClient _restClient;
    protected RestRequest _request;

    public BaseApiService(string baseUrl, string token)
    {
        try
        {
            this._restClient = new RestClient(baseUrl);

            this.InitRestRequest(token);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        catch (System.Exception)
        {
            //throw;
        }
    }

    private void InitRestRequest(string token)
    {
        this._request = new RestRequest();
        this._request.AddHeader("Authorization", "Bearer " + token);
        this._request.AddHeader("Content-Type", "application/json");
    }
}
