using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Interfaces.Services;
using ClientesGFT.WebApplication.Extensions;
using ClientesGFT.WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ClientesGFT.WebApplication.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var loggedUser = _userService.Get(User);
            if (!loggedUser.Roles.Contains(ERoles.ADMINISTRACAO)) return LocalRedirect("/");

            var users = _userService.GetAll(loggedUser);

            return View(users.ToViewModel());
        }

        [Route("create")]
        public IActionResult Create()
        {
            var loggedUser = _userService.Get(User);
            if (!loggedUser.Roles.Contains(ERoles.ADMINISTRACAO)) return LocalRedirect("/");

            ViewBag.Roles = new SelectList(_userService.GetRoles(), "Id", "DisplayName");
            var viewModel = new UserViewModel();

            return View("User", viewModel);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            var user = _userService.Get(id);
            ViewBag.Roles = new SelectList(_userService.GetRoles(), "Id", "DisplayName");

            return View("User", user.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendUser(UserViewModel userVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Roles = new SelectList(_userService.GetRoles(), "Id", "DisplayName");
                    return View("User", userVM);
                };

                var loggedUser = _userService.Get(User);

                userVM.Roles = _userService.FixRoles(userVM.RolesIds);

                if (userVM.HasId) _userService.Edit(userVM.ToModel());
                else _userService.Register(userVM.ToModel());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Roles = new SelectList(_userService.GetRoles(), "Id", "DisplayName");
                ModelState.AddModelError("", ex.Message);
                return View("User", userVM);
            }

        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _userService.Get(id);

            _userService.Delete(user);

            return RedirectToAction("Index");
        }
    }
}
