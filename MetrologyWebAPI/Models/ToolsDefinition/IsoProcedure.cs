using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class IsoProcedure
    {
        public int IsoProcedureID { get; set; }
        public int MCode { get; set; }
        public string WordFileLink { get; set; }
        public string CertificateText { get; set; }
    }
}