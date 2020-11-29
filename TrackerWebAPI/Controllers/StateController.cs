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
    public class StateController : ControllerBase
    {
        private IState _statelogic = new BusinessLogicLayer.Functions.StateFunctions();

        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            var states = await _statelogic.GetStates();
            return Ok(states);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetState(int id)
        {
            var state = await _statelogic.GetState(id);
            if (!_statelogic.StateExists(id))
                return NotFound();
            return Ok(state);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddState(StateNames state)
        {
            var stateobj = await _statelogic.AddState(state);
            return CreatedAtAction("GetState", new { id = stateobj.StateID }, stateobj);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            if (!_statelogic.StateExists(id))
                return NotFound();
            var state = await _statelogic.DeleteState(id);
            return Ok(state);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, StateNames state)
        {
            try
            {
                if (!_statelogic.StateExists(id))
                    return NotFound();
                var updatedstate = await _statelogic.UpdateState(id, state);
                return Ok(updatedstate);
            }
            
            catch (Exception e)
            {
                if (e.Message.Length > 0)
                    return BadRequest(e.Message);
                else
                    throw;
            }
        }

        [HttpGet("{id}/Hospitals")]
        public async Task<IActionResult> GetHospitals(int id)
        {
            var hospitals =  await _statelogic.GetHospitals(id);
            if (hospitals == null)
                return NotFound();
            return Ok(hospitals);
        }

        [HttpGet("{id}/Patients")]
        public IActionResult GetPatients(int id, [FromQuery] int page = 1)
        {
            var patients =  _statelogic.GetPatients(id, page);
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
