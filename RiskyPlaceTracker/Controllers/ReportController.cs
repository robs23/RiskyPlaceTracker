using RiskyPlaceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiskyPlaceTracker.Controllers
{
    public class ReportController : Controller
    {
        private ReportEntities db = new ReportEntities();

        // GET: Place
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetReports()
        {
            var reports = db.RD_Reports;
            return View(reports);
        }

        //POST
        [HttpGet]
        public ActionResult AddReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddReport(RD_Reports report)
        {
            if (!ModelState.IsValid)
            {
                return View("AddReport", report);
            }
            else
            {
                return RedirectToAction("GetReports");
            }
            
        }
    }
}