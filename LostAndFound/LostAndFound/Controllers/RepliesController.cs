﻿using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LostAndFound.Models;

namespace LostAndFound.Controllers
{
    public class RepliesController : Controller
    {
        private LFModelEntities db = new LFModelEntities();

        // GET: Replies
        public ActionResult Index(int pid)
        {
            if (Session["id"] == null)
                return View("~/Views/Error404.cshtml");

            var replies = db.Replies.Where(r => r.PID == pid).Include(u => u.User);
            return PartialView(replies.ToList());
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
