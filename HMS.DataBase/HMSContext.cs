using HMS.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataBase
{
    public class HMSContext : IdentityDbContext<HMSUser>
    {
        public HMSContext() :base("HMSConnectionString")
        {

        }
        public static HMSContext Create()
        {
            return new HMSContext();
        }

        public DbSet<AccomodationType> AccomodationType { get; set; }
        public DbSet<AccomodationPackage> AccomodationPackage { get; set; }
        public DbSet<AccomodationPackagePictures> AccomodationPackagePictures { get; set; }
        public DbSet<Accomodation> Accomodation { get; set; }
        public DbSet<AccomodationPictures> AccomodationPictures { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Pictures> Picture { get; set; }
    }
}
