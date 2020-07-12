using HMS.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadPictures()
        {
            var files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var picture = files[i];
                var filePath = Guid.NewGuid() + Path.GetExtension(picture.FileName);
                var fileName = Server.MapPath("~/Images/site/")+ filePath;
                picture.SaveAs(fileName);
            }
            return View();
        }

    }
}