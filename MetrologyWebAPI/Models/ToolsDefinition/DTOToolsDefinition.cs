using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class DTOToolsDefinition
    {
        public string innerComment { get; set; }

        public MeasurementUnit[] MeasurementUnits { get; set; }
        public Technology[] Technologies { get; set; }
        public SubTechnology[] SubTechnologies { get; set; }
        public IsoProcedure[] IsoProcedures { get; set; }
        public ToolTopLevelDefinition[] ToolTopLevelDefinitions { get; set; }
        public ToolFamilyLevelDefinition[] ToolFamilyLevelDefinitions { get; set; }
        public ToolMeasurementLevelDefinition[] ToolMeasurementLevelDefinitions { get; set; }
        public ToolLowLevelDefinition[] ToolLowLevelDefinitions { get; set; }
        public Resolution[] Resolutions { get; set; }
        public Resolution_ToolTopLevelDefinition[] Resolution_ToolTopLevelDefinitions { get; set; }
        public TestDefinitionGroup[] TestDefinitionGroups { get; set; }
        public TestDefinition[] TestDefinitions { get; set; }
        public Endurance[] Endurance { get; set; }
        public TestTemplate[] TestTemplates { get; set; }
        public TestTemplatesDefinition[] TestTemplatesDefinitions { get; set; }
    }

    public class PostToolDefinition
    {
        public string toolDefinitionName { get; set; }
        public object payload { get; set; }

    }

    public class PostToolTopLevelDefinition
    {
        public ToolTopLevelDefinition ToolTopLevelDefinition { get; set; }
        public string SValues { get; set; }
        public IsoProcedure IsoProcedure { get; set; }
    }

    public class PostToolTopLevelDefinitionResult
    {
        public DataTable Resolution_ToolTopLevelDefinitions { get; set; }
        public DataTable Resolutions { get; set; }
        public ToolTopLevelDefinition ToolTopLevelDefinition { get; set; }
        public IsoProcedure IsoProcedure { get; set; }
    }

    public class PostTestDefinition
    {
        public TestDefinition TestDefinition { get; set; }
        public Endurance[] Endurance { get; set; }
    }

    public class PostTestTemplate
    {
        public TestTemplate TestTemplate { get; set; }
        public DataTable TestTemplatesDefinitions { get; set; }
        public string TestDefinitionsIDs { get; set; }
    }


    public class PostGroupAndTestDefAndEndurance
    {
        public TestDefinitionGroup TestDefinitionGroup { get; set; }
        public TestDefinition TestDefinition { get; set; }
        public TestTemplate TestTemplate { get; set; }
        public Endurance Endurance { get; set; }
    } 

    public class PostTavrig
    {
        public ToolLowLevelDefinition ToolLowLevelDefinition { get; set; }
        public PostGroupAndTestDefAndEndurance[] PostGroupAndTestDefAndEndurance { get; set; }
    }
}