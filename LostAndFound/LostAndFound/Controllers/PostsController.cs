using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LostAndFound.Models;

namespace LostAndFound.Controllers
{
    public class PostsController : Controller
    {
        private LostAndFoundEntities1 db = new LostAndFoundEntities1();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.User);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
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

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(PostViewModel post, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                
                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Photo.FileName);

                var uploadUrl = Server.MapPath("~/Images/Post");
                var filePath = Path.Combine(uploadUrl, fileName);
                while(System.IO.File.Exists(filePath))
                {
                    fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Photo.FileName);
                    filePath = Path.Combine(uploadUrl, fileName);
                }
                Photo.SaveAs(filePath);
                Post NewPost = new Post
                {
                    Descr = post.Descr,
                    PDate = DateTime.Now,
                    LF = post.LF,
                    UID = 2,
                    CID = 1,
                    Photo = filePath
                };
                db.Posts.Add(NewPost);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    int x = 5;
                    x++;
                }
                //save photo here
                return RedirectToAction("Index");
            }

            //ViewBag.UID = new SelectList(db.Users, "ID", "FName", post.UID);
            return View(post);
        }

        // GET: Posts/Edit/5
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
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PID,UID,PDate,LF,Closed")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UID = new SelectList(db.Users, "ID", "FName", post.UID);
            return View(post);
        }

        // GET: Posts/Delete/5
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

        // POST: Posts/Delete/5
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
