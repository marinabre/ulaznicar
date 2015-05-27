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
    public class BurzaController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: Burza
        public ActionResult Index()
        {
            var burza = db.Burza.Include(b => b.Korisnik);
            return View(burza.ToList());
        }

        // GET: Burza/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Burza burza = db.Burza.Find(id);
            if (burza == null)
            {
                return HttpNotFound();
            }
            return View(burza);
        }

        // GET: Burza/Create
        public ActionResult Create()
        {
            ViewBag.IdKupac = new SelectList(db.Korisnik, "Id", "korisnickoime");
            return View();
        }

        // POST: Burza/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdKupac,IdKarta,datumponude,datumprodaje,cijena")] Burza burza)
        {
            if (ModelState.IsValid)
            {
                db.Burza.Add(burza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKupac = new SelectList(db.Korisnik, "Id", "korisnickoime", burza.IdKupac);
            return View(burza);
        }

        // GET: Burza/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Burza burza = db.Burza.Find(id);
            if (burza == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdKupac = new SelectList(db.Korisnik, "Id", "korisnickoime", burza.IdKupac);
            return View(burza);
        }

        // POST: Burza/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdKupac,IdKarta,datumponude,datumprodaje,cijena")] Burza burza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(burza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKupac = new SelectList(db.Korisnik, "Id", "korisnickoime", burza.IdKupac);
            return View(burza);
        }

        // GET: Burza/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Burza burza = db.Burza.Find(id);
            if (burza == null)
            {
                return HttpNotFound();
            }
            return View(burza);
        }

        // POST: Burza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Burza burza = db.Burza.Find(id);
            db.Burza.Remove(burza);
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
