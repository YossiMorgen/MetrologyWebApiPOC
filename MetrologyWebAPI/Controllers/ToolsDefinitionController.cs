using IgalDAL;
using MetrologyWebAPI.Data;
using MetrologyWebAPI.Models;
using MetrologyWebAPI.Models.ToolsDefinition;
using Newtonsoft.Json.Linq;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Cors;
using static System.Collections.Specialized.BitVector32;

namespace MetrologyWebAPI.Controllers
{
    public class ToolsDefinitionController : ApiController
    {
        // GET: api/ToolsDefinition
        public DTOToolsDefinition Get()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string log = "";
            //1 all tools tables and models

            var measunits = DAL.select<MeasurementUnit>("SELECT * FROM MeasurementUnit order by Symbol DESC");
            log += " measurementunits - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var techs = DAL.select<Technology>("SELECT * FROM Technology order by MCode");
            log += " technology - " + sw.ElapsedMilliseconds.ToString() + " ms ;";
            
            var subtech2s = DAL.select<SubTechnology>("SELECT * FROM subtechnology order by MCode");
            log += " subtechnology - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var isopros = DAL.select<IsoProcedure>("SELECT * FROM ISOProcedure");
            log += " isoprocedure - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toptools = DAL.select<ToolTopLevelDefinition>("SELECT * FROM ToolTopLevelDefinition");
            log += " tooltopleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toolfamlevs = DAL.select<ToolFamilyLevelDefinition>("SELECT * FROM ToolFamilyLevelDefinition");
            log += " toolfamilyleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toolmeaslevs = DAL.select<ToolMeasurementLevelDefinition>("SELECT * FROM ToolMeasurementLevelDefinition order by MCode");
            log += " toolmeasurementleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toollowlevs = DAL.select<ToolLowLevelDefinition>("SELECT * FROM ToolLowLevelDefinition order by MCode");
            log += " toollowleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var resos = DAL.select<Resolution>("SELECT * FROM Resolution");
            log += " resolution - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var resotop = DAL.select<Resolution_ToolTopLevelDefinition>("SELECT * FROM Resolution_ToolTopLevelDefinition order by ResolutionID");
            log += " resolution_tooltopleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var testdefgroups = DAL.select<TestDefinitionGroup>("SELECT * FROM TestDefinitionGroup order by TestDefinitionGroupName");
            log += " testdefinitiongroup - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var testdefs = DAL.select<TestDefinition>("SELECT * FROM TestDefinition order by TestDefinitionName");
            log += " testdefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var endurances = DAL.select<Endurance>("SELECT * FROM Endurance");
            log += " endurance - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var testtemplates = DAL.select<TestTemplate>("SELECT * FROM TestTemplate order by TestTemplateName");
            log += " testtemplate - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var testtemplatesdefs = DAL.select<TestTemplatesDefinition>("SELECT * FROM TestTemplateDefinition");
            log += " testtemplatesdefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";




            sw.Stop();

            //2 create big DTO
            DTOToolsDefinition dto = new DTOToolsDefinition()
            {
                innerComment = log,
                MeasurementUnits = measunits,
                Technologies = techs,
                SubTechnologies = subtech2s,
                IsoProcedures = isopros,
                ToolTopLevelDefinitions = toptools,
                ToolFamilyLevelDefinitions = toolfamlevs,
                ToolMeasurementLevelDefinitions = toolmeaslevs,
                ToolLowLevelDefinitions = toollowlevs,
                Resolutions = resos,
                Resolution_ToolTopLevelDefinitions = resotop,
                TestDefinitionGroups = testdefgroups,
                TestDefinitions = testdefs,
                Endurance = endurances,
                TestTemplates = testtemplates,
                TestTemplatesDefinitions = testtemplatesdefs

            };
            //3 return

            return dto;
        }

        // POST: api/ToolsDefinition
        public int Post(PostToolDefinition def)
        {
            System.Type className = Type.GetType("MetrologyWebAPI.Models.ToolsDefinition." + def.toolDefinitionName);
            object payloadAuto = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject(className);

            int id = DAL.insert(def.toolDefinitionName, payloadAuto);
            return id;

        }


        // PUT: api/ToolsDefinition/5
        public void Put(int id, PostToolDefinition def)
        {
            System.Type className = Type.GetType("MetrologyWebAPI.Models.ToolsDefinition." + def.toolDefinitionName);
            object payloadAuto = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject(className);

            DAL.update(def.toolDefinitionName, payloadAuto, id);
        }

        // DELETE: api/ToolsDefinition/5/model
        // DELETE: api/ToolsDefinition/5?model=shuki
        public void Delete(int id, [FromUri]string model)
        {
            DAL.Delete(model, id);
        }

        // POST: api/ToolsDefinition/ToolTopLevelDefinition
        [Route("api/ToolsDefinition/ToolTopLevelDefinition")]
        public PostToolTopLevelDefinitionResult PostToolTopLevelDefinition(PostToolTopLevelDefinition def)
        {

            if(def.ToolTopLevelDefinition.ToolTopLevelDefinitionID == 0)
            {
                int id = def.ToolTopLevelDefinition.ToolTopLevelDefinitionID = DAL.insert("ToolTopLevelDefinition", def.ToolTopLevelDefinition);
                def.ToolTopLevelDefinition.ToolTopLevelDefinitionID = id;
                def.IsoProcedure.ToolTopLevelDefinitionID = id;
                def.IsoProcedure.IsoProcedureID = DAL.insert("ISOProcedure", def.IsoProcedure);
            } else
            {
                DAL.update("ToolTopLevelDefinition", def.ToolTopLevelDefinition, def.ToolTopLevelDefinition.ToolTopLevelDefinitionID);
                DAL.update("ISOProcedure", def.IsoProcedure, def.IsoProcedure.IsoProcedureID);
            }
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@id", def.ToolTopLevelDefinition.ToolTopLevelDefinitionID);
            param[1] = new SqlParameter("@Values", def.SValues);
            string sCon= DAL.ConStr;

            DataSet dsTest = SqlDAC.ExecuteDataset
               (sCon, CommandType.StoredProcedure, "uspSetResolutionRelations", param);

            if (dsTest.Tables.Count == 0)
            {
                throw new Exception("No data found");
            } 

            DataTable Resolutions = dsTest.Tables[0];
            DataTable Resolution_ToolTopLevelDefinitions = dsTest.Tables[1];

            PostToolTopLevelDefinitionResult result = new PostToolTopLevelDefinitionResult()
            {
                IsoProcedure = def.IsoProcedure,
                ToolTopLevelDefinition = def.ToolTopLevelDefinition,
                Resolutions = Resolutions,
                Resolution_ToolTopLevelDefinitions = Resolution_ToolTopLevelDefinitions
            };

            return result;
        }

        // POST: api/ToolsDefinition/TestDefinition
        [Route("api/ToolsDefinition/TestDefinition")]
        public PostTestDefinition PostTestDefinition(PostTestDefinition def)
        {
            if(def.TestDefinition.TestDefinitionID == 0)
            {
                def.TestDefinition.TestDefinitionID = DAL.insert("TestDefinition", def.TestDefinition);
            } else
            {
                DAL.update("TestDefinition", def.TestDefinition, def.TestDefinition.TestDefinitionID);
                DAL.Delete("DELETE FROM Endurance WHERE TestDefinitionID = " + def.TestDefinition.TestDefinitionID.ToString());
            }


            foreach (Endurance e in def.Endurance)
            {
                e.TestDefinitionID = def.TestDefinition.TestDefinitionID;
                int eId = DAL.insert("Endurance", e);
                e.EnduranceID = eId;
            }

            return def;
        }

        //POST api/ToolsDefinition/TestTemplate
        [Route("api/ToolsDefinition/TestTemplate")]
        public PostTestTemplate PostTestTemplate(PostTestTemplate def)
        {

            if(def.TestTemplate.TestTemplateID == 0)
            {
                def.TestTemplate.TestTemplateID = DAL.insert("TestTemplate", def.TestTemplate);
            }
            else
            {
                DAL.update("TestTemplate", def.TestTemplate, def.TestTemplate.TestTemplateID);
            }

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@TestTemplateID", def.TestTemplate.TestTemplateID);
            param[1] = new SqlParameter("@TestDefinitionsIDs", def.TestDefinitionsIDs);
            string sCon = DAL.ConStr;

            DataSet dsTest = SqlDAC.ExecuteDataset
               (sCon, CommandType.StoredProcedure, "uspTestTemplateDefinitions", param);


            def.TestTemplatesDefinitions = dsTest.Tables[0];

            return def;
        }

        // POST: api/ToolsDefinition/Tavrig
        [Route("api/ToolsDefinition/Tavrig")]
        public PostTavrig PostTavrig(PostTavrig def)
        {
            
            if(def.ToolLowLevelDefinition.ToolLowLevelDefinitionID == 0)
            {
                def.ToolLowLevelDefinition.ToolLowLevelDefinitionID = DAL.insert("ToolFamilyLevelDefinition", def.ToolLowLevelDefinition);

            } else
            {
                DAL.update("ToolLowLevelDefinition", def.ToolLowLevelDefinition, def.ToolLowLevelDefinition.ToolLowLevelDefinitionID);
            }



            foreach (PostGroupAndTestDefAndEndurance g in def.PostGroupAndTestDefAndEndurance)
            {
                if (g.TestDefinitionGroup.TestDefinitionGroupID == 0)
                {
                    g.TestDefinitionGroup.TestDefinitionGroupID = DAL.insert("TestDefinitionGroup", g.TestDefinitionGroup);
                }
                else
                {
                    DAL.update("TestDefinitionGroup", g.TestDefinitionGroup, g.TestDefinitionGroup.TestDefinitionGroupID);
                }

                if (g.TestTemplate.TestTemplateID != 0)
                {
                    DAL.update("TestTemplate", g.TestTemplate, g.TestTemplate.TestTemplateID);
                }
                else
                {
                    TestTemplate tt = new TestTemplate()
                    {
                        TestTemplateName = "Tavrig",
                        ToolLowLevelDefinitionID = def.ToolLowLevelDefinition.ToolLowLevelDefinitionID
                    };
                    tt.TestTemplateID = DAL.insert("TestTemplate", tt);
                    g.TestTemplate = tt;
                }

                bool isPost = (g.TestDefinition.TestDefinitionID == 0);
                if (isPost)
                {
                    g.TestDefinition.TestDefinitionGroupID = g.TestDefinitionGroup.TestDefinitionGroupID;
                    g.TestDefinition.TestDefinitionID = DAL.insert("TestDefinition", g.TestDefinition);
                }
                else
                {
                    DAL.update("TestDefinition", g.TestDefinition, g.TestDefinition.TestDefinitionID);
                }

                if (g.Endurance.EnduranceID == 0)
                {
                    g.Endurance.TestDefinitionID = g.TestDefinition.TestDefinitionID;
                    g.Endurance.EnduranceID = DAL.insert("Endurance", g.Endurance);
                }
                else
                {
                    DAL.update("Endurance", g.Endurance, g.Endurance.EnduranceID);
                }

                // testTemplatedefinition
                if (isPost)
                {
                    TestTemplatesDefinition
                        ttd = new TestTemplatesDefinition()
                        {
                            TestDefinitionID = g.TestDefinition.TestDefinitionID,
                            TestTemplateID = g.TestTemplate.TestTemplateID
                        };
                    DAL.insert("TestTemplateDefinition", ttd);
                }

            }

            return def;
        }

        // DELETE: api/Test/TestDefinition/5
        [Route("api/ToolsDefinition/TestDefinition/{id}")]
        public void DeleteTestDefinition(int id)
        {
            DAL.Delete("TestDefinition", id);
            DAL.Delete("DELETE FROM Endurance WHERE TestDefinitionID = " + id.ToString());
        }

        // DELETE: api/Test/TestDefinitionGroup/5
        [Route("api/ToolsDefinition/TestDefinitionGroup/{id}")]
        public void DeleteTestDefinitionGroup(int id)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TestDefinitionGroupIDToDelete", id);
            string sCon = DAL.ConStr;

            // use uspDeleteTestDefinitionGroup stored procedure to delete the TestDefinitionGroup
            SqlDAC.ExecuteNonQuery(sCon, CommandType.StoredProcedure, "uspDeleteTestDefinitionGroup", param);
        }
    }
}
