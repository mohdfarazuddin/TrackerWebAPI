using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IPatient
    {
        PageList<PatientDetails> GetPatients(int page);

        Task<PatientDetails> GetPatient(string patientid);

        Task<PatientDetails> AddPatient(PatientDetails patient);

        Task<PatientDetails> DeletePatient(string patientid);

        Task<PatientDetails> UpdatePatient(string patientid,PatientDetails patient);

        Task<OccupationDetails> GetOccupation(string patid);

        Task<OccupationDetails> AddOccupation(string patid, OccupationDetails occupation);

        Task<OccupationDetails> DeleteOccupation(string patid);

        Task<OccupationDetails> UpdateOccupation(string patid, OccupationDetails occupation);

        Task<List<TreatmentDetails>> GetTreatments(string patid);

        Task<TreatmentDetails> GetTreatment(string patid, int id);

        Task<TreatmentDetails> AddTreatment(string patid, TreatmentDetails treatment);

        Task<TreatmentDetails> DeleteTreatment(string patid, int id);

        Task<TreatmentDetails> UpdateTreatment(string patid, int id, TreatmentDetails treatment);

        bool PatientDetailsExists(string id);

        bool OccupationDetailsExists(string patid);

        bool TreatmentDetailsExists(string id);

        bool TreatmentDetailsExists(int id);
    }
}
