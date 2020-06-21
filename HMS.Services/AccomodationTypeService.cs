using HMS.DataBase;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
   public class AccomodationTypeService
    {
        public IEnumerable<AccomodationType> GetAllAccomodationTypes()
        {
            var context = new HMSContext();
            return context.AccomodationType.AsEnumerable();

        }
        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.AccomodationType.Add(accomodationType);
            return context.SaveChanges() > 0;
        }
    }
}
