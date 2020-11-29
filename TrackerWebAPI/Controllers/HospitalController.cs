using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {

        private IHospital _hospitallogic = new BusinessLogicLayer.Functions.HospitalFunctions();

        [HttpGet]
        public IActionResult GetHospitals([FromQuery]int page=1)
        {
            var hospital =  _hospitallogic.GetHospitals(page);
            var metadata = new
            {
                hospital.TotalCount,
                hospital.PageSize,
                hospital.CurrentPage,
                hospital.TotalPages,
                hospital.HasNext,
                hospital.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(hospital);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHospital(int id)
        {
            var hospital = await _hospitallogic.GetHospital(id);
            if (!_hospitallogic.HospitalDetailsExists(id))
                return NotFound();
            return Ok(hospital);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddHospital(HospitalDetails hospital)
        {
            try
            {
                var hos = await _hospitallogic.AddHospital(hospital);
                return CreatedAtAction("GetHospital", new { id = hos.HospitalID }, hos);
            }
            catch (Exception e)
            {
                if (_hospitallogic.HospitalDetailsExists(hospital.HospitalID))
                    return Conflict();
                if (e.Message.Length > 0)
                    return BadRequest(e.Message);
                else
                    throw;
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHospital(int id,HospitalDetails hospital)
        {
            try
            {
                if (!_hospitallogic.HospitalDetailsExists(id))
                    return NotFound();
                var updatedhospital = await _hospitallogic.UpdateHospital(id, hospital);
                return Ok(updatedhospital);
            }
            catch (Exception e)
            {
                if (e.Message.Length > 0)
                    return BadRequest(e.Message);
                else
                    throw;
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            if (!_hospitallogic.HospitalDetailsExists(id))
                return NotFound();
            var hospital = await _hospitallogic.DeleteHospital(id);
            return Ok(hospital);
        }

        [HttpGet("{id}/Patients")]
        public IActionResult GetPatients(int id, [FromQuery] int page = 1)
        {
            var patients =  _hospitallogic.GetPatients(id,page);
            var metadata = new
            {
                patients.TotalCount,
                patients.PageSize,
                patients.CurrentPage,
                patients.TotalPages,
                patients.HasNext,
                patients.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            if (patients == null)
                return NotFound();
            return Ok(patients);
        }

    }
}
