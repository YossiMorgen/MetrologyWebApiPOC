using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class Resolution_ToolTopLevelDefinition
    {
        public int Resolution_ToolTopLevelDefinitionID { get; set; }
        public int ResolutionID { get; set; }
        public int ToolTopLevelDefinitionID { get; set; }
        public int ValueUnitID { get; set; }
    }
}