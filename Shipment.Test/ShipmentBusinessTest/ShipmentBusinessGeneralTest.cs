using Shipment.ObjectModel.DTO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Shipment.ShipmentBusinessTest.Test;
using Shipment.API.Business;

namespace Shipment.ShipmentBusinessTest.Test
{
    public class ShipmentBusinessGeneralTest : IClassFixture<ShipmentBusinessBaseTestFixture>
    {
        private readonly ShipmentBusinessBaseTestFixture _baseFixture;

        public ShipmentBusinessGeneralTest(ShipmentBusinessBaseTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void CheckIfRandomlyCreatedShipmentPackagesAreStoredInCache()
        {
            ShipmentBusiness shipmentBusiness = new ShipmentBusiness(_baseFixture.MockedShipmentPackageGenerator, _baseFixture.MockedCache);
            List<ShipmentPackageDTO> shipmentPackagesInitial = shipmentBusiness.GetShipmentPackages();
            List<ShipmentPackageDTO> shipmentPackagesCached = shipmentBusiness.GetShipmentPackages();

            Assert.True(shipmentPackagesInitial.All(shipmentPackagesCached.Contains) && shipmentPackagesInitial.Count == shipmentPackagesCached.Count);
        }

        [Fact]
        public void CheckIfQueryingAnExistingPackageReturnsValidPackage()
        {
            ShipmentBusiness shipmentBusiness = new ShipmentBusiness(_baseFixture.MockedShipmentPackageGenerator, _baseFixture.MockedCache);
            List<ShipmentPackageDTO> shipmentPackages = shipmentBusiness.GetShipmentPackages();
            ShipmentPackageDTO shipmentPackage = shipmentPackages.First();
            
            ShipmentPackageDTO queriedShipmentPackage = shipmentBusiness.QuerySpecificPackage(shipmentPackage.PackageNumber);

            Assert.True(queriedShipmentPackage != null && queriedShipmentPackage.Equals(shipmentPackage));
        }

        [Fact]
        public void CheckIfQueryingANonExistingPackageReturnsNull()
        {
            ShipmentBusiness shipmentBusiness = new ShipmentBusiness(_baseFixture.MockedShipmentPackageGenerator, _baseFixture.MockedCache);
            List<ShipmentPackageDTO> shipmentPackages = shipmentBusiness.GetShipmentPackages();
            ShipmentPackageDTO shipmentPackage = shipmentPackages.First();

            ShipmentPackageDTO queriedShipmentPackage = shipmentBusiness.QuerySpecificPackage(shipmentPackage.PackageNumber + "asd");

            Assert.True(queriedShipmentPackage == null);
        }
    }
}
