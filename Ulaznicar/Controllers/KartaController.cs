using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ulaznicar.Models;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;

namespace Ulaznicar.Controllers
{
    
    public class KartaController : Controller
    {
        bazaUlazniceEntities context = new bazaUlazniceEntities();
        // GET: Karta
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            var id = (context.Korisnik.Where(x => x.korisnickoime == userUserName).First()).Id;

            var kupljene = context.KupljeneKarte.Where(x => x.IdKorisnik == id);
            List<Karta> karte = new List<Karta>();

            foreach (var kup in kupljene)
            {
                karte.Add(context.Karta.Find(kup.IdKarta));
            }
            return View(karte);
        }

        // GET: Karta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Karta/Create
        public ActionResult Create(int id , int? idcijena)
        {
            var dogadjaj = context.Dogadjaj.Find(id);
            var brojevnostanje = (context.Karta.Where(x => x.IdDogadjaj == id)).Max(x => x.brojkarte);
            if (brojevnostanje == null)
            {
                brojevnostanje = 0;
            }

            if (idcijena == null)
            {
                var cijena = context.CijenaKarte.Where(x => x.IdDogadjaj == id);

                if (cijena.Count(x => x.IdDogadjaj == id) == 1)
                {
                    idcijena = cijena.First(x => x.IdDogadjaj == id).Id;
                }
                else
                {
                    ModelState.AddModelError("", "Niste odabrali koju vrstu ulaznice želite!");
                    return RedirectToAction("Details", "Dogadjaj", id);
                }
            }

            //provjera da nije vec stvorena karta, a da je netko odustao od kupovine
            var kupljene = context.KupljeneKarte;
            var karte = context.Karta.Where(x => x.IdDogadjaj == id);
            foreach (var kart in karte)
            {
                if (kupljene.FirstOrDefault(x => x.IdKarta == kart.Id) == null && kart.IdCijena == idcijena)
                {
                    return RedirectToAction("Kupovina", new { id = kart.Id });
                }
            }

            if (dogadjaj.brojmjesta - brojevnostanje > 0)
            {
                string zastitni = id.ToString() + " " + (brojevnostanje + 1).ToString();
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = qrEncoder.Encode(zastitni);

                Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);
                Stream memoryStream = new MemoryStream();
                renderer.WriteToStream(qrCode.Matrix, memoryStream, ImageFormat.Png);

                memoryStream.Position = 0;
                byte[] kod = ((MemoryStream)memoryStream).ToArray();

                Karta novaKarta = new Karta
                {
                    zastitnikod = zastitni,
                    IdDogadjaj = id,
                    QR_KOD = kod,
                    brojkarte = brojevnostanje + 1,
                    IdCijena = (int)idcijena
                };

                context.Karta.Add(novaKarta);
                context.SaveChanges();

                context = new bazaUlazniceEntities();

                var idkarte = context.Karta.Where(x => x.zastitnikod == zastitni);

                return RedirectToAction("Kupovina", new { id = idkarte });
            }

            return RedirectToAction("Details",id);
        }

        // POST: Karta/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Karta/Edit/5

        public ActionResult Pokloni(int id)
        {
            var karta = context.Karta.Find(id);
            ViewBag.odabran = karta.Dogadjaj;
            ViewBag.cijena = karta.CijenaKarte.VrstaKarte.imekategorije;
            ViewBag.IdKorisnik = new SelectList(context.Korisnik, "Id", "korisnickoime");
            var kupljena = context.KupljeneKarte.First(x => x.IdKarta == id);
            return View(kupljena);
        }

        // POST: Karta/Edit/5
        [HttpPost]
        public ActionResult Pokloni([Bind(Include = "Id,IdKorisnik,IdKarta")] KupljeneKarte kupljenakarta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Entry(kupljenakarta).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Kupovina(int id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            
            var karta = context.Karta.Find(id);

            ViewBag.naziv = karta.Dogadjaj.naziv;
            ViewBag.datum = karta.Dogadjaj.datum;
            ViewBag.mjesto = karta.Dogadjaj.Lokacija.naziv;
            ViewBag.adresa = karta.Dogadjaj.Lokacija.adresa;
            ViewBag.cijena = karta.CijenaKarte.cijena;
            ViewBag.korisnik = userUserName;

            return View(karta);
        }

        public ActionResult Kupovina_konacna(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userUserName = User.Identity.GetUserName();
                var korisnik = (context.Korisnik.Where(x => x.korisnickoime == userUserName).First()).Id;

                KupljeneKarte karta = new KupljeneKarte
                {
                    IdKarta = id,
                    IdKorisnik = korisnik
                };

                context.KupljeneKarte.Add(karta);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Kupovina");
            }
        }



        // GET: Karta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Karta/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Show(byte[] image)
        {
            return new FileContentResult(image, "img/gif");
        }

    }
}
