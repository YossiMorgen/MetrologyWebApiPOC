using MetrologyWebAPI.Data;
using MetrologyWebAPI.Models;
using MetrologyWebAPI.Models.ToolsDefinition;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MetrologyWebAPI.Controllers
{
    public class ToolsDefinitionController : ApiController
    {
        // GET: api/Test
        public DTOToolsDefinition Get()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string log = "";
            //1 all tools tables and models
            var techs = DAL.select<Technology>("SELECT * FROM Technology");
            log += " technology - " + sw.ElapsedMilliseconds.ToString() + " ms ;";
            var subtech2s = DAL.select<SubTechnology>("SELECT * FROM subtechnology");
            log += " subtechnology - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var isopros = DAL.select<IsoProcedure>("SELECT * FROM ISOProcedure");
            log += " isoprocedure - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toptools = DAL.select<ToolTopLevelDefinition>("SELECT * FROM ToolTopLevelDefinition");
            log += " tooltopleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var measunits = DAL.select<MeasurementUnits>("SELECT * FROM MeasurementUnits");
            log += " measurementunits - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toolmeaslevs = DAL.select<ToolMeasurementLevelDefinition>("SELECT * FROM ToolMeasurementLevelDefinition");
            log += " toolmeasurementleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            var toollowlevs = DAL.select<ToolLowLevelDefinition>("SELECT * FROM ToolLowLevelDefinition");
            log += " toollowleveldefinition - " + sw.ElapsedMilliseconds.ToString() + " ms ;";

            sw.Stop();

            //2 create big DTO
            DTOToolsDefinition dto = new DTOToolsDefinition()
            {
                innerComment = log,
                Technologies = techs,
                SubTechnologies = subtech2s,
                IsoProcedures = isopros,
                ToolTopLevelDefinitions = toptools,
                MeasurementUnits = measunits,
                ToolMeasurementLevelDefinition = toolmeaslevs,
                ToolLowLevelDefinition = toollowlevs
            };
            //3 return

            return dto;
        }

        // GET: api/Test/5
        // public string Get(int id)
        // {
        //    return "value";
        // }

        // POST: api/Test
        public int Post(PostToolDefinition def)
        {
            System.Type className = Type.GetType("MetrologyWebAPI.Models.ToolsDefinition." + def.toolDefinitionName);
            object payloadAuto = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject(className);

            int id = DAL.insert(def.toolDefinitionName, payloadAuto);
            return id;
            
            /*    let body = {
      toolDefinitionName : cname,
      payload : toolDef
    }
    const observable = this.http.post(this.appConfig.toolDefinitionURL, body);*/

            object t = null; 
            /*try
            {
                //Technology t = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject<Technology>();
                t = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject<Technology>();
                //DAL.insert<Technology>(t);
            }
            catch (Exception){}*/

            //try { t = ((JObject)def.payload).ToObject<SubTechnology>(); } catch (Exception) { }

            switch (def.toolDefinitionName)
            {
                case "Technology":
                    //Technology t = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject<Technology>();
                    t = ((JObject)def.payload).ToObject<Technology>();
                    //DAL.insert<Technology>(t);
                    break;
                default:
                    //alert("we hace a hacker !! no such type");
                    break;
            }

            DAL.insert(t.GetType().Name, t);



            SubTechnology st = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject<SubTechnology>();

            // return "ok";

        }

        // PUT: api/Test/5
        public void Put(int id, PostToolDefinition def)
        {
            System.Type className = Type.GetType("MetrologyWebAPI.Models.ToolsDefinition." + def.toolDefinitionName);
            object payloadAuto = ((Newtonsoft.Json.Linq.JObject)def.payload).ToObject(className);

            DAL.update(def.toolDefinitionName, payloadAuto, id);
        }

        // DELETE: api/Test/5
        public void Delete(int id, [FromUri]string model)
        {
            DAL.Delete(model, id);
        }
    }
}
