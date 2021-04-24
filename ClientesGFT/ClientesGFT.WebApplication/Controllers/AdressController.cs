using ClientesGFT.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientesGFT.WebApplication.Controllers
{
    [Route("[controller]")]
    public class AdressController : Controller
    {
        private readonly IAdressService _adressService;

        public AdressController(IAdressService adressService)
        {
            _adressService = adressService;
        }

        [Route("states/{countryId:int}")]
        public JsonResult GetStates(int countryId)
        {
            var states = new SelectList(_adressService.GetStates(countryId), "Id", "Description");
            return Json(states);
        }

        [Route("cities/{stateId:int}")]
        public JsonResult GetCities(int stateId)
        {
            var cities = new SelectList(_adressService.GetCities(stateId), "Id", "Description");
            return Json(cities);
        }

    }
}
