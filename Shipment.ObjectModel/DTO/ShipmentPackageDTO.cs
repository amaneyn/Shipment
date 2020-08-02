using System;
using System.ComponentModel.DataAnnotations;

namespace Shipment.ObjectModel.DTO
{
    public class ShipmentPackageDTO
    {
        [Display(Name = "Package Number")]
        public string PackageNumber { get; set; }
        
        [Display(Name = "Address Number")]
        public string DeliveryAddressNumber { get; set; }

        [Display(Name = "Package Weight")]
        public int PackageWeightInKilos { get; set; }
        
        [Display(Name = "Delivery Time")]
        public DateTime? ApproximateDeliveryTime { get; set; }

        public bool LastStopOfTour { get; set; }
    }
}
