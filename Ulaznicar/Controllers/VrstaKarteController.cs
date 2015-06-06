using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ulaznicar.Models;

namespace Ulaznicar.Controllers
{
    public class VrstaKarteController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: VrstaKarte
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var vrstaKarte = db.VrstaKarte.Include(v => v.Lokacija);

                if (!String.IsNullOrEmpty(searchString))
                {
                    vrstaKarte = vrstaKarte.Where(s => s.imekategorije.Contains(searchString)
                                           || s.Lokacija.naziv.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "naziv_desc":
                        vrstaKarte = vrstaKarte.OrderByDescending(s => s.imekategorije);
                        break;
                    default:  // Name ascending 
                        vrstaKarte = vrstaKarte.OrderBy(s => s.Lokacija.naziv);
                        break;
                }

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                return View(vrstaKarte.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // GET: VrstaKarte/Details/5
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
                VrstaKarte vrstaKarte = db.VrstaKarte.Find(id);
                if (vrstaKarte == null)
                {
                    return HttpNotFound();
                }
                return View(vrstaKarte);
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // GET: VrstaKarte/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {

                ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv");
                return View();
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // POST: VrstaKarte/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,imekategorije,IdLokacija")] VrstaKarte vrstaKarte)
        {
            if (ModelState.IsValid)
            {
                db.VrstaKarte.Add(vrstaKarte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv", vrstaKarte.IdLokacija);
            return View(vrstaKarte);
        }

        // GET: VrstaKarte/Edit/5
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
                VrstaKarte vrstaKarte = db.VrstaKarte.Find(id);
                if (vrstaKarte == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv", vrstaKarte.IdLokacija);
                return View(vrstaKarte);
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // POST: VrstaKarte/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,imekategorije,IdLokacija")] VrstaKarte vrstaKarte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vrstaKarte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdLokacija = new SelectList(db.Lokacija, "Id", "naziv", vrstaKarte.IdLokacija);
            return View(vrstaKarte);
        }

        // GET: VrstaKarte/Delete/5
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
                VrstaKarte vrstaKarte = db.VrstaKarte.Find(id);
                if (vrstaKarte == null)
                {
                    return HttpNotFound();
                }
                return View(vrstaKarte);
            }
            return RedirectToAction("Index", "Dogadjaj");
        }

        // POST: VrstaKarte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VrstaKarte vrstaKarte = db.VrstaKarte.Find(id);
            db.VrstaKarte.Remove(vrstaKarte);
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
