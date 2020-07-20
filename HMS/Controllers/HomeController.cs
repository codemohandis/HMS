using HMS.Services;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            AccomodationTypeService accomodationTypeService = new AccomodationTypeService();
            AccomodationPackageService accomodationPackageService = new AccomodationPackageService();

            model.AccommodationTypes = accomodationTypeService.GetAllAccomodationTypes();
            model.AccomodationPackages = accomodationPackageService.GetAllAccomodationPackages();

            return View(model);
        }
    }
}