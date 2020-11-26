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
    public class PatientController : ControllerBase
    {
        private IPatient _patientlogic = new BusinessLogicLayer.Functions.PatientFunctions();

        [Authorize]
        [HttpGet]
        public  IActionResult GetPatients([FromQuery]int page = 1)
        {
            var patients =  _patientlogic.GetPatients(page);

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

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(string id)
        {
            var patient = await _patientlogic.GetPatient(id);
            if (!_patientlogic.PatientDetailsExists(id))
                return NotFound();
            return Ok(patient);
        }

        [HttpGet("{patid}/Occupation")]
        public async Task<IActionResult> GetOccupation(string patid)
        {
            if (!_patientlogic.OccupationDetailsExists(patid))
                return NotFound();
            var occupation = await _patientlogic.GetOccupation(patid);
            return Ok(occupation);
        }

        [HttpGet("{patid}/Treatments")]
        public async Task<IActionResult> GetTreatments(string patid)
        {
            var treatment = await _patientlogic.GetTreatments(patid);
            if (!_patientlogic.TreatmentDetailsExists(patid))
                return NotFound();
            return Ok(treatment);
        }

        [HttpGet("{patid}/Treatment/{id}")]
        public async Task<IActionResult> GetTreatment(string patid, int id)
        {
            var treatment = await _patientlogic.GetTreatment(patid, id);
            if (treatment == null)
            {
                return NotFound();
            }
            return Ok(treatment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientDetails patient)
        {
            try
            {
                var pat = await _patientlogic.AddPatient(patient);
            }
            catch (Exception e)
            {
                if (_patientlogic.PatientDetailsExists(patient.UniqueID))
                    return Conflict();
                if (e.Message.Length > 0)
                    return BadRequest(e.Message);
                else
                    throw;
            }
            return CreatedAtAction("GetPatient", new { id = patient.UniqueID }, patient);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(string id)
        {
            if (!_patientlogic.PatientDetailsExists(id))
                return NotFound();
            var patient = await _patientlogic.DeletePatient(id);
            return Ok(patient);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(string id, PatientDetails patient)
        {
            try
            {
                if (!_patientlogic.PatientDetailsExists(id))
                    return NotFound();
                var UpdatePatient = await _patientlogic.UpdatePatient(id, patient);
                return Ok(UpdatePatient);
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
        [HttpPost("{patid}/Occupation")]
        public async Task<IActionResult> AddOccupation(string patid, OccupationDetails occupation)
        {
            try
            {
                if (_patientlogic.OccupationDetailsExists(occupation.UniqueID))
                    return Conflict();
                var occ = await _patientlogic.AddOccupation(patid, occupation);
                return CreatedAtAction(nameof(GetOccupation), new {patid = occ.UniqueID} ,occ);
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
        [HttpDelete("{patid}/Occupation")]
        public async Task<IActionResult> DeleteOccupation(string patid)
        {
            if (!_patientlogic.OccupationDetailsExists(patid))
                return NotFound();
            var occupation = await _patientlogic.DeleteOccupation(patid);
            return Ok(occupation);
        }

        [Authorize]
        [HttpPut("{patid}/Occupation")]
        public async Task<IActionResult> UpdateOccupation(string patid, OccupationDetails occupation)
        {
            try
            {
                if (!_patientlogic.OccupationDetailsExists(patid))
                    return NotFound();
                var UpdateOccupation = await _patientlogic.UpdateOccupation(patid, occupation);
                return Ok(UpdateOccupation);
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
        [HttpPost("{patid}/Treatment")]
        public async Task<ActionResult<TreatmentDetails>> AddTreatment(int a,string patid, TreatmentDetails treatment)
        {
            try
            {
                if (patid != treatment.UniqueID)
                {
                    return BadRequest();
                }
                var treat = await _patientlogic.AddTreatment(patid,treatment);
                return CreatedAtAction("GetTreatment", new { patid = treatment.UniqueID, id = treatment.TreatmentID }, treat);
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
        [HttpDelete("{patid}/Treatment/{id}")]
        public async Task<IActionResult> DeleteTreatment(string patid, int id)
        {
            if (!_patientlogic.TreatmentDetailsExists(id))
                return NotFound();
            var treatment = await _patientlogic.DeleteTreatment(patid,id);
            return Ok(treatment);
        }

        [Authorize]
        [HttpPut("{patid}/Treatment/{id}")]
        public async Task<IActionResult> UpdateTreatment(string patid,int id, TreatmentDetails treatment)
        {
            try
            {
                if (!_patientlogic.TreatmentDetailsExists(patid))
                    return NotFound();
                var UpdateOccupation = await _patientlogic.UpdateTreatment(patid,id,treatment);
                return Ok(UpdateOccupation);
            }
            catch (Exception e)
            {
                if (e.Message.Length > 0)
                    return BadRequest(e.Message);
                else
                    throw;
            }
        }



    }
}
