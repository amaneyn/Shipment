using Microsoft.Extensions.Configuration;
using RestSharp;
using Shipment.ObjectModel.Response;

namespace Shipment.WebUI.ApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly RestClient _restClient;

        public ApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            string apiGatewayBaseUrl =  _configuration.GetSection("AppSettings")["ApiGatewayUrl"];
            _restClient = new RestClient(apiGatewayBaseUrl);
        }

        public GetShipmentPackagesResponse GetShipmentPackages()
        {
            RestRequest request = new RestRequest("shipment-service/shipment", Method.GET);
            IRestResponse<GetShipmentPackagesResponse> response = _restClient.Execute<GetShipmentPackagesResponse>(request);
            return response.Data;
        }

        public QuerySpecificPackageResponse QuerySpecificPackage(string packageNumber)
        {
            RestRequest request = new RestRequest($"shipment-service/shipment/{packageNumber}", Method.GET);
            IRestResponse<QuerySpecificPackageResponse> response = _restClient.Execute<QuerySpecificPackageResponse>(request);
            return response.Data;
        }
    }
}
