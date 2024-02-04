using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend
{
    public class MeasurementSet
    {
        public int MeasurementSetID { get; set; }
        public int TestSetDefinitionID { get; set; }
        public TestSetDefinition TestSetDefinition { get; set; }
    }
}