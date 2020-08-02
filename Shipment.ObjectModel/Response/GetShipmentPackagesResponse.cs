using Shipment.ObjectModel.DTO;
using System.Collections.Generic;

namespace Shipment.ObjectModel.Response
{
    public class GetShipmentPackagesResponse : BaseResponse
    {
        public List<ShipmentPackageDTO> ShipmentPackages { get; set; }

        public GetShipmentPackagesResponse()
        {
            ShipmentPackages = new List<ShipmentPackageDTO>();
        }

        public GetShipmentPackagesResponse(List<ShipmentPackageDTO> packages)
        {
            ShipmentPackages = packages;
        }
    }
}
