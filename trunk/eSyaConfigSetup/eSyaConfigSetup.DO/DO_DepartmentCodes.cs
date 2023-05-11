using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaConfigSetup.DO
{
    public class DO_DepartmentCodes
    {
        public int DepartmentID { get; set; }
        public string DepartmentDesc { get; set; }
        public string DeptShortDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }

    public class DO_DepartmentLocation
    {
        public int BusinessKey { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentDesc { get; set; }
        public string DeptShortDesc { get; set; }
        public int DeptLocnID { get; set; }
        public string LocationDescription { get; set; }
        public string LocnShortDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
