using HMS.Entities;
using System;
using System.Collections.Generic;
using HMS.DataBase;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationPackageService
    {
        public IEnumerable<AccomodationPackage> GetAllAccomodationPackages()
        {
            var context = new HMSContext();
            return context.AccomodationPackage.ToList();

        }
        public AccomodationPackage GetAccomodationPackagesByID(int ID)
        {
            using (var context = new HMSContext())
            {
                return context.AccomodationPackage.Find(ID);
            }
        }
        public IEnumerable<AccomodationPackage> SearchAccomodationPackage(string searchTerm)
        {
            var context = new HMSContext();
            var accomodationPackage = context.AccomodationPackage.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackage = accomodationPackage.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return accomodationPackage.ToList();
        }
        public bool SaveAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.AccomodationPackage.Add(accomodationPackage);
            return context.SaveChanges() > 0;
        }
        public bool UpdateAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
        public bool DeleteAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
