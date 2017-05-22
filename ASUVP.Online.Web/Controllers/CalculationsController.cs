using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    public class CalculationsController : Controller
    {
        // GET: Calculations
        public ActionResult Index()
        {
            ViewBag.Title = "Предварительные расчеты";
            return View();
        }

        // GET: Calculations/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Calculations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Calculations/Create
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

        // GET: Calculations/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Calculations/Edit/5
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

        // GET: Calculations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Calculations/Delete/5
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
