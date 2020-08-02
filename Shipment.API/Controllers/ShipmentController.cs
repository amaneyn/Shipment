using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shipment.API.Business;
using Shipment.ObjectModel.DTO;
using Shipment.ObjectModel.Response;

namespace Shipment.API.Controllers
{
    [ApiController]
    [Route("{shipment}")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentBusiness _shipmentBusiness;

        public ShipmentController(IShipmentBusiness shipmentBusiness)
        {
            _shipmentBusiness = shipmentBusiness;
        }

        [HttpGet]
        public GetShipmentPackagesResponse GetShipmentPackages()
        {
            List<ShipmentPackageDTO> shipmentPackages = _shipmentBusiness.GetShipmentPackages();
            GetShipmentPackagesResponse response = new GetShipmentPackagesResponse(shipmentPackages);
            return response;
        }

        [HttpGet]
        [Route("{packageNumber}")]
        public QuerySpecificPackageResponse QuerySpecificPackage(string packageNumber)
        {
            ShipmentPackageDTO shipmentPackage = _shipmentBusiness.QuerySpecificPackage(packageNumber);
            QuerySpecificPackageResponse response = new QuerySpecificPackageResponse(shipmentPackage);
            return response;
        }
    }
}
