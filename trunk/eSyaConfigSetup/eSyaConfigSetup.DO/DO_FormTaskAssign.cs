using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaConfigSetup.DO
{
   public class DO_FormTaskAssign
    {
        public int FormId { get; set; }
        public int TaskId { get; set; }
        public bool AutoReassignTimeline { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string TaskName { get; set; }
        public string FormName { get; set; }
    }
}
