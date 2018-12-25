using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LostAndFound.Models
{
    public class ReportViewModel
    {
        public bool LF { get; set; }
        public string Descr { get; set; }
        public int CID { get; set; }
        public int PID { get; set; }
        public int NumberOfReports { get; set; }
        public string CategoryName { get; set; }

    }
}