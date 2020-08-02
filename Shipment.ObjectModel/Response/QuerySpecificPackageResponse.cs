using Shipment.ObjectModel.DTO;

namespace Shipment.ObjectModel.Response
{
    public class QuerySpecificPackageResponse : BaseResponse
    {
        public ShipmentPackageDTO ShipmentPackage { get; set; }

        public QuerySpecificPackageResponse()
        {
        }

        public QuerySpecificPackageResponse(ShipmentPackageDTO package)
        {
            ShipmentPackage = package;
        }
    }
}
