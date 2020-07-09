﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Server
    {
        public string ServerId { get; set; }
        public string ServerURL { get; set; }
        public Server() { }
        public Server(string id, string url)
        {
            ServerId = id;
            ServerURL = url;
        }
    }
}
