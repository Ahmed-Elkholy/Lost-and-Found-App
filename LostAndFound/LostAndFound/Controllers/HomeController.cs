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
    public class HomeController : Controller
    {
        private LostAndFoundEntities1 db = new LostAndFoundEntities1();
        /*
        public ActionResult Index()
        {
            var posts = db.Posts;
            List<Post> dcf = posts.ToList();
            //Post p = new Post();
            //p.Descr = "dfdf";
            //dcf.Add(p);

            return View(dcf);
        }*/

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

        public ActionResult Search(string query)
        {
            var posts = db.Posts.Where(m=>m.Descr.Contains(query));
            List<Post> dcf = posts.ToList();
            return View(dcf);

        }


    }
}