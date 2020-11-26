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
                                                  .Include(t => t.Patient)
                                                      .ThenInclude(p => p.Address)
                                                  .Where(a => a.Patient.Address.Any(a => a.StateID == id))
                                                  .CountAsync();
            dashboard.ActiveCases = await _context.TreatmentDetails
                                                  .Include(t => t.Patient)
                                                      .ThenInclude(p => p.Address)
                                                  .Where(t => (t.DischargeDate == null && t.Patient.Address.Any(a => a.StateID == id)))
                                                  .CountAsync();
            dashboard.Fatalities = await _context.TreatmentDetails
                                                .Include(t => t.Patient)
                                                    .ThenInclude(p => p.Address)
                                                .Where(t => (t.IsFatality == "yes" && t.Patient.Address.Any(a => a.StateID == id)))
                                                .CountAsync();
            dashboard.CuredCases = dashboard.TotalCases - (dashboard.ActiveCases + dashboard.Fatalities);

            return dashboard;
        }

    }
}
