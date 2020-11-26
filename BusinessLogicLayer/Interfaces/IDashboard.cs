using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDashboard
    {

        Task<Dashboard> GetDashboard();

        Task<Dashboard> GetDashboardHospital(int id);

        Task<Dashboard> GetDashboardState(int id);

    }
}
