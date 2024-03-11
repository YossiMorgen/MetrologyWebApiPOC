using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI.Models.ToolsDefinition
{
    public class Endurance
    {
        public int EnduranceID { get; set; }
        public int TestDefinitionID { get; set; }
        public int Resolution_ToolTopLevelDefinitionID { get; set; }
        public double ValueEnduranceUp { get; set; }
        public double ValueEnduranceDown { get; set; }
    }
}