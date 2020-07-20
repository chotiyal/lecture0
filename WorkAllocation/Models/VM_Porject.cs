using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkAllocation.Models
{
    public class VM_Porject
    {
        public int PROJECT_UID { get; set; }
        public string PROJECT_CODE { get; set; }
        public string PROJECT_NAME{ get; set; }
        public string DOMAIN { get; set; }
        public string START_DATE{ get; set; }
        public string END_DATE { get; set; }
        public string STATUS { get; set; }
        public string ENTRY_DATE { get; set; }
        public string [] EMP_CODE { get; set; }
        public List <VM_Porject_Details> PROJECT_DETAILS_LIST { get; set; }
    }
    public class VM_Porject_Details
    {
        public int PROJECT_TEAM_UID { get; set; }
        public string PROJECT_CODE { get; set; }
        public string EMP_CODE { get; set; }
        public string EMP_TYPE{ get; set; }
        public string STATUS{ get; set; }
        public string END_DATE { get; set; }
        public string ENTRY_BY { get; set; }
        public string ENTRY_DATE { get; set; }

    }
}