using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IHospital
    {

        PageList<HospitalDetails> GetHospitals(int page);

        Task<HospitalDetails> GetHospital(int id);

        Task<HospitalDetails> AddHospital(HospitalDetails hospital);

        Task<HospitalDetails> UpdateHospital(int id, HospitalDetails hospital);

        Task<HospitalDetails> DeleteHospital(int id);

        PageList<PatientDetails> GetPatients(int id, int page);

        bool HospitalDetailsExists(int id);


    }
}
