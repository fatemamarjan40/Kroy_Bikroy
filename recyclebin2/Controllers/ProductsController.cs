using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using recyclebin2.Models;
using PagedList;


namespace recyclebin2.Controllers
{
    public class ProductsController : Controller
    {
        private TestingSdProjectDBEntities2 db = new TestingSdProjectDBEntities2();
        
        // GET: Products
        public ActionResult Index()

        {
            //var products = db.Products.Include(p => p.Catagory);
           // return View(products.ToList());
         if (TempData["cart"] != null)
            {
                float x = 0;
                List<cartModel> li2 = TempData["cart"] as List<cartModel>;
                foreach (var item in li2)
                {
                    x += item.bill;

                }

                TempData["total"] = x;
            }
            TempData.Keep();
            return View(db.Products.OrderByDescending(x=>x.ProductID).ToList());
        }

        [HttpPost]
        public ActionResult Index(int? id, int? page, string search)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x => x.ProductName.Contains(search)).OrderByDescending(x => x.ProductID).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


        }

        public ActionResult Adtocart(int? Id)
        {

            Product p = db.Products.Where(x => x.ProductID == Id).SingleOrDefault();
            return View(p);
        }

        List<cartModel> li = new List<cartModel>();
        [HttpPost]
        //removed tbl_product pi
        public ActionResult Adtocart(Product pi, string qty, int Id)
        {
            Product p = db.Products.Where(x => x.ProductID == Id).SingleOrDefault();

            favList c = new favList();
            c.productid = p.ProductID;
            c.price = p.ProductPrice;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.ProductName = p.ProductName;
            if (TempData["cart"] == null)
            {
                         //li.Add(c);

                    db.favLists.Add(c);
                    db.SaveChanges();
             
                TempData["cart"] = li;

            }
            else
            {
                List<favList> li2 = TempData["cart"] as List<favList>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.productid==c.productid)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                        
                    }
                    
                }
                if (flag==0)
                {
                     li2.Add(c);    
                }
               
                TempData["cart"] = li2;
            }

            TempData.Keep();




            return RedirectToAction("Index");
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
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
            List<Product> sim_products = db.Products.Where(p => p.CatagoryID == product.CatagoryID && p.ProductID != product.ProductID).Take(5).ToList<Product>();
            ViewBag.SimilarProducts = sim_products;
            return View(product);


        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductPrice,ProductDiscription,CatagoryID,ProductImage,ProductBrand,ProductCondition")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName", product.CatagoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName", product.CatagoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductPrice,ProductDiscription,CatagoryID,ProductImage,ProductBrand,ProductCondition")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName", product.CatagoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
        public ActionResult Add()
        {
            ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName");
            return View();
            //return View();
        }

        [HttpPost]
        public ActionResult Add(Product imageModel)
        {

            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ProductImage = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);


            if (ModelState.IsValid)
            {
                db.Products.Add(imageModel);
                db.SaveChanges();
                //return RedirectToAction("Add");
            }
            ModelState.Clear();
            ViewBag.CatagoryID = new SelectList(db.Catagories, "CatagoryID", "CatagoryName", imageModel.CatagoryID);
            return View(imageModel);


            //  ModelState.Clear();
            // return View();
        }
        [HttpGet]
        /* public ActionResult View(int id)
         {
             Product12 imageModel = new Product12();
             imageModel = db.Product12.Where(x => x.ProductID == id).FirstOrDefault();

             if (imageModel == null)
             {
                 return HttpNotFound();
             }

             return View(imageModel);

         }
        */

        public ActionResult ProductShow(int? page)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.OrderByDescending(x => x.ProductID).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);

        }

        [HttpPost]
        public ActionResult ProductShow(int? id, int? page, string search)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x => x.ProductName.Contains(search)).OrderByDescending(x => x.ProductID).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


        }


        //public ActionResult Index(int? page)
        //{
        //    int pageNumber = page ?? 1;
        //    int pageSize = 5;
        //    var catList = db.Catagories.OrderBy(x => x.CatagoryName).ToPagedList(pageNumber, pageSize);
        //    return View(catList);



        //    // return View(db.Catagories.ToList());
        //}
        /* public PartialViewResult CategoryPartial()
         {
             var categoryList = db.Catagories.OrderBy(x => x.CatagoryName).ToList();
             return PartialView(categoryList);


         }*/
        [HttpGet]
        public PartialViewResult ProductListPartial(int? page, int? category)
        {
            //var categoryList = db.Catagories.OrderBy(x => x.CatagoryName).ToList();
            var pageNumber = page ?? 1;
            var pageSize = 10;
            if (category != null)
            {
                ViewBag.category = category;
                var productList = db.Products
                                .OrderByDescending(x => x.ProductID)
                                .Where(x => x.CatagoryID == category)
                                .ToPagedList(pageNumber, pageSize);
                return PartialView(productList);
            }
            else
            {
                var productList = db.Products.OrderByDescending(x => x.ProductID).ToPagedList(pageNumber, pageSize);
                return PartialView(productList);
            }
        }

        public ActionResult checkout()
        {
            TempData.Keep();
            
            return View(db.favLists.ToList());
       }
           



    }
}
