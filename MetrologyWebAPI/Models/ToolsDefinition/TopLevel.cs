using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class TopLevel
    {
        public int TopLevelID { get; set; }
        public int SubTechnologyID { get; set; }
        public string name { get; set; }
        public int MCode { get; set; }
    }
}