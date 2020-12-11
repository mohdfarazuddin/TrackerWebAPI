using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjects
{
    public class Dashboard
    {
        public int? StateID { get; set; }

        public int? hospitalID { get; set; }

        public string? State { get; set; }

        public string? Hospital { get; set; }

        public int TotalCases { get; set; }

        public int ActiveCases { get; set; }

        public int CuredCases { get; set; }

        public int Fatalities { get; set; }

    }
}
