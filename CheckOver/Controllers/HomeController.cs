using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //AutomaticChecker.Run();
            return View();
        }
    }
}