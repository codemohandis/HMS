using HMS.Entities;
using HMS.Areas.Dashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModel
{
    public class AccomodationListingModel
    {
        public IEnumerable<Accomodation> Accomodation { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackage { get; set; }
        public string SearchTerm { get; set; }
        public int? AccomodationPackageID { get; set; }
        public Pager Pager { get; set; }
    }
    public class AccomodationActionModel
    {
        public int ID { get; set; }
        public int AccomodationPackageID { get; set; }
        public AccomodationPackage AccomodationPackage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
    }
}