using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using recyclebin2.Models;

namespace recyclebin2.Controllers
{
    public class AdminsController : Controller
    {
        private TestingSdProjectDBEntities2 db = new TestingSdProjectDBEntities2();

        //// GET: Admins
        //public ActionResult Index()
        //{
        //    return View(db.Admins.ToList());
        //}

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminID,AdminName,AdminPassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminID,AdminName,AdminPassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Admin admin = db.Admins.Find(id);
        //    if (admin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(admin);
        //}

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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



        public ActionResult AdminList()
        {
            return View(db.Admins.ToList());
        }

        public ActionResult UserList()
        {
            return View(db.User1.ToList());
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ContactList()
        {
            return View(db.Contacts.ToList());
        }

        public ActionResult ProductList()
        {
            return View(db.Products.ToList());
        }

        public ActionResult CategoryList()
        {
            return View(db.Catagories.ToList());

        }

        public ActionResult CategoryDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagory catagory = db.Catagories.Find(id);

            db.Catagories.Remove(catagory);
            db.SaveChanges();
            return RedirectToAction("CategoryList");
            if (catagory == null)
            {
                return HttpNotFound();
            }
            return View(catagory);
        }

      
        public ActionResult DeleteUser(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User1 user1 = db.User1.Find(id);


            db.User1.Remove(user1);
            db.SaveChanges();
            return RedirectToAction("UserList");
            if (user1 == null)
            {
                return HttpNotFound();
            }
            return View(user1);
        }

        public ActionResult AdminDelete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);

            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("AdminList");
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        public ActionResult ContactDelete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);

            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("ContactList");

            if (contact == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("ContactList");
            }
            return View(contact);
        }
        
        public ActionResult ProductList(int id)
        {
           
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName", product.CatagoryID);
                return View(product);
            }
        }
}


