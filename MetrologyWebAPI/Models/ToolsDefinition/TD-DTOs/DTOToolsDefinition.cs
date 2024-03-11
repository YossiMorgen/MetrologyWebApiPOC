using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class DTOTavrig
    {
        public int ToolLowLevelDefinitionID { get; set; }
        public int ToolTopLevelDefinitionID { get; set; }

        public TestDefinition diameterGo { get; set; }
        public TestDefinition diameterNoGo { get; set; }
        public TestDefinition PDGo { get; set; }
        public TestDefinition PDNoGo { get; set; }

        public Endurance diameterGoEndurance { get; set; }
        public Endurance diameterNoGoEndurance { get; set; }
        public Endurance PDGoEndurance { get; set; }
        public Endurance PDNoGoEndurance { get; set; }
        
        public string CovertOpsJsonTool { get; set; }

        //test template 
    }
}