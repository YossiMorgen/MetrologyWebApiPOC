using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class ToolFamilyLevelDefinition
    {
        public int ToolFamilyLevelDefinitionID { get; set; }
        public string ToolFamilyLevelDefinitionName { get; set; }
        public int ToolTopLevelDefinitionID { get; set; }
    }
}