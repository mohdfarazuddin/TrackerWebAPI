using BusinessLogicLayer.Interfaces;
using DataAccessLayer.DataContext;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Functions
{
    public class DashboardFunctions : IDashboard
    {
        private readonly TrackerDbContext _context = new TrackerDbContext(TrackerDbContext.ops.dbOptions);

        public async Task<Dashboard> GetDashboard()
        {
            Dashboard dashboard = new Dashboard();
            dashboard.TotalCases = await _context.TreatmentDetails.CountAsync();
            dashboard.ActiveCases = await _context.TreatmentDetails.Where(t => t.DischargeDate == null).CountAsync();
            dashboard.Fatalities = await _context.TreatmentDetails.Where(t => t.IsFatality == "yes").CountAsync();
            dashboard.CuredCases = dashboard.TotalCases - (dashboard.ActiveCases + dashboard.Fatalities);
            return dashboard;
        }

        public async Task<Dashboard> GetDashboardHospital(int id)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.TotalCases = await _context.TreatmentDetails.Where(t => t.HospitalID == id).CountAsync();
            dashboard.ActiveCases = await _context.TreatmentDetails.Where(t => (t.DischargeDate == null && t.HospitalID == id)).CountAsync();
            dashboard.Fatalities = await _context.TreatmentDetails.Where(t => (t.IsFatality == "yes" && t.HospitalID == id)).CountAsync();
            dashboard.CuredCases = dashboard.TotalCases - (dashboard.ActiveCases + dashboard.Fatalities);
            return dashboard;
        }

        public async Task<Dashboard> GetDashboardState(int id)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.TotalCases = await _context.TreatmentDetails
                                                  .Include(t => t.Hospital)
                                                      .ThenInclude(p => p.Address)
                                                  .Where(a => a.Hospital.Address.StateID == id)
                                                  .CountAsync();
            dashboard.ActiveCases = await _context.TreatmentDetails
                                                  .Include(t => t.Hospital)
                                                      .ThenInclude(p => p.Address)
                                                  .Where(t => (t.DischargeDate == null && t.Hospital.Address.StateID == id))
                                                  .CountAsync();
            dashboard.Fatalities = await _context.TreatmentDetails
                                                .Include(t => t.Hospital)
                                                    .ThenInclude(p => p.Address)
                                                .Where(t => (t.IsFatality == "yes" && t.Hospital.Address.StateID == id))
                                                .CountAsync();
            dashboard.CuredCases = dashboard.TotalCases - (dashboard.ActiveCases + dashboard.Fatalities);
            return dashboard;
        }

        public async Task<List<Dashboard>> GetDashboardStates()
        {
            List<Dashboard> dashboardstates= new List<Dashboard>();
            var ids = await _context.StateNames.OrderBy(s => s.State).Select(s=>s.StateID).ToListAsync();
            foreach (var item in ids)
            {
                Dashboard dashboard = new Dashboard();
                dashboard.StateID = item;
                dashboard.State = await _context.StateNames.Where(s => s.StateID == item)
                                                            .Select(s => s.State).FirstOrDefaultAsync();
                                                            
                dashboard.TotalCases = await _context.TreatmentDetails
                                                  .Include(t => t.Hospital)
                                                      .ThenInclude(h => h.Address)
                                                  .Where(a => a.Hospital.Address.StateID== item)
                                                  .CountAsync();
                dashboard.ActiveCases = await _context.TreatmentDetails
                                                      .Include(t => t.Hospital)
                                                          .ThenInclude(p => p.Address)
                                                      .Where(t => (t.DischargeDate == null && t.Hospital.Address.StateID == item))
                                                      .CountAsync();
                dashboard.Fatalities = await _context.TreatmentDetails
                                                    .Include(t => t.Hospital)
                                                        .ThenInclude(p => p.Address)
                                                    .Where(t => (t.IsFatality == "yes" && t.Hospital.Address.StateID == item))
                                                    .CountAsync();
                dashboard.CuredCases = dashboard.TotalCases - (dashboard.ActiveCases + dashboard.Fatalities);
                dashboardstates.Add(dashboard);
            }
            return dashboardstates;
        }

        public async Task<List<Dashboard>> GetDashboardHospitals(int id)
        {
            List<Dashboard> dashboardhospitals = new List<Dashboard>();
            var ids = await _context.Hospitals.Include(h => h.Address)
                                               .Where(h => h.Address.StateID == id)
                                               .OrderBy(h => h.Name)
                                               .Select(h => h.HospitalID)
                                               .ToListAsync();
            foreach (var item in ids)
            {
                Dashboard dashboard = new Dashboard();
                dashboard.hospitalID = item;
                dashboard.Hospital = await _context.Hospitals.Where(h => h.HospitalID == item)
                                                            .Select(h => h.Name).FirstOrDefaultAsync();

                dashboard.TotalCases = await _context.TreatmentDetails.Where(t => t.HospitalID == item).CountAsync();
                dashboard.ActiveCases = await _context.TreatmentDetails.Where(t => (t.DischargeDate == null && t.HospitalID == item)).CountAsync();
                dashboard.Fatalities = await _context.TreatmentDetails.Where(t => (t.IsFatality == "yes" && t.HospitalID == item)).CountAsync();
                dashboard.CuredCases = dashboard.TotalCases - (dashboard.ActiveCases + dashboard.Fatalities);
                dashboardhospitals.Add(dashboard);
            }
            return dashboardhospitals;
        }

    }
}
