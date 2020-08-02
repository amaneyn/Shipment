using Shipment.ObjectModel.Response;

namespace Shipment.WebUI.ApiClient
{
    public interface IApiClient
    {
        GetShipmentPackagesResponse GetShipmentPackages();

        QuerySpecificPackageResponse QuerySpecificPackage(string packageNumber);
    }
}
