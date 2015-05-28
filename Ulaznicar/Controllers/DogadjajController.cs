using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ulaznicar.Models;

namespace Ulaznicar.Controllers
{
    public class DogadjajController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: Dogadjaj
        public ActionResult Index()
        {
            var dogadjaj = db.Dogadjaj.Include(d => d.Lokacija);
            return View(dogadjaj.ToList());
        }

        // GET: Dogadjaj/Details/5
        public ActionResult Details(int? id)
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

        // GET: Dogadjaj/Create
        public ActionResult Create()
        {
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "adresa");
            return View();
        }

        // POST: Dogadjaj/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,naziv,datum,IdLokacija,brojmjesta,plakat")] Dogadjaj dogadjaj)
        {
            if (ModelState.IsValid)
            {
                db.Dogadjaj.Add(dogadjaj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "adresa", dogadjaj.IdLokacija);
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
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "adresa", dogadjaj.IdLokacija);
            return View(dogadjaj);
        }

        // POST: Dogadjaj/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,naziv,datum,IdLokacija,brojmjesta,plakat")] Dogadjaj dogadjaj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dogadjaj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "adresa", dogadjaj.IdLokacija);
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
