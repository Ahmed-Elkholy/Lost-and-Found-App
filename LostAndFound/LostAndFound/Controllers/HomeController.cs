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
        /*
        public ActionResult Index(string sortOrder)
        {
            var posts = from s in db.Posts
                           select s;
            
            if (sortOrder !=null && sortOrder.Equals("Date"))
            {
                posts = posts.OrderByDescending(s => s.PDate);
            }
            List<Post> dcf = posts.ToList();

            return View(dcf);
        }*/
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

        public ActionResult Search(int CID,string query)
        {
            var posts = db.Posts.Where(m=>m.Descr.Contains(query)).Where(m=>m.Closed==false).Where(m=>m.CID==CID);
            List<Post> dcf = posts.ToList();
            ViewBag.ResultsNum = dcf.Count();
            ViewBag.CategoryList = new SelectList(db.Categories, "CID", "CName");
            return View(dcf);
        }


    }
}