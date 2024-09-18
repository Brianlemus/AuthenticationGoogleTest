using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace AuthGoggleUMG.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Autenticación con OAUTH 2.0 De GOOGLE.
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"),
                Items =
        {
            { "prompt", "select_account" } // Fuerza la pantalla de selección de cuenta
        }
            };

            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
        }

        /// <summary>
        /// Verificación de respuesta de autenticación si esta de acuerdo en permitir el acceso.
        /// Confirmación.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GoogleResponse ()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            //return Json(claims);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        /// <summary>
        /// Limpia la sesión y envía a la vista inicial de login.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            // Cierra la sesión de cookies (autenticación local)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al usuario a la vista de login después de cerrar sesión
            return RedirectToAction("Index");
        }

    }
}
