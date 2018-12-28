using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LostAndFound.Models;

namespace LostAndFound.Controllers
{
    public class HomeController : Controller
    {
        private LFModelEntities db = new LFModelEntities();
        
        public ActionResult Index()
        {
            var userid = Session["id"];
            if (userid != null)
            {
                ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
                var posts = db.Posts.Take(50).Where(m => m.Closed == false); 
                List<Post> PostsList = posts.ToList();
                return View(PostsList);
            }
            else
            {
                return View("Welcome");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(int CID, string query)
        {
            if (query == "")
            {
                List<Post> po = new List<Post>();
                ViewBag.ResultsNum = 0;
                ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
                return View(po);
            }
            var posts = db.Posts.Where(m=>m.Descr.Contains(query)).Where(m=>m.Closed==false).Where(m=>m.CID==CID);
            List<Post> dcf = posts.ToList();
            ViewBag.ResultsNum = dcf.Count();
            ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
            return View(dcf);
        }
    }
}