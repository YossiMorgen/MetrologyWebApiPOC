using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class SubTechnology
    {
        public int SubTechnologyID { get; set; }
        public int TechID { get; set; }
        public string SubTechnologyName { get; set; }
        public int MCode { get; set; }
    }
}