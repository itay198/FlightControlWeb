using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;

namespace FlightControlWeb.Models
{
    
    public class ServerManager : IContainerManager<Server>
    {
        private ServerManager() { }
        private static ServerManager single = null;
        public static Dictionary<string, Server> dictionary = new Dictionary<string, Server>();
        public static ServerManager getServerManger() {
            if (single==null)
            {
                single = new ServerManager();
            }
            return single;
                }
        public static List<Server> container = new List<Server>();
        public void AddEllement(Server server)
        {
            container.Add(server);
        }

        public void DeleteEllement(string server_id)
        {
            Server del = container.Where(x => String.Compare(x.ServerId, server_id)==0).FirstOrDefault();
            if (del == null)
            {
                throw new Exception("Server not found");
            }
            container.Remove(del);

        }

        public IEnumerable<Server> GetAllEllements()
        {
            return container;
        }

        public Server GetEllement(string server_id)
        {
            return container.Where(x => String.Compare(x.ServerId , server_id)==0).FirstOrDefault();
        }

        public void UpdateEllement(Server server)
        {
            Server update = container.Where(x => x.ServerId == server.ServerId).FirstOrDefault();
            update.ServerURL = server.ServerURL;
        }
        public int GetSize()
        {
            return container.Count;
        }
    }
}
