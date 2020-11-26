using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    class ObjectsConfiguration
    {
        public class PatientDetailsConfiguration : IEntityTypeConfiguration<PatientDetails>
        {
            public void Configure(EntityTypeBuilder<PatientDetails> modelBuilder)
            {

                modelBuilder.ToTable("tbl_PatientDetails");
                modelBuilder.HasKey(p => p.UniqueID);
                modelBuilder.Property(p => p.UniqueID)
                            .ValueGeneratedNever()
                            .HasColumnType("nvarchar(20)")
                            .IsRequired();
                modelBuilder.Property(p => p.Name)
                            .HasColumnType("nvarchar(30)")
                            .IsRequired();
                modelBuilder.Property(p => p.Age)
                            .HasColumnType("tinyint")
                            .IsRequired();
                modelBuilder.Property(p => p.Sex)
                            .HasColumnType("nvarchar(20)")
                            .IsRequired();
                modelBuilder.Property(p => p.Phone)
                            .HasColumnType("nvarchar(15)")
                            .IsRequired();
            }
        }

        public class OccupationDetailsConfiguration : IEntityTypeConfiguration<OccupationDetails>
        {
            public void Configure(EntityTypeBuilder<OccupationDetails> modelBuilder)
            {
                modelBuilder.ToTable("tbl_OccupationDetails");
                modelBuilder.HasKey(o => o.OccupationID);
                modelBuilder.Property(o => o.OccupationID)
                            .ValueGeneratedOnAdd();
                modelBuilder.Property(o => o.OccupationType)
                            .HasColumnType("nvarchar(30)")
                            .IsRequired();
                modelBuilder.Property(o => o.CompanyName)
                            .HasColumnType("nvarchar(30)")
                            .IsRequired();
                modelBuilder.Property(o => o.Phone)
                            .HasColumnType("nvarchar(15)")
                            .IsRequired();

                modelBuilder.HasOne(o => o.Patient)
                            .WithOne(p => p.OccupationDetails)
                            .HasForeignKey<OccupationDetails>(f => f.UniqueID)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class TreatmentDetailsConfiguration : IEntityTypeConfiguration<TreatmentDetails>
        {
            public void Configure(EntityTypeBuilder<TreatmentDetails> modelBuilder)
            {
                modelBuilder.ToTable("tbl_TreatmentDetails");
                modelBuilder.HasKey(t => t.TreatmentID);
                modelBuilder.Property(t => t.TreatmentID)
                            .ValueGeneratedOnAdd();
                modelBuilder.Property(t => t.DiseaseName)
                            .HasColumnType("nvarchar(30)")
                            .IsRequired();
                modelBuilder.Property(t => t.AdmitDate)
                            .HasColumnType("Date")
                            .IsRequired();
                modelBuilder.Property(t => t.DischargeDate)
                            .HasColumnType("Date");
                modelBuilder.Property(t => t.Prescription)
                            .HasColumnType("nvarchar(100)");
                modelBuilder.Property(t => t.CurrentStatus)
                            .HasColumnType("nvarchar(30)")
                            .IsRequired();
                modelBuilder.Property(t => t.IsFatality)
                            .HasColumnType("nvarchar(10)")
                            .IsRequired();

                modelBuilder.HasOne(t => t.Patient)
                            .WithMany(p => p.TreatmentDetails)
                            .HasForeignKey(f => f.UniqueID)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Cascade);
                modelBuilder.HasOne(t => t.DiseaseType)
                            .WithMany(d => d.TreatmentDetails)
                            .HasForeignKey(f => f.DiseaseTypeID)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);
                modelBuilder.HasOne(t => t.Hospital)
                            .WithMany(h => h.TreatmentDetails)
                            .HasForeignKey(f => f.HospitalID)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class HospitalDetailsConfiguration : IEntityTypeConfiguration<HospitalDetails>
        {
            public void Configure(EntityTypeBuilder<HospitalDetails> modelBuilder)
            {
                modelBuilder.ToTable("tbl_HospitalDetails");
                modelBuilder.HasKey(h => h.HospitalID);
                modelBuilder.Property(h => h.HospitalID)
                            .ValueGeneratedOnAdd();
                modelBuilder.Property(h => h.Name)
                            .HasColumnType("nvarchar(30)")
                            .IsRequired();
                modelBuilder.Property(h => h.Phone)
                            .HasColumnType("nvarchar(15)")
                            .IsRequired();
            }
        }

        public class DiseaseTypesConfiguration : IEntityTypeConfiguration<DiseaseTypes>
        {
            public void Configure(EntityTypeBuilder<DiseaseTypes> modelBuilder)
            {
                modelBuilder.ToTable("tbl_DiseaseTypes");
                modelBuilder.HasKey(d => d.DiseaseTypeID);
                modelBuilder.HasIndex(d => d.DiseaseType).IsUnique();
                modelBuilder.Property(d => d.DiseaseTypeID)
                            .ValueGeneratedOnAdd();
                modelBuilder.Property(d => d.DiseaseType)
                            .HasColumnType("nvarchar(20)")
                            .IsRequired();
            }
        }

        public class StateNamesConfiguration : IEntityTypeConfiguration<StateNames>
        {
            public void Configure(EntityTypeBuilder<StateNames> modelBuilder)
            {
                modelBuilder.ToTable("tbl_StateNames");
                modelBuilder.HasKey(s => s.StateID);
                modelBuilder.HasIndex(s => s.State).IsUnique();
                modelBuilder.Property(s => s.StateID)
                            .ValueGeneratedOnAdd();
                modelBuilder.Property(s => s.State)
                            .HasColumnType("nvarchar(20)")
                            .IsRequired();
            }
        }

        public class AddressConfiguration : IEntityTypeConfiguration<Address>
        {
            public void Configure(EntityTypeBuilder<Address> modelBuilder)
            {
                modelBuilder.ToTable("tbl_Address");
                modelBuilder.HasKey(a => a.ID);
                modelBuilder.Property(a => a.ID)
                            .ValueGeneratedOnAdd();
                modelBuilder.Property(a => a.AddressType)
                            .HasColumnType("nvarchar(20)")
                            .IsRequired();
                modelBuilder.Property(a => a.Addressline)
                            .HasColumnType("nvarchar(50)")
                            .IsRequired();
                modelBuilder.Property(a => a.City)
                            .HasColumnType("nvarchar(20)")
                            .IsRequired();
                modelBuilder.Property(a => a.ZipCode)
                            .HasColumnType("nvarchar(10)")
                            .IsRequired();

                modelBuilder.HasOne(a => a.StateName)
                            .WithMany(s => s.Address)
                            .HasForeignKey(f => f.StateID)
                            .OnDelete(DeleteBehavior.Restrict);
                modelBuilder.HasOne(a => a.PatientAddress)
                            .WithMany(p => p.Address)
                            .HasForeignKey(f => f.UniqueID)
                            .OnDelete(DeleteBehavior.Cascade);
                modelBuilder.HasOne(a => a.OccupationAddress)
                            .WithOne(o => o.Address)
                            .HasForeignKey<Address>(f => f.OccupationID)
                            .OnDelete(DeleteBehavior.Cascade);
                modelBuilder.HasOne(a => a.HospitalAddress)
                            .WithOne(h => h.Address)
                            .HasForeignKey<Address>(f => f.HospitalID)
                            .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
