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

    public class ReportController : Controller
    {
        private LFModelEntities db = new LFModelEntities();
        // GET: Report
        public ActionResult Index()
        {
            var reports = db.Reports.GroupBy(m=>m.PID).Select(g => new { pid = g.Key, count = g.Count() });
            List<ReportViewModel> ReportList = new List<ReportViewModel>();
            foreach(var report in reports)
            {
                var post = db.Posts.Find(report.pid);
                ReportViewModel rm = new ReportViewModel
                {
                    PID = report.pid,
                    CategoryName = post.Category.CName,
                    NumberOfReports = report.count,
                    Descr = post.Descr
                };
                ReportList.Add(rm);
                
            }
            return View(ReportList);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            db.SaveChanges();
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            if (report == null || post == null)
            {
                return HttpNotFound();
            }
            return View("Index",report);
        }


    }
}