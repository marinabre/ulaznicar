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
    public class CijenaKarteController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: CijenaKarte
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            if (userUserName == "ADMIN")
            {

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
                ViewBag.ImeSortParm = String.IsNullOrEmpty(sortOrder) ? "ime_desc" : "";
                ViewBag.CijenaSortParm = String.IsNullOrEmpty(sortOrder) ? "cijena_desc" : "";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var cijenaKarte = db.CijenaKarte.Include(c => c.Dogadjaj).Include(c => c.VrstaKarte);

                if (!String.IsNullOrEmpty(searchString))
                {
                    cijenaKarte = cijenaKarte.Where(s => s.VrstaKarte.imekategorije.Contains(searchString)
                                           || s.Dogadjaj.naziv.Contains(searchString) || s.cijena.Equals(Decimal.Parse(searchString)));
                }
                switch (sortOrder)
                {
                    case "naziv_desc":
                        cijenaKarte = cijenaKarte.OrderByDescending(s => s.Dogadjaj.naziv);
                        break;
                    case "ime_desc":
                        cijenaKarte = cijenaKarte.OrderByDescending(s => s.VrstaKarte.imekategorije);
                        break;
                    case "cijena_desc":
                        cijenaKarte = cijenaKarte.OrderByDescending(s => s.cijena);
                        break;
                    default:  // Name ascending 
                        cijenaKarte = cijenaKarte.OrderBy(s => s.Dogadjaj.naziv);
                        break;
                }

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                return View(cijenaKarte.ToPagedList(pageNumber, pageSize));


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
