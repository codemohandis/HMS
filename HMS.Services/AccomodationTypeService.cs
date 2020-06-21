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
            return context.AccomodationType.ToList();

        }
        public AccomodationType GetAccomodationTypesByID(int ID)
        {
            var context = new HMSContext();
            return context.AccomodationType.Find(ID);

        }
        public IEnumerable<AccomodationType> SearchAccomodationType(string searchTerm)
        {
            var context = new HMSContext();
            var accomodationType = context.AccomodationType.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationType = accomodationType.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return accomodationType.ToList();
        }
        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.AccomodationType.Add(accomodationType);
            return context.SaveChanges() > 0;
        }
        public bool UpdateAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
        public bool DeleteAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
