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
    public class ProfileController : Controller
    {
        // GET: Profile
        private LFModelEntities db = new LFModelEntities();

        public ActionResult Index(int id)
        {
            id = 2;
            if (id == 0)
            {
                
                //take session id or return failed
            }

            var users = db.Users.Where(m=>m.ID==id);
            if (users.Count()>0)
            {
                return View(users.First());
            }
            return View(users.ToList());
        }
    }
}