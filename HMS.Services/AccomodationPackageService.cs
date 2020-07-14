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
        public IEnumerable<AccomodationPackage> GetAllAccomodationPackagesByAccomodationType(int accomodationTypeID)
        {
            var context = new HMSContext();
            return context.AccomodationPackage.Where(x => x.AccomodationTypeID == accomodationTypeID).ToList();

        }
        public AccomodationPackage GetAccomodationPackagesByID(int ID)
        {
            var context = new HMSContext();
            return context.AccomodationPackage.Find(ID);
        }
        public IEnumerable<AccomodationPackage> SearchAccomodationPackage(string searchTerm, int? AccomdationTypeID,int page,int recordSize)
        {
            var context = new HMSContext();
            var accomodationPackage = context.AccomodationPackage.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackage = accomodationPackage.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (AccomdationTypeID.HasValue && AccomdationTypeID.Value > 0)
            {
                accomodationPackage = accomodationPackage.Where(x => x.AccomodationTypeID == AccomdationTypeID);
            }
            //skip = 1-1 * 3 = 0
            //skip = 2-1 * 3 = 3
            //skip = 3-1 * 3 = 6
            var skip = (page - 1) * recordSize;
            return accomodationPackage.OrderBy(x=>x.AccomodationTypeID).Skip(skip).Take(recordSize).ToList();
        }
        public int SearchAccomodationPackageCount(string searchTerm, int? AccomdationTypeID)
        {
            var context = new HMSContext();
            var accomodationPackage = context.AccomodationPackage.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackage = accomodationPackage.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (AccomdationTypeID.HasValue && AccomdationTypeID.Value > 0)
            {
                accomodationPackage = accomodationPackage.Where(x => x.AccomodationTypeID == AccomdationTypeID);
            }
               return accomodationPackage.Count();
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
            //iskay krny Ki waaja ye hai ki context dispose horha tha q ki ham accomdationpkgid ko 2 jaga use kr rhy thy yani
            //2 context mai to vo dusry ma akar dispose horha tha iswaja sy ye existingAccomodationPackage ka scene kia hai
            //isky zareye phly accomodation find ki phr id sy phr update krdya mgr jo List hain unhen nahe
            var existingAccomodationPackage = context.AccomodationPackage.Find(accomodationPackage.ID);

            context.AccomodationPackagePictures.RemoveRange(existingAccomodationPackage.AccomodationPackagePictures);

            context.Entry(existingAccomodationPackage).CurrentValues.SetValues(accomodationPackage);

            context.AccomodationPackagePictures.AddRange(accomodationPackage.AccomodationPackagePictures);

            return context.SaveChanges() > 0;
        }
        public bool DeleteAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
        public List<AccomodationPackagePictures> GetPicturesByAccomodationPackageID(int accomodationPackageID)
        {
            var context = new HMSContext();
            return context.AccomodationPackage.Find(accomodationPackageID).AccomodationPackagePictures.ToList();
        }
    }
}
