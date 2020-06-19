using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataBase
{
    public class HMSContext : DbContext
    {
        public HMSContext() :base("HMSConnectionString")
        {

        }

        public DbSet<AccomodationType> AccomodationType { get; set; }
        public DbSet<AccomodationPackage> AccomodationPackage { get; set; }
        public DbSet<Accomodation> Accomodation { get; set; }
        public DbSet<Booking> Booking { get; set; }

    }
}
