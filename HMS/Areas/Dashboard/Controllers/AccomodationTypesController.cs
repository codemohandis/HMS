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
    public class AccomodationTypesController : Controller
    {
        AccomodationTypeService accomodationTypeService = new AccomodationTypeService();
        public ActionResult Index(string searchTerm)
        {
            AccomodationTypesListingModel model = new AccomodationTypesListingModel();
            model.SearchTerm = searchTerm;
            model.AccomodationType = accomodationTypeService.SearchAccomodationType(searchTerm);
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
            AccomodationTypesActionModel model = new AccomodationTypesActionModel();

            if (ID.HasValue)//We are trying to edit a Record
            {
                var accomodationType = accomodationTypeService.GetAccomodationTypesByID(ID.Value);
                model.ID = accomodationType.ID;
                model.Name = accomodationType.Name;
                model.Description = accomodationType.Description;

            }
            return PartialView("_Action", model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationTypesActionModel model)
        {
            JsonResult jsonResult =new JsonResult();
            var result = false;

            if (model.ID > 0)//Edit
            {
                var accomodationType = accomodationTypeService.GetAccomodationTypesByID(model.ID);
                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;
                result = accomodationTypeService.UpdateAccomodationType(accomodationType);

            }
            else//Add/Create
            {
                AccomodationType accomodationType = new AccomodationType();
                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;
                result = accomodationTypeService.SaveAccomodationType(accomodationType);
            }

            if (result)
            {
                jsonResult.Data = new { Success = true };
            }
            else
            {
                jsonResult.Data = new { Success = false, Message= "Unable to Perform Action On Accomodation Type" };
            }
            return jsonResult;
        }

        [HttpGet]
        public ActionResult Delete(int ID)
        {
           AccomodationTypesActionModel model = new AccomodationTypesActionModel();
           var accomodationType = accomodationTypeService.GetAccomodationTypesByID(ID);
           model.ID = accomodationType.ID;
           return PartialView("_Delete", model);
        }
        [HttpPost]
        public JsonResult Delete(AccomodationTypesActionModel model)
        {
           JsonResult jsonResult = new JsonResult();
           var result = false;
           var accomodationType = accomodationTypeService.GetAccomodationTypesByID(model.ID);
           result = accomodationTypeService.DeleteAccomodationType(accomodationType);

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
    }
}