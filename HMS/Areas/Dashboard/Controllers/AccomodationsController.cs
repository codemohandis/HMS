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
    public class AccomodationsController : Controller
    {
        AccomodationService accomodationService = new AccomodationService();
        AccomodationPackageService accomodationPackageService = new AccomodationPackageService();

        public ActionResult Index(string searchTerm, int? accomodationPackageID, int page = 1)
        {
            int recordSize = 3;
            //            page = page ?? 1;
            AccomodationListingModel model = new AccomodationListingModel();
            model.SearchTerm = searchTerm;
            model.AccomodationPackageID = accomodationPackageID;
            model.AccomodationPackage = accomodationPackageService.GetAllAccomodationPackages();
            model.Accomodation = accomodationService.SearchAccomodation(searchTerm, accomodationPackageID, page, recordSize);
            var totalRecord = accomodationService.SearchAccomodationCount(searchTerm, accomodationPackageID);
            model.Pager = new Pager(totalRecord, page, recordSize);
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
            AccomodationActionModel model = new AccomodationActionModel();

            if (ID.HasValue)//We are trying to edit a Record
            {
                var accomodation = accomodationService.GetAccomodationsByID(ID.Value);
                model.ID = accomodation.ID;
                model.AccomodationPackageID = accomodation.AccomodationPackageID;
                model.Name = accomodation.Name;
                model.Description = accomodation.Description;
            }
           model.AccomodationPackages = accomodationPackageService.GetAllAccomodationPackages();
            return PartialView("_Action", model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            var result = false;

            if (model.ID > 0)//Edit
            {
                var accomodation = accomodationService.GetAccomodationsByID(model.ID);
                accomodation.AccomodationPackageID = model.AccomodationPackageID;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;

                result = accomodationService.UpdateAccomodation(accomodation);

            }
            else//Add/Create
            {
                Accomodation accomodation = new Accomodation();
                accomodation.AccomodationPackageID = model.AccomodationPackageID;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;

                result = accomodationService.SaveAccomodation(accomodation);
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
            AccomodationActionModel model = new AccomodationActionModel();
            var accomodation = accomodationService.GetAccomodationsByID(ID);
            model.ID = accomodation.ID;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public JsonResult Delete(AccomodationActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            var result = false;
            var accomodation = accomodationService.GetAccomodationsByID(model.ID);
            result = accomodationService.DeleteAccomodation(accomodation);

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