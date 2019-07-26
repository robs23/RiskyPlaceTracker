using RiskyPlaceTracker.Models;
using RiskyPlaceTracker.Static;
using RiskyPlaceTracker.ViewModels;
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
            List<User> users = new List<User>();
            users = GetUsers();
            AddReportViewModel vm = new AddReportViewModel();
            vm.Users = users;
            vm.ThisReport = new RD_Reports();
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddReport(AddReportViewModel vm, HttpPostedFileBase file)
        {
            List<User> users = new List<User>();
            users = GetUsers();
            vm.Users = users;
            if (!ModelState.IsValid)
            {
                
                vm.ThisReport = new RD_Reports();
                return View("AddReport", vm);
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
                    vm.ThisReport.Photo = fileName;
                    ProduceThumbnail(path);
                }
                vm.ThisReport.AddedByName = vm.Users.Where(u => u.UserId == vm.ThisReport.AddedBy).FirstOrDefault().FullName;
                vm.ThisReport.AddedOn = DateTime.Now;
                db.RD_Reports.Add(vm.ThisReport);
                db.SaveChanges();
                return RedirectToAction("GetReports");
            }
            
        }

        public void ProduceThumbnail(string path)
        {
            string fileName = Path.GetFileName(path);
            using (Image image = Image.FromFile(path))
            {
                Image thumb = Functions.ResizeImage(image, new Size(120, 120), true);
                Bitmap nBitmap = new Bitmap(thumb);
                thumb.Dispose();
                thumb = null;
                nBitmap.Save(Path.Combine(Static.Secrets.Path2Files + "\\Thumbnails",fileName));
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
        public FileResult GetThumb(string Photo)
        {
            var path = Path.Combine(Static.Secrets.Path2Thumbs, Photo);
            var theFile = new FileInfo(path);
            if (theFile.Exists)
            {
                return File(System.IO.File.ReadAllBytes(path), "image/png");
            }
            return null;
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
                    System.IO.File.Delete(Path.Combine(Static.Secrets.Path2Thumbs, oldReport.Photo));
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

        public List<User> GetUsers()
        {
            List<User> ret = null;
            string conStr = Secrets.MoodleConnectionString; //ConfigurationManager.ConnectionStrings["Moodle"].ConnectionString;
            var con = new MySql.Data.MySqlClient.MySqlConnection(conStr);

            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }

            string str = string.Format("SELECT us.id, us.firstname, us.lastname FROM mdl_user us ORDER BY us.lastname ASC");

            var command = new MySql.Data.MySqlClient.MySqlCommand(str, con);

            var reader = command.ExecuteReader();

            User u = new User();

            if (reader.HasRows)
            {
                ret = new List<User>();

                while (reader.Read())
                {
                    u = new User();
                    u.UserId = Convert.ToInt32(reader[reader.GetOrdinal("id")].ToString());
                    u.FullName = reader[reader.GetOrdinal("firstname")].ToString() + " " + reader[reader.GetOrdinal("lastname")].ToString();
                    ret.Add(u);
                }
            }

            return ret;
        }


    }
}