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
    public class UsersListController : Controller
    {
        private LFModelEntities db = new LFModelEntities();

        // GET: UsersList
        public ActionResult Index()
        {
            if (Session["id"] == null || (bool)Session["type"] != true)
                return View("~/Views/Error404.cshtml");

            return View(db.Users.ToList());
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
