using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shipment.ObjectModel.DTO;
using Shipment.ObjectModel.Response;
using Shipment.WebUI.Models;

namespace Shipment.WebUI.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly ApiClient.IApiClient _apiClient;

        public ShipmentController(ApiClient.IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Index()
        {
            GetShipmentPackagesResponse response = _apiClient.GetShipmentPackages();
            
            if (response != null && response.ShipmentPackages != null)
            {
                return View(response.ShipmentPackages);
            }
            
            return View(new List<ShipmentPackageDTO>());
        }

        public IActionResult QueryShipment(string packageNumber)
        {
            if (string.IsNullOrEmpty(packageNumber))
            {
                return View();
            }

            QuerySpecificPackageResponse response = _apiClient.QuerySpecificPackage(packageNumber);

            if (response != null && response.ShipmentPackage != null)
            {
                return View(response.ShipmentPackage);
            }
            else
            {
                ViewBag.Message = $"Could not find a package with the following number: {packageNumber}";
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
