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
        public string SearchTerm { get; set; }
    }
    public class AccomodationPackageActionModel
    {
        public int ID { get; set; }
        public int AccomodationTypeID { get; set; }
        public AccomodationType AccomodationType { get; set; }
        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }
    }
}