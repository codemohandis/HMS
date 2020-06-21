using HMS.Areas.Dashboard.ViewModel;
using HMS.Entities;
using HMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationPackageController : Controller
    {
        AccomodationPackageService accomodationPackageService = new AccomodationPackageService();
        public ActionResult Index(string searchTerm)
        {
            AccomodationPackageListingModel model = new AccomodationPackageListingModel();
            model.SearchTerm = searchTerm;
            model.AccomodationPackage = accomodationPackageService.SearchAccomodationPackage(searchTerm);
            return View(model);
        }
        /// <summary>
        /// use Action For Create and Update Action
        /// </summary>
        /// <returns></returns>
        ///

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            AccomodationPackageActionModel model = new AccomodationPackageActionModel();

            if (ID.HasValue)//We are trying to edit a Record
            {
                var accomodationPackage = accomodationPackageService.GetAccomodationPackagesByID(ID.Value);
                model.ID = accomodationPackage.ID;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;
            }
            return PartialView("_Action", model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationPackageActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            var result = false;

            if (model.ID > 0)//Edit
            {
                var accomodationPackage = accomodationPackageService.GetAccomodationPackagesByID(model.ID);
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;
                result = accomodationPackageService.UpdateAccomodationPackage(accomodationPackage);

            }
            else//Add/Create
            {
                AccomodationPackage accomodationPackage = new AccomodationPackage();
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;
                result = accomodationPackageService.SaveAccomodationPackage(accomodationPackage);
            }

            if (result)
            {
                jsonResult.Data = new { Success = true };
            }
            else
            {
                jsonResult.Data = new { Success = false, Message = "Unable to Perform Action On Accomodation Type" };
            }
            return jsonResult;
        }

        [HttpGet]
        public ActionResult Delete(int ID)
        {
            AccomodationPackageActionModel model = new AccomodationPackageActionModel();
            var accomodationPackage = accomodationPackageService.GetAccomodationPackagesByID(ID);
            model.ID = accomodationPackage.ID;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public JsonResult Delete(AccomodationPackageActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            var result = false;
            var accomodationPackage = accomodationPackageService.GetAccomodationPackagesByID(model.ID);
            result = accomodationPackageService.DeleteAccomodationPackage(accomodationPackage);

            if (result)
            {
                jsonResult.Data = new { Success = true };
            }
            else
            {
                jsonResult.Data = new { Success = false, Message = "Unable to Perform Action On Accomodation Package" };
            }
            return jsonResult;
        }
    }
}