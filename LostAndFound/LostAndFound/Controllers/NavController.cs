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
        private LostAndFoundEntities1 db = new LostAndFoundEntities1();

        // GET: Nav
        public ActionResult Index()
        {
            //TO DO Get from session
            var id = 5;
            var user = db.Users.Where(m => m.ID == id);
            var model = user;
            return PartialView("~/Views/Shared/Layout.cshtml", model);
        }
    }
}