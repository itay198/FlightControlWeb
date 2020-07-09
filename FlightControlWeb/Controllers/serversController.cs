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
    [Route("api/[controller]")]
    [ApiController]
    public class serversController : ControllerBase,IServerController
    {
        ServerManager serverManager;
        public static Dictionary<string, Server> dictionary = new Dictionary<string, Server>();
       public serversController()
        {
            this.serverManager = ServerManager.getServerManger();
        }
        
        // Get: api/servers
        [HttpGet]
        public JsonResult GetServers()
        {
            JsonResult js = new JsonResult(serverManager.GetAllEllements());
            return js;
        }
        [HttpPost]
        public void PostNewServer([FromBody] Server server)
        {
            serverManager.AddEllement(server);
        }
        [HttpDelete ("{id}")]
        public void DeleteServer(string id)
        {
            serverManager.DeleteEllement(id);
        }

        public void IserversController()
        {
            throw new NotImplementedException();
        }
    }
}