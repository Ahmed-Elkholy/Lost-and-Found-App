using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using LostAndFound.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Web;
using System.IO;

namespace LostAndFound.Controllers
{
    public class UsersController : Controller
    {
        private LFModelEntities db = new LFModelEntities();

        // SRC: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.md5?redirectedfrom=MSDN&view=netframework-4.7.2
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // GET: Users/EditProfile
        public ActionResult EditProfile(int id)
        {
            if (Session["id"] == null || id != (int)Session["id"])
                return View("~/Views/Error404.cshtml");

            var user = db.Users.Where(u => u.ID == id).First();
            RegisterModel rModel = new RegisterModel
            {
                FName = user.FName,
                LName = user.LName,
                Email = user.Email,
                Mobile = user.Mobile,
                Photo = user.Photo
            };
            return View(rModel);
        }

        public ActionResult UpdateUser([Bind(Exclude = "Photo")] User user, HttpPostedFileBase Photo)
        {
            if (Session["id"] == null)
                return View("~/Views/Error404.cshtml");

            if (ModelState.IsValid)
            {
                var user_retrieved = db.Users.Where(u => u.Email == user.Email).First();
                
                if (Photo != null)
                {
                    byte[] buf = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(buf, 0, buf.Length);
                }
                MD5 md5Hash = MD5.Create();
                user_retrieved.Password = GetMd5Hash(md5Hash, user.Password);
                user_retrieved.FName = user.FName;
                user_retrieved.LName = user.LName;
                user_retrieved.Email = user.Email;
                user_retrieved.Mobile = user.Mobile;
                user_retrieved.Photo = user.Photo;
                db.SaveChanges();
                return RedirectToAction("Index");   
            }
            return View(user);
        }

        // GET: Users/ResetToken
        public ActionResult ResetToken()
        {
            return View();
        }

        // POST: Users/ResetToken
        [HttpPost]
        public ActionResult ResetToken(int token, string password)
        {
            MD5 md5Hash = MD5.Create();
            string hashed = GetMd5Hash(md5Hash, password);
            var email = Session["email"];
            if (Session["token"].Equals(token) && password.Length >= 8 && password.Length <= 40)
            {
                var entry = db.Users.Where(u => u.Email == email.ToString()).ToList().First();
                entry.Password = hashed;
                db.SaveChanges();
            }
            return View();
        }


        // POST: Users/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "Photo")] User user, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                var users_retrieved = db.Users.Where(u => u.Email == user.Email).ToList();
                var filePath = "";
                var fileName = "";
                if (Photo != null)
                {
                    fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Photo.FileName);

                    var uploadUrl = Server.MapPath("~/imgs/User");
                    filePath = Path.Combine(uploadUrl, fileName);
                    while (System.IO.File.Exists(filePath))
                    {
                        fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Photo.FileName);
                        filePath = Path.Combine(uploadUrl, fileName);

                    }
                    Photo.SaveAs(filePath);
                }
                if (fileName != null)
                {
                    filePath = "/imgs/User/" + fileName;
                }
                if (users_retrieved.Count == 0)
                {
                    MD5 md5Hash = MD5.Create();
                    user.Password = GetMd5Hash(md5Hash, user.Password);
                    user.Photo = filePath;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index","Home");
                }
            }
            return View(user);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Users/Failed
        public ActionResult Failed()
        {
            return View();
        }

        // GET: Users/Failed
        public ActionResult Reset()
        {
            return View();
        }

        // POST: Users/Reset
        [HttpPost]
        public ActionResult Reset([Bind(Include = "Email")] User user)
        {
            var email = db.Users.Where(u => u.Email == user.Email).ToList().First().Email;
            Random random = new Random();
            var num = random.Next(0, 100000);
            Session["token"] = num;
            Session["email"] = email;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential("lostandfound.project.2018@gmail.com", "Consultation1234");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage mail = new MailMessage();
            smtpClient.EnableSsl = true;
            //Setting From , To and CC
            mail.From = new MailAddress("lostandfound.project.2018@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Password reset link";
            mail.Body = "Your recovery pin is " + num;
            smtpClient.Send(mail);
            return View("~/Views/Users/ResetToken.cshtml");
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] User user)
        {
            var passwordLength = user.Password.Length;
            if (IsValidEmail(user.Email) && passwordLength >= 8 && passwordLength <= 40)
            {
                MD5 md5Hash = MD5.Create();
                var user_retrieved = db.Users.Where(u => u.Email == user.Email).ToList();
                if (user_retrieved.Count == 1 && VerifyMd5Hash(md5Hash, user.Password, user_retrieved.First().Password))
                {
                    Session["id"] = user_retrieved.First().ID;
                    Session["email"] = user.Email;
                    Session["type"] = user.Type;
                    return RedirectToAction("Index","Home");
                }
                else
                    return RedirectToAction("Failed");
            }

            return View(user);
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
