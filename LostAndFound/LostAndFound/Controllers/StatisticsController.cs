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
    public class StatisticsController : Controller
    {
        private LFModelEntities db = new LFModelEntities();

        // GET: Statistics
        public ActionResult Index()
        {
            var countMissing = db.Posts.Where(m => m.Closed == false).Count();
            var countFound = db.Posts.Where(m => m.Closed == true).Count();
            StatisticsModel s = new StatisticsModel
            {
                CountMissing = countMissing,
                CountFound = countFound
            };
            return View(s);
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
