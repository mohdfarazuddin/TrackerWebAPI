using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessObjects
{
    public class Address
    {

        public int ID { get; set; }

        public string? UniqueID { get; set; }

        public int? OccupationID { get; set; }

        public int? HospitalID { get; set; }

        public string AddressType { get; set; }

        public string Addressline { get; set; }

        public string City { get; set; }

        public int StateID { get; set; }

        public string ZipCode { get; set; }

        public StateNames StateName { get; set; }

        public PatientDetails PatientAddress { get; set; }

        public OccupationDetails OccupationAddress { get; set; }

        public HospitalDetails HospitalAddress { get; set; }

    }
}
