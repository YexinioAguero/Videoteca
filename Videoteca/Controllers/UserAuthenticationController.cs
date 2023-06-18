using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Videoteca.Models.DTO;
using Videoteca.Models.Repositories.Abstract;

namespace Videoteca.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        public UserAuthenticationController(IUserAuthenticationService service)
        {
            this._service = service;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.LoginAsync(model);

            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (result.StatusCode == 2) // Verificar si el estado es 2 (Inicio de sesión fallido)
                {
                    var user = await _service.GetUserByNameAsync(model.UserName);

                    if (user != null)
                    {
                        var lockoutEnabled = await _service.GetLockoutEnabledAsync(user);

                        if (lockoutEnabled) // Comprobar si el bloqueo está habilitado para el usuario
                        {
                            var failedCount = await _service.GetAccessFailedCountAsync(user);

                            if (failedCount >= 2) // Comprobar si el usuario ya ha fallado dos veces
                            {
                                await _service.LockUserAsync(user); // Bloquear al usuario
                                TempData["msg"] = "La cuenta ha sido bloqueada debido a múltiples intentos fallidos de inicio de sesión.";
                                return RedirectToAction(nameof(Login));
                            }
                        }
                    }
                }

                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //public async Task<IActionResult> Reg()
        //{
        //    var model = new RegistrationModel
        //    {
        //        UserName = "admin_w",
        //        Name = "Wilson-Mata",
        //        Email = "wilsonbm@gmail.com",
        //        Password = "Admin2023*",
        //    };
        //    model.Role = "admin";
        //    var result = await _service.RegistrationAsync(model);
        //    return Ok(result);
        //}

        public async Task<IActionResult> Reg()
        {
            var model = new RegistrationModel
            {
                UserName = "SuperAdminACY",
                Name = "SuperAdmin",
                Email = "SuperAdmin2088@gmail.com",
                Password = "SuperAdmin2023*",
            };
            model.Role = "superadmin";
            var result = await _service.RegistrationAsync(model);
            return Ok(result);

        }

            public async Task<IActionResult> RegDeneg()
            {
                var model = new RegistrationModel
                {
                    UserName = "UserDenegado",
                    Name = "Deneg",
                    Email = "Deneg2088@gmail.com",
                    Password = "Deneg2023*",
                };
                model.Role = "Denegado";
                var result = await _service.RegistrationAsync(model);
                return Ok(result);
            }

        //UserName: AlexM2088 pass:Aa258* //user
        //alex    Aa2588* //user
        // A18   Aa369*// admin
        // Allex   AdminPass1*// admin   after changes

    }
}
