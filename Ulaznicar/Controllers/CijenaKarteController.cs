using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ulaznicar.Models;

namespace Ulaznicar.Controllers
{
    public class CijenaKarteController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: CijenaKarte
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {
                var cijenaKarte = db.CijenaKarte.Include(c => c.Dogadjaj).Include(c => c.VrstaKarte);
                return View(cijenaKarte.ToList());
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // GET: CijenaKarte/Details/5
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CijenaKarte cijenaKarte = db.CijenaKarte.Find(id);
                if (cijenaKarte == null)
                {
                    return HttpNotFound();
                }
                return View(cijenaKarte);
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // GET: CijenaKarte/Create
        public ActionResult Create()
        {            
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {
                ViewBag.IdDogadjaj = new SelectList(db.Dogadjaj, "Id", "naziv");
                ViewBag.IdVrstaKarte = new SelectList(db.VrstaKarte, "Id", "imekategorije");
                return View();
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // POST: CijenaKarte/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdDogadjaj,IdVrstaKarte,cijena")] CijenaKarte cijenaKarte)
        {
            if (ModelState.IsValid)
            {
                db.CijenaKarte.Add(cijenaKarte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDogadjaj = new SelectList(db.Dogadjaj, "Id", "naziv", cijenaKarte.IdDogadjaj);
            ViewBag.IdVrstaKarte = new SelectList(db.VrstaKarte, "Id", "imekategorije", cijenaKarte.IdVrstaKarte);
            return View(cijenaKarte);
        }

        // GET: CijenaKarte/Edit/5
        public ActionResult Edit(int? id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CijenaKarte cijenaKarte = db.CijenaKarte.Find(id);
                if (cijenaKarte == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdDogadjaj = new SelectList(db.Dogadjaj, "Id", "naziv", cijenaKarte.IdDogadjaj);
                ViewBag.IdVrstaKarte = new SelectList(db.VrstaKarte, "Id", "imekategorije", cijenaKarte.IdVrstaKarte);
                return View(cijenaKarte);
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // POST: CijenaKarte/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdDogadjaj,IdVrstaKarte,cijena")] CijenaKarte cijenaKarte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cijenaKarte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDogadjaj = new SelectList(db.Dogadjaj, "Id", "naziv", cijenaKarte.IdDogadjaj);
            ViewBag.IdVrstaKarte = new SelectList(db.VrstaKarte, "Id", "imekategorije", cijenaKarte.IdVrstaKarte);
            return View(cijenaKarte);
        }

        // GET: CijenaKarte/Delete/5
        public ActionResult Delete(int? id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CijenaKarte cijenaKarte = db.CijenaKarte.Find(id);
                if (cijenaKarte == null)
                {
                    return HttpNotFound();
                }
                return View(cijenaKarte);
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // POST: CijenaKarte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CijenaKarte cijenaKarte = db.CijenaKarte.Find(id);
            db.CijenaKarte.Remove(cijenaKarte);
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
