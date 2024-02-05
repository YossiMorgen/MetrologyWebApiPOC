using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class ToolLowLevelDefinition
    {
        public int ToolLowLevelDefinitionID { get; set; }
        public int ToolMeasurementLevelDefinitionID { get; set; }
        public int MCode { get; set; }
        public int ValueMin { get; set; }
        public int ValueMax { get; set; }
        public int ValueUnitID { get; set; }
    }
}