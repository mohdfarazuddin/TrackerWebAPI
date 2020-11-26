using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BusinessObjects
{
    public class PatientDetails
    {

        public string UniqueID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }

        public string Phone { get; set; }

        public IList<Address> Address { get; set; } = new List<Address>();
        
        public OccupationDetails OccupationDetails { get; set; }
        
        public IList<TreatmentDetails> TreatmentDetails { get; set; } = new List<TreatmentDetails>();

    }
}
