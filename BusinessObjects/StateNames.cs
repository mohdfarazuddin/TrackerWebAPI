using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessObjects
{
    public class StateNames
    {
        public int StateID { get; set; }

        public string State { get; set; }

        public ICollection<Address> Address { get; set; } = new HashSet<Address>();

    }
}