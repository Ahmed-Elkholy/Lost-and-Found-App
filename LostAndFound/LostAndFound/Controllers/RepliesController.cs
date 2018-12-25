using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LostAndFound.Models;

namespace LostAndFound.Controllers
{
    public class RepliesController : Controller
    {
        private LostAndFoundEntities1 db = new LostAndFoundEntities1();

        // GET: Replies
        public ActionResult Index(int pid)
        {
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
