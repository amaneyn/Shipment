using Shipment.ObjectModel.DTO;
using Shipment.SDK.ShipmentPackagesGenerator;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Shipment.SDK.Extension;

namespace Shipment.ShipmentPackagesGeneratorTest.Test
{
    public class ShipmentAddressNumberTest : IClassFixture<ShipmentPackagesGeneratorBaseTestFixture>
    {
        private readonly ShipmentPackagesGeneratorBaseTestFixture _baseFixture;

        public ShipmentAddressNumberTest(ShipmentPackagesGeneratorBaseTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void CheckIfAddressNumbersAreUnique()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();

            Assert.True(packages.Select(p => p.DeliveryAddressNumber).Distinct().Count() == packages.Count());
        }

        [Fact]
        public void CheckIfAddressNumbersAreNullOrEmpty()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();
            IEnumerable<string> packageAddressNumbers = packages.Select(p => p.DeliveryAddressNumber);

            Assert.DoesNotContain(packageAddressNumbers, p => string.IsNullOrEmpty(p));
        }

        [Fact]
        public void CheckIfAddressNumbersAreTwoDigits()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();
            IEnumerable<string> packageAddressNumbers = packages.Select(p => p.DeliveryAddressNumber);

            Assert.True(packageAddressNumbers.All(p => p.Length == 2));
        }

        [Fact]
        public void CheckIfAddressNumbersAreNumeric()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();

            Assert.True(packages.Select(p => p.DeliveryAddressNumber).ConvertToInt().Count() == packages.Count());
        }
    }
}
