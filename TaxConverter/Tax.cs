using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxConverter
{
    class Tax
    {
        public string ScenarioId { get; set; }
        public string ShipToPostalCode { get; set; }
        public string ShipToState { get; set; }
        public string ShipToCounty { get; set; }
        public string ShipToCity { get; set; }
        public string Tax_Rate { get; set; }
        public string TaxCode { get; set; }
        public string EffDate { get; set; }
    }
}
