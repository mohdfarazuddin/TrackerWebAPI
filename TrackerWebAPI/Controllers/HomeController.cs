using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private IDashboard _dashboardlogic = new BusinessLogicLayer.Functions.DashboardFunctions();

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _dashboardlogic.GetDashboard();
            return Ok(dashboard);
        }

        [HttpGet("Hospital/{id}")]
        public async Task<IActionResult> GetDashboardHospital(int id)
        {
            var dashboard = await _dashboardlogic.GetDashboardHospital(id);
            return Ok(dashboard);
        }

        [HttpGet("State/{id}")]
        public async Task<IActionResult> GetDashboardState(int id)
        {
            var dashboard = await _dashboardlogic.GetDashboardState(id);
            return Ok(dashboard);
        }

        [HttpGet("State")]
        public async Task<IActionResult> GetDashboardStates()
        {
            var dashboard = await _dashboardlogic.GetDashboardStates();
            return Ok(dashboard);
        }

        [HttpGet("Hospital")]
        public async Task<IActionResult> GetDashboardHospitals(int id)
        {
            var dashboard = await _dashboardlogic.GetDashboardHospitals(id);
            return Ok(dashboard);
        }


    }
}
