using HMS.Areas.Dashboard.ViewModel;
using HMS.Areas.Dashboard.ViewModels;
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
        AccomodationTypeService accomodationTypeService = new AccomodationTypeService();
        public ActionResult Index(string searchTerm,int? accomdationTypeID,int page = 1)
        {
            int recordSize = 3;
//            page = page ?? 1;
            AccomodationPackageListingModel model = new AccomodationPackageListingModel();
            model.SearchTerm = searchTerm;
            model.AccomdationTypeID = accomdationTypeID;
            model.AccomodationType = accomodationTypeService.GetAllAccomodationTypes();
            model.AccomodationPackage = accomodationPackageService.SearchAccomodationPackage(searchTerm, accomdationTypeID,page,recordSize);
            var totalRecord = accomodationPackageService.SearchAccomodationPackageCount(searchTerm, accomdationTypeID);
            model.Pager = new Pager(totalRecord, page,recordSize);
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
                model.AccomodationTypeID = accomodationPackage.AccomodationTypeID;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;
            }
            model.AccomodationTypes = accomodationTypeService.GetAllAccomodationTypes();
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
                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = accomodationPackageService.UpdateAccomodationPackage(accomodationPackage);

            }
            else//Add/Create
            {
                AccomodationPackage accomodationPackage = new AccomodationPackage();
                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
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