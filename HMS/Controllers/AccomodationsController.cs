using HMS.Services;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Controllers
{
    public class AccomodationsController : Controller
    {
        AccomodationTypeService accomodationTypeService = new AccomodationTypeService();
        AccomodationPackageService accomodationPackageService = new AccomodationPackageService();
        AccomodationService accomodationService = new AccomodationService();
        // GET: Accomodations
        public ActionResult Index(int accomodationTypeID, int? accomodationPackageID)
        {
            AccomodationViewModels model = new AccomodationViewModels();
            model.AccomodationType = accomodationTypeService.GetAccomodationTypesByID(accomodationTypeID);
            model.AccomodationPackages = accomodationPackageService.GetAllAccomodationPackagesByAccomodationType(accomodationTypeID);
            model.SelectedAccomodationPackageID = accomodationPackageID.HasValue ? accomodationPackageID.Value : model.AccomodationPackages.First().ID;
            model.Accomodations = accomodationService.GetAllAccomodationsByAccomodationPackage(model.SelectedAccomodationPackageID);
            return View(model);
        }
    }
}