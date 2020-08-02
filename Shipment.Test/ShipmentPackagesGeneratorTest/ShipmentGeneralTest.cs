using Shipment.ObjectModel.DTO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System;
using Shipment.SDK.ShipmentPackagesGenerator;

namespace Shipment.ShipmentPackagesGeneratorTest.Test
{
    public class ShipmentGeneralTest : IClassFixture<ShipmentPackagesGeneratorBaseTestFixture>
    {
        private readonly ShipmentPackagesGeneratorBaseTestFixture _baseFixture;

        public ShipmentGeneralTest(ShipmentPackagesGeneratorBaseTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void CheckIfPackageCountIsBetweenFiftyAndEighty()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();

            Assert.True(packages.Count >= 50 && packages.Count <= 80);
        }

        [Fact]
        public void CheckIfPackageWeightsAreBetweenTenAndTwentyFive()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();

            Assert.True(packages.All(p => p.PackageWeightInKilos >= 10 && p.PackageWeightInKilos <= 25));
        }

        [Fact]
        public void CheckIfLastDeliverablePackagesDeliveryTimeIsBeforeWorkingHoursEnd()
        {
            ShipmentPackageGenerator shipmentPackageGenerator = new ShipmentPackageGenerator(_baseFixture.MockConfiguration);
            List<ShipmentPackageDTO> packages = shipmentPackageGenerator.GenerateRandomShipmentPackages();
            ShipmentPackageDTO lastDeliverablePackage = packages.Last(p => p.ApproximateDeliveryTime.HasValue);

            int workingHourEndHour, workingHourEndMinute;
            int.TryParse(_baseFixture.MockConfiguration.GetSection("AppSettings")["WorkingHourEndHour"], out workingHourEndHour);
            int.TryParse(_baseFixture.MockConfiguration.GetSection("AppSettings")["WorkingHourEndMinute"], out workingHourEndMinute);

            var endWorkingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, workingHourEndHour, workingHourEndMinute, 0);


            Assert.True(lastDeliverablePackage.ApproximateDeliveryTime < endWorkingTime);
        }
    }
}
