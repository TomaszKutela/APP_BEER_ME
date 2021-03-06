﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APP_BEER_ME.DAL;
using APP_BEER_ME.Models;

namespace APP_BEER_ME.Controllers
{
    public class StockController : Controller
    {
        private APP_BEER_MEContext db = new APP_BEER_MEContext();

        public double CalcUnits(double ABV, int Volume)
        {
            double Units = 0;
            Units = Volume * (ABV / 100);
            return Units;
        }

        public double CalcPricePerUnit(double ABV, int Volume, double Price)
        {
            double PricePerUnit = 0;
            PricePerUnit = Price / (Volume * (ABV / 100));
            return PricePerUnit;
        }

        // GET: Stock
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "shopname_desc" : "";

            var stocks = db.Stocks.Include(s => s.Beer).Include(s => s.Shop);

            switch (sortOrder)
            {
                default:
                    stocks = stocks.OrderBy(s => s.Shop.ShopName);
                    break;
            }

            return View(stocks.ToList());
        }

        // GET: Stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name");
            ViewBag.ShopID = new SelectList(db.Shops, "ShopID", "ShopName");
            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockID,BeerID,ShopID,Price")] Stock stock)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Stocks.Add(stock);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name", stock.BeerID);
            ViewBag.ShopID = new SelectList(db.Shops, "ShopID", "ShopName", stock.ShopID);
            return View(stock);
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name", stock.BeerID);
            ViewBag.ShopID = new SelectList(db.Shops, "ShopID", "ShopName", stock.ShopID);
            return View(stock);
        }

        // POST: Stock/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockID,BeerID,ShopID,Price")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name", stock.BeerID);
            ViewBag.ShopID = new SelectList(db.Shops, "ShopID", "ShopName", stock.ShopID);
            return View(stock);
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }


        // POST: Stock/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Stock stock = db.Stocks.Find(id);
                db.Stocks.Remove(stock);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
