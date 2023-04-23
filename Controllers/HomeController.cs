using Kviz_Aplikacija___IT_Proektna_2023.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.DynamicData.ModelProviders;
using System.Web.Mvc;

namespace Kviz_Aplikacija___IT_Proektna_2023.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db=new ApplicationDbContext();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}