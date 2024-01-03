using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YEMEKCI.Models;
using YEMEKCI.Security;

namespace YEMEKCI.Controllers
{
    public class AnaSayfaController : Controller
    {
        YemekciEntities model = new YemekciEntities();
        // GET: AnaSayfa
        public ActionResult Index()
        {
            return View();
        }
    }
}