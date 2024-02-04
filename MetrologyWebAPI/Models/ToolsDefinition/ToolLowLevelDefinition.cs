using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace baMetrologyWebAPI.Models.ToolsDefinitionckend
{
    public class ToolLowLevelDefinition
    {
        public int ToolLowLevelDefinitionID { get; set; }
        public string ToolLowLevelDefinitionName { get; set; }
        public int MCode { get; set; }
        public int ValueMin { get; set; }
        public int ValueMax { get; set; }
        public int ValueUnitID { get; set; }
    }
}