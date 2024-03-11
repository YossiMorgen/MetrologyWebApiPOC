using MetrologyWebAPI.Data;
using MetrologyWebAPI.Models.ToolsDefinition;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace MetrologyWebAPI.Controllers
{
    public class TavrigController : ApiController
    {
        private string diameterGoGroupName { get; set; }
        private string diameterNoGoGroupName { get; set; }
        private string PDGoGroupName { get; set; }
        private string PDNoGoGroupName { get; set; }

        public TavrigController()
        {
            diameterGoGroupName = ConfigurationManager.AppSettings["DiameterGoGroupName"];
            diameterNoGoGroupName = ConfigurationManager.AppSettings["DiameterNoGoGroupName"];
            PDGoGroupName = ConfigurationManager.AppSettings["PDGoGroupName"];
            PDNoGoGroupName = ConfigurationManager.AppSettings["PDNoGoGroupName"];
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //if no 4 groups , will create them
        [Route("api/Tavrig/TestGroups/{toptoolid}")]
        public TestDefinitionGroup[] Get4Groups(int toptoolid)
        {
            //check if groups exist
            TestDefinitionGroup[] groups = DAL.select<TestDefinitionGroup>(
                "SELECT * FROM TestDefinitionGroup WHERE ToolTopLevelDefinitionID = " + toptoolid);

            string[] gnames = new string[] { diameterGoGroupName, diameterNoGoGroupName, PDGoGroupName, PDNoGoGroupName };
            var existings = groups.Where(x => gnames.Contains(x.TestDefinitionGroupName));
            if (existings.Count() > 0)
            {
                return groups;
            }

            TestDefinitionGroup diameterGoGroup = new TestDefinitionGroup()
            {
                TestDefinitionGroupName = diameterGoGroupName,
                ToolTopLevelDefinitionID = toptoolid
            };
            diameterGoGroup.TestDefinitionGroupID = DAL.insert<TestDefinitionGroup>(diameterGoGroup);

            TestDefinitionGroup diameterNoGoGroup = new TestDefinitionGroup()
            {
                TestDefinitionGroupName = diameterNoGoGroupName,
                ToolTopLevelDefinitionID = toptoolid
            };
            diameterNoGoGroup.TestDefinitionGroupID = DAL.insert<TestDefinitionGroup>(diameterNoGoGroup);

            TestDefinitionGroup PDGoGroup = new TestDefinitionGroup()
            {
                TestDefinitionGroupName = PDGoGroupName,
                ToolTopLevelDefinitionID = toptoolid
            };
            PDGoGroup.TestDefinitionGroupID = DAL.insert<TestDefinitionGroup>(PDGoGroup);

            TestDefinitionGroup PDNoGoGroup = new TestDefinitionGroup()
            {
                TestDefinitionGroupName = PDNoGoGroupName,
                ToolTopLevelDefinitionID = toptoolid
            };
            PDNoGoGroup.TestDefinitionGroupID = DAL.insert<TestDefinitionGroup>(PDNoGoGroup);

            return new TestDefinitionGroup[] { diameterGoGroup, diameterNoGoGroup, PDGoGroup, PDNoGoGroup };
        }

        // GET api/<controller>/5
        // GET api/<controller>/5?toptoolid=1
        //id of lowleveltooldefinition
        [Route("api/Tavrig/{id}/{toptoolid}")]
        public DTOTavrig Get(int id, int toptoolid)
        {
            int lowToolId = id;
            var dto = new DTOTavrig() { ToolLowLevelDefinitionID = id };

            ToolLowLevelDefinition tool = DAL.select<ToolLowLevelDefinition>(
                "SELECT * FROM ToolLowLevelDefinition WHERE ToolLowLevelDefinitionID = " + id).First();

            dto.CovertOpsJsonTool = tool.CovertOpsJsonTool;

            TestTemplate[] templates = DAL.select<TestTemplate>(
                "SELECT * FROM TestTemplate WHERE ToolLowLevelDefinitionID = " + id);

            //will bring 4 test groups names
            TestDefinitionGroup[] testGroups = DAL.select<TestDefinitionGroup>(
                "SELECT * FROM TestDefinitionGroup WHERE ToolTopLevelDefinitionID = " + toptoolid);

            string templateIDs = string.Join(", ", templates.Select(x => x.TestTemplateID));
            TestTemplatesDefinition[] test_tempates = DAL.select<TestTemplatesDefinition>(
                "SELECT * FROM TestTemplatesDefinition WHERE TestTemplateID IN (" + templateIDs + ")");

            string testsIDs = string.Join(", ", test_tempates.Select(x => x.TestDefinitionID));
            TestDefinition[] tests = DAL.select<TestDefinition>(
                "SELECT * FROM TestDefinition WHERE TestDefinitionID IN (" + testsIDs + ")");

            Endurance[] endurances = DAL.select<Endurance>(
                "SELECT * FROM Endurance WHERE TestDefinitionID IN (" + testsIDs + ")");

            foreach (var g in testGroups)
            {
                switch (g.TestDefinitionGroupName)
                {
                    //case diameterGoGroupName:
                    case var value when value == diameterGoGroupName:
                        dto.diameterGo = tests.First(x => x.TestDefinitionGroupID == g.TestDefinitionGroupID);
                        dto.diameterGoEndurance = endurances.First(x => x.TestDefinitionID == dto.diameterGo.TestDefinitionID);
                        break;
                    case var value when value == diameterNoGoGroupName:
                        dto.diameterNoGo = tests.First(x => x.TestDefinitionGroupID == g.TestDefinitionGroupID);
                        dto.diameterNoGoEndurance = endurances.First(x => x.TestDefinitionID == dto.diameterNoGo.TestDefinitionID);
                        break;
                    case var value when value == PDGoGroupName:
                        dto.PDGo = tests.First(x => x.TestDefinitionGroupID == g.TestDefinitionGroupID);
                        dto.PDGoEndurance = endurances.First(x => x.TestDefinitionID == dto.PDGo.TestDefinitionID);
                        break;
                    case var value when value == PDNoGoGroupName:
                        dto.PDNoGo = tests.First(x => x.TestDefinitionGroupID == g.TestDefinitionGroupID);
                        dto.PDNoGoEndurance = endurances.First(x => x.TestDefinitionID == dto.PDNoGo.TestDefinitionID);
                        break;
                    default:
                        //send warn log
                        break;
                }
            }

            return dto;
        }

        // POST api/<controller>
        public bool Post([FromBody] DTOTavrig dto)
        {
            try
            {

                //test valid json
                if (string.IsNullOrEmpty(dto.CovertOpsJsonTool))
                {
                    throw new Exception("CovertOpsJsonTool is empty");
                }
                try
                {
                    JsonConvert.DeserializeObject(dto.CovertOpsJsonTool);
                }
                catch (Exception jsonEX)
                {
                    throw;
                }

                DAL.update("UPADTE ToolLowLevelDefinition SET CovertOpsJsonTool = '" + dto.CovertOpsJsonTool + "'");

                TestDefinitionGroup[] testGroups = DAL.select<TestDefinitionGroup>(
                    "SELECT * FROM TestDefinitionGroup WHERE ToolTopLevelDefinitionID = " + dto.ToolTopLevelDefinitionID);

                if (testGroups.Length != 4)
                {
                    //handle create groups
                    this.Get4Groups(dto.ToolTopLevelDefinitionID);
                }


                //int gDiameterGo = testGroups.First(x => x.TestDefinitionGroupName == diameterGoGroupName).TestDefinitionGroupID;
                //froups comes from ui
                dto.diameterGo.TestDefinitionID = DAL.insert<TestDefinition>(dto.diameterGo);
                dto.diameterNoGo.TestDefinitionID = DAL.insert<TestDefinition>(dto.diameterNoGo);
                dto.PDGo.TestDefinitionID = DAL.insert<TestDefinition>(dto.PDGo);
                dto.PDNoGo.TestDefinitionID = DAL.insert<TestDefinition>(dto.PDNoGo);

                dto.diameterGoEndurance.TestDefinitionID = dto.diameterGo.TestDefinitionID;
                DAL.insert<Endurance>(dto.diameterGoEndurance);

                dto.diameterNoGoEndurance.TestDefinitionID = dto.diameterNoGo.TestDefinitionID;
                DAL.insert<Endurance>(dto.diameterNoGoEndurance);

                dto.PDGoEndurance.TestDefinitionID = dto.PDGo.TestDefinitionID;
                DAL.insert<Endurance>(dto.PDGoEndurance);

                dto.PDNoGoEndurance.TestDefinitionID = dto.PDNoGo.TestDefinitionID;
                DAL.insert<Endurance>(dto.PDNoGoEndurance);

                string[] strings = new string[] { "diameterGo", "diameterNoGo", "PDGo", "PDNoGo" };
                foreach (var s in strings)
                {
                    TestTemplate testTemplate = new TestTemplate()
                    {
                        ToolLowLevelDefinitionID = dto.ToolLowLevelDefinitionID,
                        TestTemplateName = s,
                    };

                    TestTemplatesDefinition ttd = new TestTemplatesDefinition()
                    {
                        TestTemplateID = DAL.insert<TestTemplate>(testTemplate),
                        TestDefinitionID = Convert.ToInt32(dto.GetType().GetProperty(s).GetValue(dto))
                    };
                    DAL.insert<TestTemplatesDefinition>(ttd);
                }   

                /*TestTemplate testTemplate_diameterGo = new TestTemplate()
                {
                    ToolLowLevelDefinitionID = dto.ToolLowLevelDefinitionID,
                    TestTemplateName = "diameterGo",
                };
                TestTemplatesDefinition ttd_diameterGo = new TestTemplatesDefinition()
                {
                    TestTemplateID = DAL.insert<TestTemplate>(testTemplate_diameterGo),
                    TestDefinitionID = dto.diameterGo.TestDefinitionID
                };    
                DAL.insert<TestTemplatesDefinition>(ttd_diameterGo);*/

                return true;
            }
            catch (Exception ex)
            {
                //log error
                throw ex;
                //return false;
            }
        }

        // PUT api/<controller>/5
        public bool Put(int id, [FromBody] string value)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                //log
                return false;
                //throw;
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}