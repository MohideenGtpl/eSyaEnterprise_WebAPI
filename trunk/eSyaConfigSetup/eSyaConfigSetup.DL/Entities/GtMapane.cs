using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtMapane
    {
        public string TemplateType { get; set; }
        public string LanguageCode { get; set; }
        public int TemplateId { get; set; }
        public string DisplayType { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayName { get; set; }
        public string VideoUrl { get; set; }
        public string TemplateDesc { get; set; }
        public string Faqs { get; set; }
        public string FaqsAnswer { get; set; }
        public int DisplayOrder { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }
    }
}
