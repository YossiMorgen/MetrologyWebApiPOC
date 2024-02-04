using MetrologyWebAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MetrologyWebAPI.Controllers
{
    public class TechnologyController : ApiController
    {
        // POST: api/Technology
       // public int Post([FromBody]string TechnoloyName)
        //{
            // insert one new technology and return its id
            //string sql = "INSERT INTO Technology (TechnologyName) VALUES ('" + TechnoloyName + "'); SELECT LAST_INSERT_ID();";
            // var id = DAL.insert(sql);
            // return id;

       // }

        // PUT: api/Technology/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Technology/5
        public void Delete(int id)
        {
        }
    }
}
