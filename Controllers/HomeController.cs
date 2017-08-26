using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Im.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //если залогинен то на страницу
            //если не залогинен то на главную
            return View();
        }

        
    }
}