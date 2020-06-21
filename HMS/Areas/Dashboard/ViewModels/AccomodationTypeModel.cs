using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModel
{
    public class AccomodationTypesListingModel
    {
        public IEnumerable<AccomodationType> AccomodationType { get; set; }
    }
    public class AccomodationTypesActonModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}