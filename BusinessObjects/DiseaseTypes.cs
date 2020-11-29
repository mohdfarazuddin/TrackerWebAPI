using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessObjects
{
    public class DiseaseTypes

    {

        public int DiseaseTypeID { get; set; }

        public string DiseaseType { get; set; }

        public ICollection<TreatmentDetails> TreatmentDetails { get; set; } = new HashSet<TreatmentDetails>();
    }
}
