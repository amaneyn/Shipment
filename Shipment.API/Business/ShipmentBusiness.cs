using Microsoft.Extensions.Caching.Memory;
using Shipment.ObjectModel.DTO;
using Shipment.SDK.ShipmentPackagesGenerator;
using Shipment.SDK.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipment.API.Business
{
    public class ShipmentBusiness : IShipmentBusiness
    {
        // Generated shipment packages are stored on Memory Cache, so we will not get a different list everytime API is queried
        private readonly IMemoryCache _memoryCache;
        private readonly IShipmentPackageGenerator _shipmentPackageGenerator;

        private const string shipmentPackagessCacheKeyPrefix = "ShipmentPackages";

        public ShipmentBusiness(IShipmentPackageGenerator shipmentPackageGenerator, IMemoryCache memoryCache)
        {
            _shipmentPackageGenerator = shipmentPackageGenerator;
            _memoryCache = memoryCache;
        }

        public List<ShipmentPackageDTO> GetShipmentPackages()
        {
            List<ShipmentPackageDTO> shipmentPackages = ManageShipmentPackagesDataCache();
            return shipmentPackages;
        }

        public ShipmentPackageDTO QuerySpecificPackage(string packageNumber)
        {
            List<ShipmentPackageDTO> shipmentPackages = ManageShipmentPackagesDataCache();
            return shipmentPackages.FirstOrDefault(sp => sp.PackageNumber == packageNumber);
        }

        private List<ShipmentPackageDTO> ManageShipmentPackagesDataCache()
        {
            List<ShipmentPackageDTO> shipmentPackages;
            if (!_memoryCache.TryGetValue(shipmentPackagessCacheKeyPrefix, out shipmentPackages))
            {
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.High)
                                                                                         .SetAbsoluteExpiration(TimeSpan.FromHours(5));

                shipmentPackages = _shipmentPackageGenerator.GenerateRandomShipmentPackages();
                if (shipmentPackages.HasElements())
                {
                    _memoryCache.Set(shipmentPackagessCacheKeyPrefix, shipmentPackages, cacheEntryOptions);
                }
            }

            return shipmentPackages;
        }
    }
}
