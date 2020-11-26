using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessObjects
{
    public class HospitalDetails
    {

        public int HospitalID { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public Address Address { get; set; }

        [JsonIgnore]
        public ICollection<TreatmentDetails> TreatmentDetails { get; set; } = new HashSet<TreatmentDetails>();

    }
}
