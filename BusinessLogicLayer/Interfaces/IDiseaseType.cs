using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDiseaseType
    {

        Task<List<DiseaseTypes>> GetDiseaseTypes();

        Task<DiseaseTypes> GetDiseaseType(int id);

        Task<DiseaseTypes> AddDiseaseType(DiseaseTypes diseasetype);

        Task<DiseaseTypes> DeleteDiseaseType(int id);

        Task<DiseaseTypes> UpdateDiseaseType(int id, DiseaseTypes diseasetype);

        bool DiseaseTypesExists(int id);


    }
}
