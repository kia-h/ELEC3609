using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.DAL;
using PSP_EF.Models;


namespace PSP_EF.Controllers
{
    public class HomeController : Controller
    {
        PetDbContext _db = new PetDbContext();

       public ActionResult Index()
        {
            
            var country = _db.Countries.ToList();
            return View(country);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard ttt";

            return View();
        }
    }
}
