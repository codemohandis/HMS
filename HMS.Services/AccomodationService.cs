using HMS.DataBase;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationService
    {

        public IEnumerable<Accomodation> GetAllAccomodations()
        {
            var context = new HMSContext();
            return context.Accomodation.ToList();

        }
        public IEnumerable<Accomodation> GetAllAccomodationsByAccomodationPackage(int accomdationPackageID)
        {
            var context = new HMSContext();
            return context.Accomodation.Where(x=>x.AccomodationPackageID == accomdationPackageID).ToList();

        }
        public Accomodation GetAccomodationsByID(int ID)
        {
            var context = new HMSContext();
               return context.Accomodation.Find(ID);
        }
        public IEnumerable<Accomodation> SearchAccomodation(string searchTerm, int? AccomdationPackageID, int page, int recordSize)
        {
            var context = new HMSContext();
            var accomodation = context.Accomodation.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodation = accomodation.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (AccomdationPackageID.HasValue && AccomdationPackageID.Value > 0)
            {
                accomodation = accomodation.Where(x => x.AccomodationPackageID == AccomdationPackageID);
            }
            //skip = 1-1 * 3 = 0
            //skip = 2-1 * 3 = 3
            //skip = 3-1 * 3 = 6
            var skip = (page - 1) * recordSize;
            return accomodation.OrderBy(x => x.AccomodationPackageID).Skip(skip).Take(recordSize).ToList();
        }

        public int AccomodationCount(string searchTerm, int? AccomdationTypeID)
        {
            var context = new HMSContext();
            var accomodation = context.Accomodation.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodation = accomodation.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (AccomdationTypeID.HasValue && AccomdationTypeID.Value > 0)
            {
                accomodation = accomodation.Where(x => x.AccomodationPackageID == AccomdationTypeID);
            }
            return accomodation.Count();
        }
        public bool SaveAccomodation(Accomodation accomodation)
        {
            var context = new HMSContext();
            context.Accomodation.Add(accomodation);
            return context.SaveChanges() > 0;
        }
        public bool UpdateAccomodation(Accomodation accomodation)
        {
            var context = new HMSContext();
            //iskay krny Ki waaja ye hai ki context dispose horha tha q ki ham accomdationpkgid ko 2 jaga use kr rhy thy yani
            //2 context mai to vo dusry ma akar dispose horha tha iswaja sy ye existingAccomodationPackage ka scene kia hai
            //isky zareye phly accomodation find ki phr id sy phr update krdya mgr jo List hain unhen nahe
            var existingAccomodation = context.Accomodation.Find(accomodation.ID);

            context.AccomodationPictures.RemoveRange(existingAccomodation.AccomodationPictures);

            context.Entry(existingAccomodation).CurrentValues.SetValues(accomodation);

            context.AccomodationPictures.AddRange(accomodation.AccomodationPictures);

            return context.SaveChanges() > 0;
        }
        public bool DeleteAccomodation(Accomodation accomodation)
        {
            var context = new HMSContext();
            context.Entry(accomodation).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
        public List<AccomodationPictures> GetPicturesByAccomodationID(int accomodationID)
        {
            var context = new HMSContext();
            return context.Accomodation.Find(accomodationID).AccomodationPictures.ToList();
        }
    }
}
