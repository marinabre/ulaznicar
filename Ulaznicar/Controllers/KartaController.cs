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
        public ActionResult Create(int id)
        {
            var userId = User.Identity.GetUserId();
            var userUserName = User.Identity.GetUserName();
            var idkorisnik = (context.Korisnik.Where(x => x.korisnickoime == userUserName).First()).Id;


            var dogadjaj = context.Dogadjaj.Find(id);
            var brojevnostanje = (context.Karta.Where(x => x.IdDogadjaj == id)).Max(x => x.brojkarte);
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
                    IdDogadjaj = id,
                    QR_KOD = kod,
                    brojkarte = brojevnostanje + 1,
                    IdVrstakarte = 1,
                    cijena = 20m
                };

                context.Karta.Add(novaKarta);
                context.SaveChanges();

                KupljeneKarte kup = new KupljeneKarte
                {
                    IdKarta = context.Karta.FirstOrDefault(k => k.brojkarte == brojevnostanje + 1).Id,
                    IdKorisnik = idkorisnik
                };

                context.KupljeneKarte.Add(kup);
                context.SaveChanges();

               // return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //// POST: Karta/Create
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
            ViewBag.IdKorisnik = new SelectList(context.Korisnik, "Id", "korisnickoime");
            return View(karta);
        }

        // POST: Karta/Edit/5
        [HttpPost]
        public ActionResult Pokloni(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
    }
}
