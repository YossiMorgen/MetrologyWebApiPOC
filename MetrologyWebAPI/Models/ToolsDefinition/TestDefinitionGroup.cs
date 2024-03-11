using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class TestDefinitionGroup
    {
        public int TestDefinitionGroupID { get; set; }
        public string TestDefinitionGroupName { get; set; }
        public int ToolTopLevelDefinitionID { get; set; }
        public string DeviationCalcType { get; set; }
    }
}