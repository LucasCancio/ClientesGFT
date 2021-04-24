using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.WebApplication.Enums;
using ClientesGFT.WebApplication.Extensions;
using ClientesGFT.WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ClientesGFT.WebApplication.Controllers
{
    [Route("[controller]")]
    public class OperationController : Controller
    {
        private readonly IClientService _clienteService;
        private readonly IFluxoDeAprovacaoService _fluxoService;
        private readonly IUserService _usuarioService;
        private readonly IAdressService _adressService;

        public OperationController(IClientService clienteService, IFluxoDeAprovacaoService fluxoService, IUserService usuarioService, IAdressService adressService)
        {
            _clienteService = clienteService;
            _fluxoService = fluxoService;
            _usuarioService = usuarioService;
            _adressService = adressService;
        }

        public IActionResult Index()
        {
            var loggedUser = _usuarioService.Get(User);
            if (!loggedUser.Roles.Contains(ERoles.OPERACAO) && !loggedUser.Roles.Contains(ERoles.ADMINISTRACAO)) return LocalRedirect("/");

            var clients = _clienteService.GetAllOperationClients(loggedUser);

            return View(clients.ToViewModel());
        }

        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(_adressService.GetCountries(), "Id", "Description");
            var viewModel = new ClientViewModel();

            return View("Client", viewModel);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            var client = _clienteService.Get(id, withPhones: true);
            ViewBag.Countries = new SelectList(_adressService.GetCountries(), "Id", "Description", client.Adress.City.State.Country.Id);

            var viewModel = client.ToViewModel();
            viewModel.ViewState = EViewStates.EDIT;

            return View("Client", viewModel);
        }

        [HttpGet("select/{id:int}")]
        public IActionResult Select(int id)
        {
            string returnUrl = HttpContext.Request.Query["ReturnUrl"].ToString();

            var client = _clienteService.Get(id, withPhones: true);
            ViewBag.Countries = new SelectList(_adressService.GetCountries(), "Id", "Description", client.Adress.City.State.Country.Id);

            ClientViewModel viewModel = client.ToViewModel();
            viewModel.ViewState = EViewStates.READONLY;

            ViewData["ReturnUrl"] = returnUrl;
            return View("Client", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendClient(ClientViewModel clientVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Countries = new SelectList(_adressService.GetCountries(), "Id", "Description");
                    return View("Client", clientVM);
                };

                var loggedUser = _usuarioService.Get(User);

                clientVM.Phones = _clienteService.FixPhones(clientVM.Id, clientVM.PhonesNumbers);

                if (clientVM.HasId) _clienteService.Edit(clientVM.ToModel());
                else _clienteService.Register(clientVM.ToModel(), loggedUser);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Countries = new SelectList(_adressService.GetCountries(), "Id", "Description");
                ModelState.AddModelError("", ex.Message);
                return View("Client", clientVM);
            }

        }

        [HttpPost("approve/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var loggedUser = _usuarioService.Get(User);

            var client = _clienteService.Get(id);

            _fluxoService.AprovarCliente(client, loggedUser);

            return RedirectToAction("Index");
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var loggedUser = _usuarioService.Get(User);

            var client = _clienteService.Get(id);

            _clienteService.Delete(client, loggedUser);

            return RedirectToAction("Index");
        }

        

    }
}
