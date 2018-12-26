using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LostAndFound.Models;

namespace LostAndFound.Controllers
{
    public class NavController : Controller
    {
        private LFModelEntities db = new LFModelEntities();

        // GET: Nav
        public ActionResult Index()
        {
            //TO DO Get from session
            if (Session["id"] == null)
            {
                return PartialView("~/Views/Shared/NavBarNotLogged.cshtml");
            }
            var id = (int)Session["id"];
            var users = db.Users.Where(m=>m.ID==id);
            if (users.Count() > 0)
            {
                var user = users.First();
                return PartialView("~/Views/Shared/NavBarLoggedIn.cshtml", user);
            }
            else
            {
                return PartialView("~/Views/Shared/NavBarNotLogged.cshtml");

            }

        }
    }
}