namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class ToolLowLevelDefinition
    {
        public int ToolLowLevelDefinitionID { get; set; }
        public string ToolLowLevelDefinitionName { get; set; }
        public int ToolMeasurementLevelDefinitionID { get; set; }
        public int MCode { get; set; }
        public int ValueMin { get; set; }
        public int ValueMax { get; set; }
        public string CovertOpsJsonTool { get; set; }
    }
}