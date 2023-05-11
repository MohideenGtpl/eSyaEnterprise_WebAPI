using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEciuom
    {
        public int UnitOfMeasure { get; set; }
        public string Uompurchase { get; set; }
        public string Uomstock { get; set; }
        public string Uompdesc { get; set; }
        public string Uomsdesc { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }
    }
}
