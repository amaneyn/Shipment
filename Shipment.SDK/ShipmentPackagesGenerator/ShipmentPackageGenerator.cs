using Microsoft.Extensions.Configuration;
using Shipment.ObjectModel.DTO;
using Shipment.SDK.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipment.SDK.ShipmentPackagesGenerator
{
    public class ShipmentPackageGenerator : IShipmentPackageGenerator
    {
        private readonly IConfiguration _configuration;

        // Difference between _minQuantity and _maxQuantity MUST NOT BE bigger than 89. Because Address Numbers must be unique and 2 digits
        private int _minQuantity;
        private int _maxQuantity;

        private int _minWeight;
        private int _maxWeight;

        private int _maxDeliverableWeight;
        private int _deliveryVehicleCharge;

        private DateTime _startTime;
        private DateTime _endTime;

        // Average delivery time between stops is 1 minute
        private int _deliveryTimeBetweenStopsInMinutes;

        public ShipmentPackageGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            int.TryParse(_configuration.GetSection("AppSettings")["MinQuantity"], out _minQuantity);
            int.TryParse(_configuration.GetSection("AppSettings")["MaxQuantity"], out _maxQuantity);

            int.TryParse(_configuration.GetSection("AppSettings")["MinWeight"], out _minWeight);
            int.TryParse(_configuration.GetSection("AppSettings")["MaxWeight"], out _maxWeight);

            int.TryParse(_configuration.GetSection("AppSettings")["MaxDeliverableWeight"], out _maxDeliverableWeight);
            int.TryParse(_configuration.GetSection("AppSettings")["DeliveryVehicleCharge"], out _deliveryVehicleCharge);

            int.TryParse(_configuration.GetSection("AppSettings")["DeliveryTimeBetweenStopsInMinutes"], out _deliveryTimeBetweenStopsInMinutes);

            int workingHourStartHour, workingHourStartMinute, workingHourEndHour, workingHourEndMinute;
            int.TryParse(_configuration.GetSection("AppSettings")["WorkingHourStartHour"], out workingHourStartHour);
            int.TryParse(_configuration.GetSection("AppSettings")["WorkingStartMinute"], out workingHourStartMinute);
            int.TryParse(_configuration.GetSection("AppSettings")["WorkingHourEndHour"], out workingHourEndHour);
            int.TryParse(_configuration.GetSection("AppSettings")["WorkingHourEndMinute"], out workingHourEndMinute);

            _startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, workingHourStartHour, workingHourStartMinute, 0);
            _endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, workingHourEndHour, workingHourEndMinute, 0);
        }

        public List<ShipmentPackageDTO> GenerateRandomShipmentPackages()
        {
            List<ShipmentPackageDTO> packages = GenerateRandomPackageList();
           
            List<ShipmentPackageDTO> deliverablePackages = new List<ShipmentPackageDTO>();
            while (_startTime.AddMinutes(_deliveryTimeBetweenStopsInMinutes * 2) <= _endTime && // 1 delivery time must be reserved for returning to delivery center
                  deliverablePackages.Count < packages.Count)
            {
                // Calculate remaining packages
                IEnumerable<ShipmentPackageDTO> remainingPackages = packages.Except(deliverablePackages);

                // Calculate deliverable packages for one tour
                deliverablePackages.AddRange(CalculateDeliverablePackagesForOneTour(remainingPackages));

                // Last stop of tour is marked
                deliverablePackages.Last().LastStopOfTour = true;

                // Return to delivery center for another load
                _startTime = _startTime.AddMinutes(_deliveryTimeBetweenStopsInMinutes);
            }

            foreach(var package in packages)
            {
                ShipmentPackageDTO deliverablePackage = deliverablePackages.FirstOrDefault(dp => dp.PackageNumber == package.PackageNumber);
                if (deliverablePackage != null) 
                { 
                    package.ApproximateDeliveryTime = deliverablePackage.ApproximateDeliveryTime;
                }
            }

            return packages;
        }

        private List<ShipmentPackageDTO> GenerateRandomPackageList()
        {
            List<ShipmentPackageDTO> packages = new List<ShipmentPackageDTO>();

            int packageQuantity = RandomNumberHelper.GetRandomInt(_minQuantity, _maxQuantity);

            // Get unique 2 digit numbers
            IEnumerable<int> uniqueAddressNumbers = RandomNumberHelper.GetUniqueRandomNumbers(10, 99, packageQuantity);
            int[] uniqueAddressNumbersArray = uniqueAddressNumbers.ToArray();

            // Get unique MINIMUM 8 digit numbers
            IEnumerable<int> uniquePackageNumbers = RandomNumberHelper.GetUniqueRandomNumbers(10000000, int.MaxValue, packageQuantity);
            int[] uniquePackageNumbersArray = uniquePackageNumbers.ToArray();

            for (int i = 0; i < packageQuantity; i++)
            {
                var package = new ShipmentPackageDTO();
                package.PackageWeightInKilos = RandomNumberHelper.GetRandomInt(_minWeight, _maxWeight);
                package.DeliveryAddressNumber = uniqueAddressNumbersArray[i].ToString();
                package.PackageNumber = uniquePackageNumbersArray[i].ToString();

                packages.Add(package);
            }

            // Order packages by weight so that maximum number of packages can be delivered in short time periods
            packages = packages.OrderBy(p => p.PackageWeightInKilos).ToList();

            return packages;
        }

        private List<ShipmentPackageDTO> CalculateDeliverablePackagesForOneTour(IEnumerable<ShipmentPackageDTO> packages)
        {
            List<ShipmentPackageDTO> deliverablePackages = new List<ShipmentPackageDTO>();

            foreach (ShipmentPackageDTO package in packages)
            {
                int currentLoad = deliverablePackages.Sum(dp => dp.PackageWeightInKilos);
                if (currentLoad + package.PackageWeightInKilos <= _maxDeliverableWeight && // current load should not exceed max weight capacity
                    _startTime.AddMinutes(_deliveryTimeBetweenStopsInMinutes * 2) <= _endTime && // 1 delivery time must be reserved for returning to delivery center
                    deliverablePackages.Count < _deliveryVehicleCharge - _deliveryTimeBetweenStopsInMinutes) // 1 delivery KM (1 charge) must be reserved for returning to delivery center
                {
                    _startTime = _startTime.AddMinutes(_deliveryTimeBetweenStopsInMinutes);
                    package.ApproximateDeliveryTime = _startTime;
                    deliverablePackages.Add(package);
                }
            }

            return deliverablePackages;
        }
    }
}
