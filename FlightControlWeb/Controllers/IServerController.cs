using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    public interface IServerController
    {
        public void IserversController();


        // Get: api/servers
        [HttpGet]
        public abstract JsonResult GetServers();
        [HttpPost]
        public abstract void PostNewServer([FromBody] Server server);
        [HttpDelete("{id}")]
        public abstract void DeleteServer(string id);
    }
}
