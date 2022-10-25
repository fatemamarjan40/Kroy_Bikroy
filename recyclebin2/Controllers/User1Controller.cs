using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using recyclebin2.Models;

namespace recyclebin2.Controllers
{
    public class User1Controller : Controller
    {
        private TestingSdProjectDBEntities2 db = new TestingSdProjectDBEntities2();

        // GET: User1
        public ActionResult Index()
        {
            return View(db.User1.ToList());
        }

        // GET: User1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User1 user1 = db.User1.Find(id);
            if (user1 == null)
            {
                return HttpNotFound();
            }
            return View(user1);
        }

        // GET: User1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserFirstName,UserLastName,UserEmail,UserPassword,UserContact,UserAddress")] User1 user1)
        {
            if (ModelState.IsValid)
            {
                db.User1.Add(user1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user1);
        }

        // GET: User1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User1 user1 = db.User1.Find(id);
            if (user1 == null)
            {
                return HttpNotFound();
            }
            return View(user1);
        }

        // POST: User1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserFirstName,UserLastName,UserEmail,UserPassword,UserContact,UserAddress")] User1 user1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user1);
        }

        // GET: User1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User1 user1 = db.User1.Find(id);
            if (user1 == null)
            {
                return HttpNotFound();
            }
            return View(user1);
        }

        // POST: User1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User1 user1 = db.User1.Find(id);
            db.User1.Remove(user1);
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
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User1 user)
        {
            if (ModelState.IsValid)
            {
                if (db.User1.Any(x => x.UserEmail == user.UserEmail))
                {
                    return Content("already exits");
                }

                db.User1.Add(user);
                db.SaveChanges();
                return Redirect("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tempUser tempuser)
        {
            if (ModelState.IsValid)
            {
                var user = db.User1.Where(u => u.UserEmail.Equals(tempuser.UserEmail) && u.UserPassword.Equals(tempuser.UserPassword))
                    .FirstOrDefault();

                if (user != null)
                {
                    Session["fname"] = user.UserFirstName;
                    Session["lname"] = user.UserLastName;
                    Session["email"] = user.UserEmail;
                    Session["pass"] = user.UserPassword;
                    Session["id"] = user.UserID;
                    ViewBag.id = user.UserID;
                    return RedirectToAction("dashboard");
                    // return RedirectToAction("Edit", new { id = user.UserID });
                    // return Content("Login successfull");
                }
                else
                {
                    ViewBag.LoginFailed = "User not Found";
                    return View();
                }


            }
            return View();
        }
        public ActionResult DashBoard()
        {

            var id = Convert.ToInt32(Session["id"]);
            // ViewBag.id=User.;
            var user = db.Users.Where(u => u.UserID.Equals(id)).FirstOrDefault();
            return View(user);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove("id");
            Session.Abandon(); 
            return RedirectToAction("index", "Home");
        }
        public ActionResult About()
        {
            //.//ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "ContactID,UploadTime,Message,CotactEmail")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Contact");
            }

            return View(contact);
        }

    }
}
