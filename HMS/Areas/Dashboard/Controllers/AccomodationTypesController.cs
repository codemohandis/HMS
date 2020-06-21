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
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// use Action For Create and Update Action
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        public ActionResult Action()
        {
            AccomodationTypesActonModel model = new AccomodationTypesActonModel();
            return PartialView("_Action", model);
        }
        [HttpPost]
        public JsonResult Action(AccomodationTypesActonModel model)
        {
            JsonResult jsonResult =new JsonResult();
            AccomodationType accomodationType = new AccomodationType();
            accomodationType.Name = model.Name;
            accomodationType.Description = model.Description;
            var result =  accomodationTypeService.SaveAccomodationType(accomodationType);
            if (result)
            {
                jsonResult.Data = new { Success = true };
            }
            else
            {
                jsonResult.Data = new { Success = false, Message= "Unable to add Accomodation Type" };
            }
            return jsonResult;
        }
        public ActionResult Listing()
        {
            AccomodationTypesListingModel modal = new AccomodationTypesListingModel();
            modal.AccomodationType = accomodationTypeService.GetAllAccomodationTypes();
            return PartialView("_Listing", modal);
        }
    }
}