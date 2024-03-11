using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class ToolTopLevelDefinition
    {
        public int ToolTopLevelDefinitionID { get; set; }
        public string ToolTopLevelDefinitionName { get; set; }
        public int SubTechID { get; set; }
        public int ValueUnitID { get; set; }
    }
}