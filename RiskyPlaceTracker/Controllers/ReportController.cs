using RiskyPlaceTracker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        public ActionResult AddReport(RD_Reports report, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View("AddReport", report);
            }
            else
            {
                //save report
                //but first check if there are any photos


                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Static.Secrets.Path2Files, fileName);
                    file.SaveAs(path);
                    report.Photo = fileName;
                    ProduceThumbnail(path);
                }

                report.AddedOn = DateTime.Now;
                db.RD_Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("GetReports");
            }
            
        }

        public void ProduceThumbnail(string path)
        {
            var stream = new MemoryStream();
            var exteName = Path.GetExtension(path);

            using (Image image = Image.FromFile(path))
            {
                using (Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero))
                {
                    thumb.Save("thumbnail.png", ImageFormat.Png);
                }
            }
        }

        [HttpGet]
        public ActionResult GetPhoto(string Photo)
        {
            var path = Path.Combine(Static.Secrets.Path2Files, Photo);
            var theFile = new FileInfo(path);
            if (theFile.Exists)
            {
                return File(System.IO.File.ReadAllBytes(path), "image/png");
            }
            return this.HttpNotFound();
        }



        [HttpGet]
        public ActionResult DeleteReport(int id)
        {
            var reports = db.RD_Reports.Where(r => r.ReportId==id);
            if (reports.Any())
            {
                RD_Reports oldReport = reports.FirstOrDefault();
                if (oldReport.Photo != null)
                {
                    //delete image
                    System.IO.File.Delete(Path.Combine(Static.Secrets.Path2Files, oldReport.Photo));
                }
                db.RD_Reports.Remove(oldReport);
                db.SaveChanges();
                return RedirectToAction("GetReports");
            }
            else
            {
                return new HttpNotFoundResult("Nie znaleziono raportu");
            }
        }
    }
}