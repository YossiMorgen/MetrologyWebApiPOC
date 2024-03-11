using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrologyWebAPI.Models
{
    public class ToolDefinition
    {
        public int ToolDefinitionID { get; set; }
        public int ToolLowLevelDefinitionID { get; set; }
        public int MeasurementResolutionSingleID { get; set; }
        public int TestTemplateID { get; set; }
    }
}