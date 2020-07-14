using HMS.Entities;
using HMS.Services;
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
        public JsonResult UploadPictures()
        {
            JsonResult jsonResult = new JsonResult();
            var pictureList = new List<Pictures>();
            DashboardService dashboardService = new DashboardService();
            var files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var picture = files[i];
                var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);
                var filePath = Server.MapPath("~/Images/site/")+ fileName;
                picture.SaveAs(filePath);
                var dbPicture = new Pictures();
                dbPicture.URL = fileName;
                if (dashboardService.SavePicture(dbPicture))
                {
                    pictureList.Add(dbPicture);
                }
            }
            jsonResult.Data = pictureList;
            return jsonResult;
        }

    }
}