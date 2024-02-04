using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class ToolMeasurementLevelDefinition
    {
        public int ToolMeasurementLevelDefinitionID { get; set; }
        public int ToolTopLevelDefinitionID { get; set; }
        public int ValueMin { get; set; }
        public int ValueMax { get; set; }
        public int ValueUnitID { get; set; }
        public int MCode { get; set; }
        public int UncertaintyDelta { get; set; }
        public int UncertaintyUnitID { get; set; }
    }
}