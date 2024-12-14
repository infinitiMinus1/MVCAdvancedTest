using Microsoft.AspNetCore.Mvc;
using MVCData.Models;
using MVCServices;
using MVCWeb.Models;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Transactions;

namespace MVCWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VerhuurService verhuurService;
        public HomeController(ILogger<HomeController> logger, VerhuurService verhuurService)
        {
            _logger = logger;
            this.verhuurService = verhuurService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("klant") == "onbekend")
            {
                TempData["Message"] = "Onbekende klant, probeer opnieuw.";
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Voornaam")))
            {
                ViewBag.isAlIngelogd = true;
            }
            return View(new KlantViewModel());
        }


        public IActionResult Aanmelden(KlantViewModel m)
        {
            if (this.ModelState.IsValid)
            {
                var klant = verhuurService.SearchKlant(m.Postcode, m.VoorNaam.ToUpper());
                if (klant == null)
                {
                    HttpContext.Session.SetString("klant", "onbekend");
                    return RedirectToAction("Index");
                }
                else
                {
                    var klantVolledig = JsonSerializer.Serialize(klant);
                    HttpContext.Session.SetString("klant", klantVolledig);
                    HttpContext.Session.SetString("Voornaam", klant.VoorNaam);
                    HttpContext.Session.SetString("Naam", klant.Naam);
                    return RedirectToAction("Genres");
                }

            }
            else
                TempData["Message"] = "Inlog poging ongeldig";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Genres()
        {
            var genres = verhuurService.GetGenres();
            return View(genres);

        }


        public IActionResult FilmKeuze(int genreId)
        {
            IEnumerable<Film> films = verhuurService.GetFilmByGenre(genreId);
            ViewBag.genre = verhuurService.GetGenreNaam(genreId);
            return View(films);
        }


        public IActionResult WinkelmandToevoegen(int id)
        {
            Film? film = verhuurService.GetMovie(id);

            if (film != null) //film kan eigenlijk nooit null zijn hier, maar ik doe toch een null check voor zekerheid
            {
                var sessionVariabeleFilms = HttpContext.Session.GetString("GekozenFilms");
                List<Film>? gekozenFilms;
                if (string.IsNullOrEmpty(sessionVariabeleFilms))
                    gekozenFilms = new List<Film>();
                else
                    gekozenFilms = JsonSerializer.Deserialize<List<Film>>(sessionVariabeleFilms);
                if (gekozenFilms.All(f => f.FilmId != film.FilmId))
                {
                    gekozenFilms?.Add(film);
                    var geserializeerdeLijst = JsonSerializer.Serialize(gekozenFilms);
                    HttpContext.Session.SetString("GekozenFilms", geserializeerdeLijst);
                    return RedirectToAction("Winkelmand");
                }
                else
                {
                    TempData["Message"] = "Je hebt deze film al in je winkelmand";
                    return RedirectToAction("FilmKeuze", new { genreId = film.GenreId });
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Winkelmand()
        {
            {
                var sessionVariabeleBestellingen = HttpContext.Session.GetString("GekozenFilms");
                List<Film>? gekozenFilms = string.IsNullOrEmpty(sessionVariabeleBestellingen)
                    ? new List<Film>()
                    : JsonSerializer.Deserialize<List<Film>>(sessionVariabeleBestellingen);

                return View(gekozenFilms);
            }
        }

        public IActionResult VerwijderUitWinkelmand(int id)
        {
            Film film = verhuurService.GetMovie(id);
            return View(film);
        }

        public IActionResult VerwijderUitWinkelmandDoorvoeren(int id)
        {
            var film = verhuurService.GetMovie(id);
            var sessionVariabele = HttpContext.Session.GetString("GekozenFilms");
            List<Film>? gekozenFilms = JsonSerializer.Deserialize<List<Film>>(sessionVariabele);
            var filmToRemove = gekozenFilms.FirstOrDefault(f => f.FilmId == id);
            gekozenFilms.Remove(filmToRemove);
            var geserializeerdeLijst = JsonSerializer.Serialize(gekozenFilms);
            HttpContext.Session.SetString("GekozenFilms", geserializeerdeLijst);

            TempData["message"] = "Film verwijderd";
            return RedirectToAction("Winkelmand");
        }

        public IActionResult Afrekenen()
        {
            var klantGegevens = HttpContext.Session.GetString("klant");
            TempData["Client"] = JsonSerializer.Deserialize<Klant>(klantGegevens);
            var sessionVariabeleBestellingen = HttpContext.Session.GetString("GekozenFilms");
            List<Film>? gekozenFilms = string.IsNullOrEmpty(sessionVariabeleBestellingen)
                ? new List<Film>()
                : JsonSerializer.Deserialize<List<Film>>(sessionVariabeleBestellingen);
            return View(gekozenFilms);
        }

        public IActionResult AfrekenenDoorvoeren()
        {
            using (var transactionScope = new TransactionScope())
            {

                var sessionVariabele = HttpContext.Session.GetString("GekozenFilms");
                List<Film>? films = JsonSerializer.Deserialize<List<Film>>(sessionVariabele);
                films.ForEach(film =>
                {
                    var klantGegevens = HttpContext.Session.GetString("klant");
                    Klant klant = JsonSerializer.Deserialize<Klant>(klantGegevens);
                    verhuurService.GetMovieFromStock(film.FilmId);
                    verhuurService.AddVerhuur(new Verhuring { VerhuurDatum = DateTime.Now, FilmId = film.FilmId, KlantId = klant.KlantId });
                });
                //opruimen
                films.Clear();
                var geserializeerdeLijst = JsonSerializer.Serialize(films);
                HttpContext.Session.SetString("GekozenFilms", geserializeerdeLijst);

                transactionScope.Complete();
            }

            return RedirectToAction("AfrekenBevestiging");

        }

        public IActionResult AfrekenBevestiging()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
