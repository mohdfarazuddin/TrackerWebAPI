using DataAccessLayer.DataContext;
using BusinessObjects;
using BusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BusinessLogicLayer.Functions
{
    public class PatientFunctions : IPatient
    {

        private IAddress _addresslogic = new AddressFunctions();

        private IDiseaseType _diseasetypelogic = new DiseaseTypeFunctions();

        private IHospital _hospitallogic = new HospitalFunctions();

        private readonly TrackerDbContext _context = new TrackerDbContext(TrackerDbContext.ops.dbOptions);


        public  PageList<PatientDetails> GetPatients(int page)
        {
            var patients = PageList<PatientDetails>.ToPagedList(_context.Patients
                                                .Include(p => p.Address)
                                                   .ThenInclude(a => a.StateName)
                                                   .OrderBy(p => p.Name), page);

            return patients;
        }

        public async Task<PatientDetails> GetPatient(string patientid)
        {

            var patient = await _context.Patients.FindAsync(patientid);

            _context.Entry(patient)
                .Collection(p => p.Address)
                .Query()
                .Include(Address => Address.StateName)
                .Load();
            _context.Entry(patient)
                .Reference(p => p.OccupationDetails)
                .Query()
                .Include(OccupationDetails => OccupationDetails.Address)
                    .ThenInclude(Address => Address.StateName)
                .Load();
            _context.Entry(patient)
                .Collection(p => p.TreatmentDetails)
                .Query()
                .Include(TreatmentDetails => TreatmentDetails.DiseaseType)
                .Load();
            _context.Entry(patient)
                .Collection(p => p.TreatmentDetails)
                .Query()
                .Include(TreatmentDetails => TreatmentDetails.Hospital)
                    .ThenInclude(Hospital => Hospital.Address)
                        .ThenInclude(Address => Address.StateName)
                .Load();


            return patient;
        }

        public async Task<PatientDetails> AddPatient(PatientDetails patient)
        {
            if(patient.Address.Count == 0)
                throw new Exception("Address missing.Try adding Address");
            if (patient.TreatmentDetails.Count == 0)
                throw new Exception("Treatment Details cannot be null. Try adding Treatment Details");
            var addresses = patient.Address;
            var occupation = patient.OccupationDetails;
            var treatments = patient.TreatmentDetails;
            patient.OccupationDetails = null;
            patient.TreatmentDetails = null;
            patient.Address = null;
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            addresses[0].AddressType = "Temporary";
            addresses[1].AddressType = "Permanent";
            foreach (Address address in addresses)
                await _addresslogic.AddAddress(patient.UniqueID, address);
            if (occupation != null)
                await AddOccupation(patient.UniqueID, occupation);
            foreach (TreatmentDetails treatment in treatments)
                await AddTreatment(patient.UniqueID, treatment);
            var addedpatient = await _context.Patients.FindAsync(patient.UniqueID);
            return addedpatient;

        }

        public async Task<PatientDetails> DeletePatient(string patientid)
        {
            var patient = await _context.Patients.FindAsync(patientid);

            _context.Entry(patient)
                .Reference(p => p.OccupationDetails)
                .Load();
            
            if(patient.OccupationDetails != null)
            {
                var occid = patient.OccupationDetails.OccupationID;
                var occupationdetails = await _context.OccupationDetails.FindAsync(occid);
                _context.OccupationDetails.Remove(occupationdetails);
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task<PatientDetails> UpdatePatient(string patientid, PatientDetails patient)
        {
            if (patientid != patient.UniqueID)
                throw new Exception("UID did not match. Try entering correct ID");
            patient.Address = null;
            patient.OccupationDetails = null;
            patient.TreatmentDetails = null;
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var updatedpatient = await _context.Patients.FindAsync(patientid);
            return updatedpatient;
        }

        public async Task<OccupationDetails> GetOccupation(string patid)
        {
            var occupationDetails = await _context.OccupationDetails.FirstOrDefaultAsync(o => o.UniqueID == patid);
            _context.Entry(occupationDetails)
                .Reference(o => o.Address)
                .Query()
                .Include(Address => Address.StateName)
                .Load();

            return occupationDetails;
        }

        public async Task<OccupationDetails> AddOccupation(string patid, OccupationDetails occupation)
        {
            if (patid != occupation.UniqueID)
                throw new Exception("Occupation does not matches patient. Try adding occupation details for the same patient");
            if (occupation.Address == null)
                throw new Exception("Occupation address cannot be null.Try adding occupation address.");
            var address = occupation.Address;
            occupation.Address = null;
            await _context.OccupationDetails.AddAsync(occupation);
            await _context.SaveChangesAsync();
            await _addresslogic.AddAddress(address);
            return occupation;
        }

        public async Task<OccupationDetails> DeleteOccupation(string patid)
        {
            var occupationdetails = await _context.OccupationDetails.FirstOrDefaultAsync(o => o.UniqueID == patid);
            _context.OccupationDetails.Remove(occupationdetails);
            await _context.SaveChangesAsync();
            return occupationdetails;
        }

        public async Task<OccupationDetails> UpdateOccupation(string patid, OccupationDetails occupation)
        {
            OccupationDetails occobj;
            int addid;
            if (patid != occupation.UniqueID)
                throw new Exception("you are adding Occupation details for wrong patient. Try adding with correct UID");
            using (var _datacontext = new TrackerDbContext(TrackerDbContext.ops.dbOptions))
            {
                occobj = await _datacontext.OccupationDetails.FirstOrDefaultAsync(o => o.UniqueID == patid);
                _datacontext.Entry(occobj)
                    .Reference(h => h.Address)
                    .Query()
                    .Include(Address => Address.StateName)
                    .Load();
                addid = occobj.Address.ID;
            }
            if (occupation.Address == null)
                throw new Exception("Occupation Address cannot be null. Try adding Occupation Address.");
            var addobj = occupation.Address;
            await _addresslogic.UpdateAddress(addid, addobj);

            _context.Entry(occupation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return occupation;
        }

        public async Task<List<TreatmentDetails>> GetTreatments(string patid)
        {
            var treatments = await _context.TreatmentDetails
                                            .Where(t => t.UniqueID == patid)
                                            .Include(t => t.DiseaseType)
                                            .Include(t => t.Hospital)
                                                .ThenInclude(h => h.Address)
                                                    .ThenInclude(a => a.StateName)
                                            .ToListAsync();
            return treatments;
        }

        public async Task<TreatmentDetails> GetTreatment(string patid, int id)
        {
            var treatment = await _context.TreatmentDetails.FindAsync(id);
            if (treatment.UniqueID != patid)
                throw new Exception("This Treatment does not exist for the given Patient");
            return treatment;
        }

        public async Task<TreatmentDetails> AddTreatment(string patid, TreatmentDetails treatment)
        {
            DiseaseTypes diseasetype = null;
            HospitalDetails hospital = null;
            treatment.UniqueID = patid;
            if (treatment.DiseaseTypeID != 0)
                treatment.DiseaseType = null;
            if (treatment.HospitalID != 0)
                treatment.Hospital = null;
            if (treatment.DiseaseTypeID == 0) 
            {
                if (treatment.DiseaseType == null)
                    throw new Exception("Disease type is not entered. Try Entering Disease Type");
                diseasetype = treatment.DiseaseType;
                treatment.DiseaseType = null;
            }
            if (treatment.HospitalID == 0)
            {
                if (treatment.Hospital == null)
                    throw new Exception("Hospital Details not entered. Try Entering Hospital Details");
                hospital = treatment.Hospital;
                treatment.Hospital = null;
            }
            await _context.TreatmentDetails.AddAsync(treatment);
            await _context.SaveChangesAsync();
            if (diseasetype != null)
                await _diseasetypelogic.AddDiseaseType(diseasetype);
            if (hospital != null)
                await _hospitallogic.AddHospital(hospital);
            return treatment;
        }

        public async Task<TreatmentDetails> DeleteTreatment(string patid, int id)
        {
            var treatment = await _context.TreatmentDetails.FindAsync(patid);
            _context.TreatmentDetails.Remove(treatment);
            await _context.SaveChangesAsync();
            return treatment;
        }

        public async Task<TreatmentDetails> UpdateTreatment(string patid,int id, TreatmentDetails treatment)
        {
            TreatmentDetails treatobj;
            DiseaseTypes diseasetype = null;
            HospitalDetails hospital = null;
            using (var _datacontext = new TrackerDbContext(TrackerDbContext.ops.dbOptions))
            {
                treatobj = await _datacontext.TreatmentDetails.FindAsync(id);
                if (treatobj.UniqueID != patid)
                    throw new Exception("This treatment is not related to the patient selected");
            }
            if (treatment.DiseaseTypeID != 0)
                treatment.DiseaseType = null;
            if (treatment.HospitalID != 0)
                treatment.Hospital = null;
            if (treatment.DiseaseTypeID == 0)
            {
                if (treatment.DiseaseType == null)
                    throw new Exception("Disease type is not entered. Try Entering Disease Type");
                diseasetype = treatment.DiseaseType;
                treatment.DiseaseType = null;
            }
            if (treatment.HospitalID == 0)
            {
                if (treatment.Hospital == null)
                    throw new Exception("Hospital Details not entered. Try Entering Hospital Details");
                hospital = treatment.Hospital;
                treatment.Hospital = null;
            }
            _context.Entry(treatment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (diseasetype != null)
                await _diseasetypelogic.AddDiseaseType(diseasetype);
            if (hospital != null)
                await _hospitallogic.AddHospital(hospital);

            var updatedtreatment = await _context.TreatmentDetails.FindAsync(patid);
            return updatedtreatment;
        }


        public bool TreatmentDetailsExists(string id)
        {
            return _context.TreatmentDetails.Any(e => e.UniqueID == id);
        }

        public bool TreatmentDetailsExists(int id)
        {
            return _context.TreatmentDetails.Any(e => e.TreatmentID == id);
        }

        public bool PatientDetailsExists(string id)
        {
            return _context.Patients.Any(e => e.UniqueID == id);
        }

        public bool OccupationDetailsExists(string patid)
        {
            return _context.OccupationDetails.Any(e => e.UniqueID == patid);
        }
    }
}
