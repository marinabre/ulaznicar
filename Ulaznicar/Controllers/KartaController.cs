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

namespace Ulaznicar.Controllers
{
    public class KartaController : Controller
    {
        bazaUlazniceEntities context = new bazaUlazniceEntities();
        // GET: Karta
        public ActionResult Index()
        {
            return View();
        }

        // GET: Karta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Karta/Create
        public ActionResult Create(int id)
        {
            var dogadjaj = context.Dogadjaj.Find(id);
            var brojevnostanje = (context.Karta.Where(x => x.IdDogadjaj == id)).Max(x => x.brojkarte);
            if (dogadjaj.brojmjesta - brojevnostanje > 0)
            {
                //QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                //QrCode qrCode = qrEncoder.Encode("stvari ke idu unutra");

                //Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);
                //renderer.CreateImageFile(qrCode.Matrix, @"c:\temp\HelloWorld.png", ImageFormat.Png);
            }



            return View();
        }

        // POST: Karta/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Karta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Karta/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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
