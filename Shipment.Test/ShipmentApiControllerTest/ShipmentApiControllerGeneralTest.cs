using Shipment.ObjectModel.DTO;
using System.Linq;
using Xunit;
using Shipment.ShipmentBusinessTest.Test;
using Shipment.API.Controllers;
using Shipment.ObjectModel.Response;
using Shipment.SDK.Extension;

namespace Shipment.ShipmentApiControllerTest.Test
{
    public class ShipmentApiControllerGeneralTest : IClassFixture<ShipmentApiControllerBaseTestFixture>
    {
        private readonly ShipmentApiControllerBaseTestFixture _baseFixture;

        public ShipmentApiControllerGeneralTest(ShipmentApiControllerBaseTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void CheckIfShipmentControllerGetsRandomizedPackages()
        {
            ShipmentController shipmentController = new ShipmentController(_baseFixture.MockedShipmentBusiness);
            GetShipmentPackagesResponse getShipmentPackagesResponse = shipmentController.GetShipmentPackages();
            
            Assert.True(getShipmentPackagesResponse != null && getShipmentPackagesResponse.ShipmentPackages.HasElements());
        }

        [Fact]
        public void CheckIfQueryingAnExistingPackageReturnsValidPackage()
        {
            ShipmentController shipmentController = new ShipmentController(_baseFixture.MockedShipmentBusiness);
            GetShipmentPackagesResponse shipmentPackagesResponse = shipmentController.GetShipmentPackages();
            ShipmentPackageDTO shipmentPackage = shipmentPackagesResponse.ShipmentPackages.First();

            QuerySpecificPackageResponse querySpecificPackageResponse = shipmentController.QuerySpecificPackage(shipmentPackage.PackageNumber);

            Assert.True(querySpecificPackageResponse != null && 
                        querySpecificPackageResponse.ShipmentPackage != null && 
                        querySpecificPackageResponse.ShipmentPackage.Equals(shipmentPackage));
        }

        [Fact]
        public void CheckIfQueryingANonExistingPackageReturnsNull()
        {
            ShipmentController shipmentController = new ShipmentController(_baseFixture.MockedShipmentBusiness);
            GetShipmentPackagesResponse shipmentPackagesResponse = shipmentController.GetShipmentPackages();
            ShipmentPackageDTO shipmentPackage = shipmentPackagesResponse.ShipmentPackages.First();
            
            QuerySpecificPackageResponse querySpecificPackageResponse = shipmentController.QuerySpecificPackage(shipmentPackage.PackageNumber + "asd");

            Assert.True(querySpecificPackageResponse != null &&
                        querySpecificPackageResponse.ShipmentPackage == null);
        }
    }
}
