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
    public class LokacijaController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: Lokacija
        public ActionResult Index()
        {
            var lokacija = db.Lokacija.Include(l => l.Grad);
            return View(lokacija.ToList());
        }

        // GET: Lokacija/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lokacija lokacija = db.Lokacija.Find(id);
            if (lokacija == null)
            {
                return HttpNotFound();
            }
            return View(lokacija);
        }

        // GET: Lokacija/Create
        public ActionResult Create()
        {
            ViewBag.IdGrad = new SelectList(db.Grad, "Id", "imegrad");
            return View();
        }

        // POST: Lokacija/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,adresa,naziv,IdGrad")] Lokacija lokacija)
        {
            if (ModelState.IsValid)
            {
                db.Lokacija.Add(lokacija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdGrad = new SelectList(db.Grad, "Id", "imegrad", lokacija.IdGrad);
            return View(lokacija);
        }

        // GET: Lokacija/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lokacija lokacija = db.Lokacija.Find(id);
            if (lokacija == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdGrad = new SelectList(db.Grad, "Id", "imegrad", lokacija.IdGrad);
            return View(lokacija);
        }

        // POST: Lokacija/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,adresa,naziv,IdGrad")] Lokacija lokacija)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lokacija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdGrad = new SelectList(db.Grad, "Id", "imegrad", lokacija.IdGrad);
            return View(lokacija);
        }

        // GET: Lokacija/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lokacija lokacija = db.Lokacija.Find(id);
            if (lokacija == null)
            {
                return HttpNotFound();
            }
            return View(lokacija);
        }

        // POST: Lokacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lokacija lokacija = db.Lokacija.Find(id);
            db.Lokacija.Remove(lokacija);
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
