using baMetrologyWebAPI.Models.ToolsDefinitionckend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class DTOToolsDefinition
    {
        public string innerComment { get; set; }

        public Technology[] Technologies { get; set; }
        public SubTechnology[] SubTechnologies { get; set; }
        public IsoProcedure[] IsoProcedures { get; set; }
        public ToolTopLevelDefinition[] ToolTopLevelDefinitions { get; set; }
        public MeasurementUnits[] MeasurementUnits { get; set; }
        public ToolMeasurementLevelDefinition[] ToolMeasurementLevelDefinition { get; set; }
        public ToolLowLevelDefinition[] ToolLowLevelDefinition { get; set; }
    }

    public class PostToolDefinition
    {
        public string toolDefinitionName { get; set; }
        public object payload { get; set; }

    }
}