using BusinessLogicLayer.Interfaces;
using BusinessObjects;
using DataAccessLayer.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Functions
{
    public class HospitalFunctions :  IHospital
    {
        private IAddress _addresslogic = new AddressFunctions();

        private readonly TrackerDbContext _context = new TrackerDbContext(TrackerDbContext.ops.dbOptions);

        public PageList<HospitalDetails> GetHospitals(int page = 1)
        {
            var hospitals = PageList<HospitalDetails>.ToPagedList( _context.Hospitals
                                                    .Include(h => h.Address)
                                                        .ThenInclude(a => a.StateName)
                                                        .OrderBy(h => h.Name), page);
            return hospitals;
        }

        public async Task<HospitalDetails> GetHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            _context.Entry(hospital)
                .Reference(h => h.Address)
                .Query()
                .Include(Address => Address.StateName)
                .Load();
            return hospital;
        }

        public async Task<HospitalDetails> AddHospital(HospitalDetails hospital)
        {
            if (hospital.Address == null)
                throw new Exception("Hospital Address cannot be null. Try adding address");
            var address = hospital.Address;
            hospital.Address = null;
            await _context.Hospitals.AddAsync(hospital);
            await _context.SaveChangesAsync();
            address.OccupationID = null;
            address.AddressType = "Hospital";
            address.HospitalID = hospital.HospitalID;
            await _addresslogic.AddAddress(address);
            return hospital;
        }

        public async Task<HospitalDetails> UpdateHospital(int id, HospitalDetails hospital)
        {
            HospitalDetails hosobj;
            int addid;
            using (var _datacontext = new TrackerDbContext(TrackerDbContext.ops.dbOptions))
            {
                hosobj = await _datacontext.Hospitals.FindAsync(id);
                _datacontext.Entry(hosobj)
                    .Reference(h => h.Address)
                    .Query()
                    .Include(Address => Address.StateName)
                    .Load();
                addid = hosobj.Address.ID;
            }

            if (hospital.Address == null)
                throw new Exception("Hospital Address cannot be null. Try adding Hospital Address.");
            var addobj = hospital.Address;
            hospital.Address = null;
            _context.Entry(hospital).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //addobj.ID = addid;
            await _addresslogic.UpdateAddress(addid, addobj);
            return hospital;
        }

        public async Task<HospitalDetails> DeleteHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
            return hospital;
        }

        public  PageList<PatientDetails> GetPatients(int id,int page)
        {
            var patients = PageList<PatientDetails>.ToPagedList( _context.Patients
                                                    .Include(p => p.TreatmentDetails)
                                                    .Where(p => p.TreatmentDetails
                                                                 .Any(t => t.HospitalID == id))
                                                    .OrderBy(p => p.Name), page);
            return patients;
        }

        public bool HospitalDetailsExists(int id)
        {
            return _context.Hospitals.Any(e => e.HospitalID == id);
        }
    }
}
