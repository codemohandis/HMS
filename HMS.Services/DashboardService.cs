using HMS.DataBase;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
   public class DashboardService
    {
        public bool SavePicture(Pictures picture)
        {
            using (var context = new HMSContext())
            {
                context.Picture.Add(picture);
                return context.SaveChanges() > 0;

            }
        }
        public IEnumerable<Pictures> GetPictureByIDs(List<int> pictureIDs)
        {
            using (var context = new HMSContext())
            {
                return pictureIDs.Select(x => context.Picture.Find(x)).ToList();
            }
        }

    }
}
