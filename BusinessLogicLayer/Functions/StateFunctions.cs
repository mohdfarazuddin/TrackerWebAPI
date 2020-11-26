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
    public class StateFunctions : IState
    {
        private readonly TrackerDbContext _context = new TrackerDbContext(TrackerDbContext.ops.dbOptions);

        public async Task<List<StateNames>> GetStates()
        {
            var states = await _context.StateNames.ToListAsync();
            return states;
        }

        public async Task<StateNames> GetState(int stateid) 
        {
            var state = await _context.StateNames.FirstOrDefaultAsync(s => s.StateID == stateid);
            return state;
        }

        public async Task<StateNames> AddState(StateNames state)
        {
            await _context.StateNames.AddAsync(state);
            await _context.SaveChangesAsync();
            return state;
        }

        public async Task<StateNames> DeleteState(int stateid)
        {
            var state = await _context.StateNames.FindAsync(stateid);
            _context.StateNames.Remove(state);
            await _context.SaveChangesAsync();
            return state;
        }

        public async Task<StateNames> UpdateState(int stateid, StateNames state)
        {
            if (state.StateID != stateid)
                throw new Exception("State ID did not match. Try adding correct state ID");
            _context.Entry(state).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var updatedstate = await _context.StateNames.FindAsync(stateid);
            return updatedstate;
        }
        public  PageList<HospitalDetails> GetHospitals(int id, int page)
        {
            var hospitals = PageList<HospitalDetails>.ToPagedList( _context.Hospitals
                                                    .Include(h => h.Address)
                                                    .Where(h => h.Address.StateID == id)
                                                    .OrderBy(h => h.Name), page);
            return hospitals;
        }

        public  PageList<PatientDetails> GetPatients(int id, int page)
        {
            var patients = PageList<PatientDetails>.ToPagedList(_context.Patients
                                                    .Include(p => p.Address)
                                                    .Where(p => p.Address
                                                                 .Any(a => a.StateID == id))
                                                    .OrderBy(p => p.Name), page);

            return patients;
        }

        public bool StateExists(int stateid)
        {
            return _context.StateNames.Any(e => e.StateID == stateid);
        }
    }
}
