using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend
{
    public class TestTemplatesDefinitions
    {
        public int TestTemplatesDefinitionsID { get; set; }
        public int TestTemplateID { get; set; }
        public TestTemplate TestTemplate { get; set; }
        public int TestSetDefinitionID { get; set; }
        public TestSetDefinition TestSetDefinition { get; set; }
    }
}