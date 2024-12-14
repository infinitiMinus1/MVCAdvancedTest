using Microsoft.AspNetCore.Mvc;

namespace MVCWeb.ViewComponents
{
    public class Begroeting : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Begroeting(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IViewComponentResult Invoke()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            if (session.IsAvailable &&
                session.GetString("Naam") != null &&
                session.GetString("Voornaam") != null)
            {
                string naam = session.GetString("Naam");
                string voornaam = session.GetString("Voornaam");
                ViewData["Begroeting"] = $"Welkom, {voornaam} {naam}";
            }
            else
            {
                ViewData["Begroeting"] = "Welkom! Meld je aan om verder te gaan.";
            }

            return View();
        }
    }
    }

