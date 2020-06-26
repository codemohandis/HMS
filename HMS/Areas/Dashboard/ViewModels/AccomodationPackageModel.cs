using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace HMS.Areas.Dashboard.ViewModel
{
    public class AccomodationPackageListingModel
    {
        public IEnumerable<AccomodationPackage> AccomodationPackage { get; set; }
        public IEnumerable<AccomodationType> AccomodationType { get; set; }
        public string SearchTerm { get; set; }
        public int? AccomdationTypeID { get; set; }
        public Pager Pager { get; set; }
    }
    public class AccomodationPackageActionModel
    {
        public int ID { get; set; }
        public int AccomodationTypeID { get; set; }
        public AccomodationType AccomodationType { get; set; }
        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
    }
}