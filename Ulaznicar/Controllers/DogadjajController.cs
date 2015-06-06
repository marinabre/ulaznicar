using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ulaznicar.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity.Infrastructure;

namespace Ulaznicar.Controllers
{
    public class DogadjajController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: Dogadjaj
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            ViewBag.user = userUserName;



            ViewBag.CurrentSort = sortOrder;
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.DatumSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LokSortParm = String.IsNullOrEmpty(sortOrder) ? "lok_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var dogadjaj = db.Dogadjaj.Where(s => s.datum >= DateTime.Now).Include(d => d.Lokacija);

            if (!String.IsNullOrEmpty(searchString))
            {
                dogadjaj = dogadjaj.Where(s => s.naziv.Contains(searchString)
                                       || s.Lokacija.naziv.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "naziv_desc":
                    dogadjaj = dogadjaj.OrderByDescending(s => s.naziv);
                    break;
                case "Date":
                    dogadjaj = dogadjaj.OrderByDescending(s => s.datum);
                    break;
                case "date_desc":
                    dogadjaj = dogadjaj.OrderByDescending(s => s.datum);
                    break;
                case "lok_desc":
                    dogadjaj = dogadjaj.OrderByDescending(s => s.Lokacija.naziv);
                    break;
                default:  // Name ascending 
                    dogadjaj = dogadjaj.OrderBy(s => s.datum);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(dogadjaj.ToPagedList(pageNumber, pageSize));

        }

        // GET: Dogadjaj/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogadjaj dogadjaj = db.Dogadjaj.Find(id);
            ViewBag.cijene = db.CijenaKarte.Where(x => x.IdDogadjaj == id);
            if (dogadjaj == null)
            {
                return HttpNotFound();
            }
            return View(dogadjaj);
        }

        // GET: Dogadjaj/Create
        public ActionResult Create()
        {
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv");
            return View();
        }

        // POST: Dogadjaj/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase upload, [Bind(Include = "Id,naziv,datum,IdLokacija,brojmjesta,plakat")] Dogadjaj dogadjaj)
        {
             try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        dogadjaj.plakat = reader.ReadBytes(upload.ContentLength);
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Dogadjaj.Add(dogadjaj);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv", dogadjaj.IdLokacija);
            }
             catch (RetryLimitExceededException /* dex */  )
             {
                 //Log the error (uncomment dex variable name and add a line here to write a log.
                 ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
             }
            return View(dogadjaj);
        }

        // GET: Dogadjaj/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogadjaj dogadjaj = db.Dogadjaj.Find(id);
            if (dogadjaj == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv", dogadjaj.IdLokacija);
            return View(dogadjaj);
        }

        // POST: Dogadjaj/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,naziv,datum,IdLokacija,brojmjesta,plakat")] Dogadjaj dogadjaj, HttpPostedFileBase upload)
        {
            if (upload == null)
            {
                var dogadjaji = (db.Dogadjaj.AsNoTracking().Where(x => x.Id == dogadjaj.Id));
                dogadjaj.plakat = dogadjaji.FirstOrDefault(x => x.Id == dogadjaj.Id).plakat;
            }
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        dogadjaj.plakat = reader.ReadBytes(upload.ContentLength);
                    }
                }

                db.Entry(dogadjaj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv", dogadjaj.IdLokacija);
            return View(dogadjaj);
        }

        // GET: Dogadjaj/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogadjaj dogadjaj = db.Dogadjaj.Find(id);
            if (dogadjaj == null)
            {
                return HttpNotFound();
            }
            return View(dogadjaj);
        }

        // POST: Dogadjaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dogadjaj dogadjaj = db.Dogadjaj.Find(id);
            db.Dogadjaj.Remove(dogadjaj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Show(int id)
        {
            var plakat = db.Dogadjaj.Find(id).plakat; 
            if (plakat == null)
            {
                return base.File("~/Content/noimage.png", "image/png");
            }

            return new FileContentResult(plakat, "img");
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
