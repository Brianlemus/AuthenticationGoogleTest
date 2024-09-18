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
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, 
                new AuthenticationProperties
                {
                    RedirectUri= Url.Action("GoogleResponse")
                });
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
        /// Limpia la sessión y envia a la vista inicializador de login.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View("Index");
        }

    }
}
