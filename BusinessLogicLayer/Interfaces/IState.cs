using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IState
    {
        Task<List<StateNames>> GetStates();

        Task<StateNames> GetState(int stateid);

        Task<StateNames> AddState(StateNames state);

        Task<StateNames> DeleteState(int stateid);

        Task<StateNames> UpdateState(int stateid, StateNames state);

        PageList<HospitalDetails> GetHospitals(int id,int page);

        PageList<PatientDetails> GetPatients(int id,int page);

        bool StateExists(int stateid);
    }
}
