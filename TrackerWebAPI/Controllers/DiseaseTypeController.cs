using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseTypeController : ControllerBase
    {
        private IDiseaseType _diseasetypelogic = new BusinessLogicLayer.Functions.DiseaseTypeFunctions();

        [HttpGet]
        public async Task<IActionResult> GetDiseaseTypes()
        {
            var diseasetype = await _diseasetypelogic.GetDiseaseTypes();
            return Ok(diseasetype);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiseaseType(int id)
        {
            var diseasetype = await _diseasetypelogic.GetDiseaseType(id);
            if (!_diseasetypelogic.DiseaseTypesExists(id))
                return NotFound();
            return Ok(diseasetype);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddDiseaseType(DiseaseTypes diseasetype)
        {
            var diseasetypeobj = await _diseasetypelogic.AddDiseaseType(diseasetype);
            return CreatedAtAction("GetDiseaseType", new { id = diseasetypeobj.DiseaseTypeID }, diseasetypeobj);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiseaseType(int id)
        {
            if (!_diseasetypelogic.DiseaseTypesExists(id))
                return NotFound();
            var diseasetype = await _diseasetypelogic.DeleteDiseaseType(id);
            return Ok(diseasetype);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiseaseType(int id, DiseaseTypes diseasetype)
        {
            try
            {
                if (!_diseasetypelogic.DiseaseTypesExists(id))
                    return NotFound();
                var updateddiseasetype = await _diseasetypelogic.UpdateDiseaseType(id, diseasetype);
                return Ok(updateddiseasetype);
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
