using Shipment.ObjectModel.DTO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Shipment.SDK.ShipmentPackagesGenerator;
using Shipment.SDK.Extension;

namespace Shipment.ShipmentPackagesGeneratorTest.Test
{
    public class ShipmentPackageNumberTest : IClassFixture<ShipmentPackagesGeneratorBaseTestFixture>
    {
        private readonly ShipmentPackagesGeneratorBaseTestFixture _baseFixture;

        public ShipmentPackageNumberTest(ShipmentPackagesGeneratorBaseTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void CheckIfPackageNumbersAreUnique()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();

            Assert.True(packages.Select(p => p.PackageNumber).Distinct().Count() == packages.Count());
        }

        [Fact]
        public void CheckIfPackageNumbersAreNullOrEmpty()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();
            IEnumerable<string> packageNumbers = packages.Select(p => p.PackageNumber);

            Assert.DoesNotContain(packageNumbers, p => string.IsNullOrEmpty(p));
        }

        [Fact]
        public void CheckIfPackageNumbersAreMinimumEightDigits()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();
            IEnumerable<string> packageNumbers = packages.Select(p => p.PackageNumber);

            Assert.True(packageNumbers.All(p => p.Length >= 8));
        }

        [Fact]
        public void CheckIfPackageNumbersAreNumeric()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();

            Assert.True(packages.Select(p => p.PackageNumber).ConvertToInt().Count() == packages.Count());
        }
    }
}
