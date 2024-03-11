using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class TestDefinition
    {
        public int TestDefinitionID { get; set; }
        public string TestDefinitionName { get; set; }
        public int TestDefinitionGroupID { get; set; }
        public double ValueRequired { get; set; }
        public double ValueUncertainty { get; set; }
        public bool IsIso17025 { get; set; }
    }
}