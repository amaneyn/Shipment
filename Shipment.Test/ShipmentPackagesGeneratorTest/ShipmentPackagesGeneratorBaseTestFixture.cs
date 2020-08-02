using Microsoft.Extensions.Configuration;
using Moq;
using System;

namespace Shipment.ShipmentPackagesGeneratorTest.Test
{
    public class ShipmentPackagesGeneratorBaseTestFixture : IDisposable
    {
        public IConfiguration MockConfiguration;

        public ShipmentPackagesGeneratorBaseTestFixture()
        {
            var mockConfSection = new Mock<IConfigurationSection>();

            // Difference between _minQuantity and _maxQuantity MUST NOT BE bigger than 89. Because Address Numbers must be unique and 2 digits
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "MinQuantity")]).Returns("50");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "MaxQuantity")]).Returns("80");

            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "MinWeight")]).Returns("10");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "MaxWeight")]).Returns("25");

            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "MaxDeliverableWeight")]).Returns("500");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "DeliveryVehicleCharge")]).Returns("80");

            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "DeliveryTimeBetweenStopsInMinutes")]).Returns("1");

            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "WorkingHourStartHour")]).Returns("9");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "WorkingStartMinute")]).Returns("0");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "WorkingHourEndHour")]).Returns("18");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "WorkingHourEndMinute")]).Returns("30");

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "AppSettings"))).Returns(mockConfSection.Object);

            MockConfiguration = mockConfiguration.Object;
        }

        public void Dispose()
        {
        }
    }
}
