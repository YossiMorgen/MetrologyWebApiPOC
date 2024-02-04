using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend
{
    public class MeasurementSingle
    {
        public int MeasurementSingleID { get; set; }
        public int MeasurementSetID { get; set; }
        public MeasurementSet MeasurementSet { get; set; }
        public int ValueUnitID { get; set; }
        public MeasurementUnits Units { get; set; }
        public int ValueRequired { get; set; }
        public int ValueEnduranceUp { get; set; }
        public int ValueEnduranceDown { get; set; }
    }
}