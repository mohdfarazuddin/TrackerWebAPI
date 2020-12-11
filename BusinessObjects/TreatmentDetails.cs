using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessObjects
{
    public class TreatmentDetails
    {
        public int TreatmentID { get; set; }

        public string UniqueID { get; set; }

        public int DiseaseTypeID { get; set; }

        public string DiseaseName { get; set; }

        public int HospitalID { get; set; }

        public DateTime AdmitDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public string Prescription { get; set; }

        public string CurrentStatus { get; set; }

        public string IsFatality { get; set; }

        public PatientDetails Patient { get; set; }

        public DiseaseTypes DiseaseType { get; set; }
        
        public HospitalDetails Hospital { get; set; }

    }
}
