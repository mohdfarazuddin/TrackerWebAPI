using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessObjects
{
    public class OccupationDetails
    {
        public int OccupationID { get; set; }

        public string UniqueID { get; set; }

        public string OccupationType { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
        
        public Address Address { get; set; }

        public PatientDetails Patient { get; set; }

    }
}
