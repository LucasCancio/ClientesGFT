using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.WebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ClientesGFT.WebApplication.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ApprovalController : Controller
    {
        private readonly IClientService _clienteService;
        private readonly IFluxoDeAprovacaoService _fluxoService;
        private readonly IUserService _usuarioService;

        public ApprovalController(IClientService clienteService, IFluxoDeAprovacaoService fluxoService, IUserService usuarioService)
        {
            _clienteService = clienteService;
            _fluxoService = fluxoService;
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            var loggedUser = _usuarioService.Get(User);

            var clientsToApprove = _clienteService.GetAllToApproveClients(loggedUser);
            var finalizedClients = _clienteService.GetAllFinalizedClients();
            var approvedClients = finalizedClients.Where(x => x.CurrentStatus.Description == EStatus.APROVADO);
            var repprovedClients = finalizedClients.Where(x => x.CurrentStatus.Description == EStatus.REPROVADO);

            var viewModel = new ApprovalViewModel()
            {
                ApprovedClients = approvedClients,
                ClientsToApprove = clientsToApprove,
                RepprovedClients = repprovedClients,
                Roles = loggedUser.Roles
            };

            return View(viewModel);
        }

        [HttpPost("approve/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            try
            {
                var loggedUser = _usuarioService.Get(User);

                var client = _clienteService.Get(id);

                _fluxoService.AprovarCliente(client, loggedUser);
            }
            catch (Exception ex)
            {
                if (ex is InvalidClientException || ex is InvalidUserException)
                {
                    TempData["Warning"] = ex.Message;
                }
                else
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("decline/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Decline(int id)
        {
            try
            {
                var loggedUser = _usuarioService.Get(User);

                var client = _clienteService.Get(id);

                _fluxoService.ReprovarCliente(client, loggedUser);

            }
            catch (Exception ex)
            {
                if (ex is InvalidClientException || ex is InvalidUserException)
                {
                    TempData["Warning"] = ex.Message;
                }
                else
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("correction/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult SendForCorrection(int id)
        {
            try
            {
                var loggedUser = _usuarioService.Get(User);

                var client = _clienteService.Get(id);

                _fluxoService.EnviarParaCorrecao(client, loggedUser);
            }
            catch (Exception ex)
            {
                if (ex is InvalidClientException || ex is InvalidUserException)
                {
                    TempData["Warning"] = ex.Message;
                }
                else
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return RedirectToAction("Index");
        }
    }
}
