using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.Domain.Util;
using ClientesGFT.WebApplication.Models;
using ClientesGFT.WebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace ClientesGFT.WebApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IFluxoDeAprovacaoService _fluxoService;

        public HomeController(IFluxoDeAprovacaoService fluxoService)
        {
            _fluxoService = fluxoService;
        }


        public IActionResult Index()
        {
            var filter = new FluxoFilter()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            var fluxos = _fluxoService.ListarFluxo(filter);

            var viewModel = new HomeViewModel()
            {
                Fluxos = fluxos,
                Filter= filter
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult GetFluxos([FromBody] FluxoFilter filter)
        {
            var fixedFluxos = _fluxoService.ListarFluxo(filter)
                        .Select(fluxo=> {
                            return new
                            {
                                userName= fluxo.User.Name,
                                clientId= fluxo.ClientId,
                                clientCPF = fluxo.Client.CPF,
                                clientName= fluxo.Client.Name,
                                status= fluxo.Status.Description.GetDisplayName(),
                                createDate = fluxo.CreateDate.ToString()
                            };
                        })
                        .ToList();

            return new ObjectResult(fixedFluxos);
        }

        [HttpPost]
        public IActionResult DownloadReport(FluxoFilter filter)
        {
            var fluxos = _fluxoService.ListarFluxo(filter);

            var stream = ReportPrinter.PrintAsStream(fluxos, filter);
            var filename = ReportPrinter.GetFileName();

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
