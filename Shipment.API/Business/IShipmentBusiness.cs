using Shipment.ObjectModel.DTO;
using System.Collections.Generic;

namespace Shipment.API.Business
{
    public interface IShipmentBusiness
    {
        List<ShipmentPackageDTO> GetShipmentPackages();
        ShipmentPackageDTO QuerySpecificPackage(string packageNumber);
    }
}
