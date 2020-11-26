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
    public class DiseaseTypeFunctions : IDiseaseType
    {

        private readonly TrackerDbContext _context = new TrackerDbContext(TrackerDbContext.ops.dbOptions);

        public async Task<List<DiseaseTypes>> GetDiseaseTypes()
        {
            var diseasetypes = await _context.DiseaseTypes.ToListAsync();
            return diseasetypes;
        }

        public async Task<DiseaseTypes> GetDiseaseType(int id)
        {
            var diseasetype = await _context.DiseaseTypes.FindAsync(id);
            return diseasetype;
        }

        public async Task<DiseaseTypes> AddDiseaseType(DiseaseTypes diseasetype)
        {
            await _context.DiseaseTypes.AddAsync(diseasetype);
            await _context.SaveChangesAsync();
            return diseasetype;
        }

        public async Task<DiseaseTypes> DeleteDiseaseType(int id)
        {
            var diseasetype = await _context.DiseaseTypes.FindAsync(id);
            _context.DiseaseTypes.Remove(diseasetype);
            await _context.SaveChangesAsync();
            return diseasetype;
        }

        public async Task<DiseaseTypes> UpdateDiseaseType(int id, DiseaseTypes diseasetype)
        {
            if (diseasetype.DiseaseTypeID != id)
                throw new Exception("Disease Type ID did not match. Try specifying correct ID");
            _context.Entry(diseasetype).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return diseasetype;
        }

        public bool DiseaseTypesExists(int id)
        {
            return _context.DiseaseTypes.Any(e => e.DiseaseTypeID == id);
        }

    }
}
