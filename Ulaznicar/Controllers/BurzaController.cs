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
using System.Data.Entity.Infrastructure;
using Ulaznicar.Models;

namespace Ulaznicar.Controllers
{
    public class BurzaController : Controller
    {
        private bazaUlazniceEntities db = new bazaUlazniceEntities();

        // GET: Burza
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var burza = db.Burza.Include(b => b.Korisnik).Where(b=>b.IdKupac == null);

            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            var korisnik = db.Korisnik.First(k => k.korisnickoime == userUserName);

            //provjeravamo ima li korisnik neku svoju kartu već na burzi, te mu želimo omogućiti da ju može maknuti
            var kupljene = db.KupljeneKarte.Where(x => x.IdKorisnik == korisnik.Id).Include(b=>b.Karta);
            List<int> karte = new List<int>();

            foreach (var kup in kupljene)
            {
                if (burza.FirstOrDefault(x => x.IdKarta == kup.IdKarta) != null)
                {
                    karte.Add(kup.IdKarta);
                }
            }
            ViewBag.vlastite = karte;
            ViewBag.user = userUserName;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.DatumSortParm = sortOrder == "Date" ? "date_desc" : "Date";
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


            if (!String.IsNullOrEmpty(searchString))
            {
                burza = burza.Where(s => s.KupljeneKarte.Karta.Dogadjaj.naziv.Contains(searchString)
                                       || s.cijena.Equals(Decimal.Parse(searchString)));
            }
            switch (sortOrder)
            {
                case "naziv_desc":
                    burza = burza.OrderByDescending(s => s.KupljeneKarte.Karta.Dogadjaj.naziv);
                    break;
                case "Date":
                    burza = burza.OrderByDescending(s => s.datumponude);
                    break;
                case "date_desc":
                    burza = burza.OrderByDescending(s => s.datumponude);
                    break;
                case "cijena_desc":
                    burza = burza.OrderByDescending(s => s.cijena);
                    break;
                default:  // Name ascending 
                    burza = burza.OrderBy(s => s.datumponude);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(burza.ToPagedList(pageNumber, pageSize));

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
        public ActionResult Create(int? id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            var korisnik = db.Korisnik.First(k => k.korisnickoime == userUserName);
            var postojeci = db.Burza;

            if (id != null)
            {
                var dogadjaj = db.Dogadjaj.Find(id);
                ViewBag.dogadjaj = dogadjaj;
                ViewBag.ime = id;
                //ViewData["karta"] = dogadjaj.Id;
            }
            else
            {
                var kupljenekarte = db.KupljeneKarte.Where(kar => kar.IdKorisnik == korisnik.Id).Include(x => x.Karta);
                List<Dogadjaj> dogadjaji = new List<Dogadjaj>();

                foreach (var karta in kupljenekarte)
                {
                    //provjera je li karta stara i je li već postavljena na burzu
                    var provjera = postojeci.FirstOrDefault(x=>x.IdKarta == karta.Id);
                    if (karta.Karta.Dogadjaj.datum >= DateTime.Today && (provjera == null ||provjera.IdKupac != null))
                    {
                        dogadjaji.Add(karta.Karta.Dogadjaj);
                    }
                }

                ViewBag.IdKarta = new SelectList(dogadjaji, "Id", "naziv");
            }
            return View();
        }

        // POST: Burza/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdKupac,IdKarta,datumponude,datumprodaje,cijena")] Burza burza)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            var korisnik = db.Korisnik.AsNoTracking().First(k => k.korisnickoime == userUserName).Id;
            var Idd = burza.IdKarta;

            var idkarte = db.KupljeneKarte.AsNoTracking().Where(
                x => x.IdKorisnik == korisnik).Include(x => x.Karta).FirstOrDefault(
                x => x.Karta.Dogadjaj.Id == Idd
                ).Id;


            burza.IdKarta = idkarte;
            burza.datumponude = DateTime.Now;
            if (burza.Id != 0)
            {
                burza.Id = 0;
            }

            try
            {
                db.Burza.Add(burza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                var kupljenekarte = db.KupljeneKarte.Where(kar => kar.IdKorisnik == korisnik).Include(x => x.Karta);
                List<Dogadjaj> dogadjaji = new List<Dogadjaj>();

                foreach (var karta in kupljenekarte)
                {
                    dogadjaji.Add(karta.Karta.Dogadjaj);
                }

                ViewBag.IdKarta = new SelectList(dogadjaji, "Id", "naziv", Idd);
                return View(burza);
            }
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

        public ActionResult Kupovina(int id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();

            var burzaunos = db.Burza.Find(id);
            var karta = burzaunos.KupljeneKarte.Karta;

            ViewBag.naziv = karta.Dogadjaj.naziv;
            ViewBag.datum = karta.Dogadjaj.datum;
            ViewBag.mjesto = karta.Dogadjaj.Lokacija.naziv;
            ViewBag.adresa = karta.Dogadjaj.Lokacija.adresa;
            ViewBag.cijena = burzaunos.cijena;
            ViewBag.korisnik = userUserName;

            return View(burzaunos);
        }

        public ActionResult Kupovina_konacna(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userUserName = User.Identity.GetUserName();
                var korisnik = (db.Korisnik.Where(x => x.korisnickoime == userUserName).First()).Id;

                var burza = db.Burza.Find(id);
                burza.IdKupac = korisnik;
                burza.datumprodaje = DateTime.Now;

                var kupljena = db.KupljeneKarte.Find(burza.IdKarta);
                kupljena.IdKorisnik = korisnik;

                db.Entry(burza).State = EntityState.Modified;
                db.Entry(kupljena).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Kupovina", new { id = id });
            }
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
