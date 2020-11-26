
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using BusinessObjects;

namespace DataAccessLayer.DataContext
{
    public class TrackerDbContext : DbContext
    {

        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                opsBuilder = new DbContextOptionsBuilder<TrackerDbContext>();
                opsBuilder.UseSqlServer(settings.connectionstring);
                dbOptions = opsBuilder.Options;
            }
            public DbContextOptionsBuilder<TrackerDbContext> opsBuilder { get; set; }
            public DbContextOptions<TrackerDbContext> dbOptions { get; set; }
            private AppConfiguration settings { get; set; }
        }

        public static OptionsBuild ops = new OptionsBuild();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) { }

        public DbSet<PatientDetails> Patients { get; set; }

        public DbSet<StateNames> StateNames { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<OccupationDetails> OccupationDetails { get; set; }

        public DbSet<HospitalDetails> Hospitals { get; set; }

        public DbSet<TreatmentDetails> TreatmentDetails { get; set; }

        public DbSet<DiseaseTypes> DiseaseTypes { get; set; }



    }
}
