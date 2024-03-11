using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class TestTemplate
    {
        public int TestTemplateID { get; set; }
        public string TestTemplateName { get; set; }
        public int ToolLowLevelDefinitionID { get; set; }
    }
}