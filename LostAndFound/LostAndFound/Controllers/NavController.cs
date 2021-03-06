﻿using System.Data;
using System.Linq;
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
                if (user.Type == false)
                {
                    return PartialView("~/Views/Shared/NavBarLoggedIn.cshtml", user);
                }
                else
                {
                    return PartialView("~/Views/Shared/NavBarAdmin.cshtml", user);

                }
            }
            else
            {
                return PartialView("~/Views/Shared/NavBarNotLogged.cshtml");

            }

        }
    }
}