using System;
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
        private LFModelEntities db = new LFModelEntities();

        // GET: Posts
        public ActionResult Index(int id)
        {
            if (Session["id"] != null)
            { 
                var posts = db.Posts.Where(p => p.PID == id).Include(u => u.User);
               
                return View(posts.ToList());
            }
            return View("~/Views/Error404.cshtml");
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            var userid = Session["id"];
            if (userid != null)
            {
                ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");

            }
        }

        // POST: Posts/Create
        [HttpPost]
        public ActionResult Create(PostViewModel post, HttpPostedFileBase Photo)
        {
            if (Session["id"] == null)
                return View("~/Views/Error404.cshtml");
           
            var fileName = "";

            var UID = (int)Session["id"];
            if (ModelState.IsValid)
            {
                var filePath = "";
                if (Photo != null)
                {
                    fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Photo.FileName);

                    if (fileName != null)
                    {
                        var uploadUrl = Server.MapPath("~/imgs/Post");
                        filePath = Path.Combine(uploadUrl, fileName);
                        while (System.IO.File.Exists(filePath))
                        {
                            fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Photo.FileName);
                            filePath = Path.Combine(uploadUrl, fileName);

                        }
                        Photo.SaveAs(filePath);
                    }
                }
                if (fileName != "")
                {
                    filePath = "/imgs/Post/" + fileName;
                }
                else
                {
                    filePath = null;
                }
                Post NewPost = new Post
                {
                    Descr = post.Descr,
                    PDate = DateTime.Now,
                    LF = post.LF,
                    UID = UID,
                    CID = post.CID,
                    Photo = filePath
                };
                db.Posts.Add(NewPost);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception)
                {
                }
                //save photo here
                return RedirectToAction("Index","Home");
            }

            ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
            return View();
        }

        [HttpPost]
        public void AddReply(int pid, string replyText)
        {
            if (Session["id"] == null)
                return;

            Reply reply = new Reply
            {
                PID = pid,
                UID = (int)Session["id"],
                RDate = DateTime.Now,
                Descr = replyText
            };
            db.Replies.Add(reply);
            db.SaveChanges();
        }

        [HttpPost]
        public void AddReport(int pid)
        {
            if (Session["id"] == null)
                return;

            Report report = new Report
            {
                PID = pid,
                UID = (int)Session["id"]
            };
            if (db.Reports.Count(x => x.UID == report.UID && x.PID == pid) == 0)
            {
                db.Reports.Add(report);
                db.SaveChanges();
            }
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
