using Microsoft.AspNetCore.Mvc;
using MVCData.Models;
using MVCServices;


namespace MVCWeb.Controllers
{
    public class VerhuurController : Controller
    {

        private readonly VerhuurService verhuurService;
        public VerhuurController(VerhuurService verhuurService)
        {

            this.verhuurService = verhuurService;
        }
        public IActionResult Index()
        {           
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }


    }
}
