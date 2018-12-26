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

        // GET: Statistics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Statistics/Create
        public ActionResult Create()
        {
            ViewBag.UID = new SelectList(db.Users, "ID", "FName");
            ViewBag.CID = new SelectList(db.Categories, "CID", "CName");
            return View();
        }

        // POST: Statistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PID,UID,PDate,LF,Closed,Descr,CID,Photo")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UID = new SelectList(db.Users, "ID", "FName", post.UID);
            ViewBag.CID = new SelectList(db.Categories, "CID", "CName", post.CID);
            return View(post);
        }

        // GET: Statistics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.UID = new SelectList(db.Users, "ID", "FName", post.UID);
            ViewBag.CID = new SelectList(db.Categories, "CID", "CName", post.CID);
            return View(post);
        }

        // POST: Statistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PID,UID,PDate,LF,Closed,Descr,CID,Photo")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UID = new SelectList(db.Users, "ID", "FName", post.UID);
            ViewBag.CID = new SelectList(db.Categories, "CID", "CName", post.CID);
            return View(post);
        }

        // GET: Statistics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Statistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
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
