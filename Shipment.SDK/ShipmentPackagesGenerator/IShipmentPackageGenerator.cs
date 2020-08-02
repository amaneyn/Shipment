using Shipment.ObjectModel.DTO;
using System.Collections.Generic;

namespace Shipment.SDK.ShipmentPackagesGenerator
{
    public interface IShipmentPackageGenerator
    {
        List<ShipmentPackageDTO> GenerateRandomShipmentPackages();
    }
}
