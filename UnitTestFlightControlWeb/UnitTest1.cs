using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTestFlightControlWeb
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            FlightPlanController fp = new FlightPlanController();
            // read a 100 flight plan from the json file
            string json = File.ReadAllText("./FlightplanMultiple (100).json");
            List<FlightPlan> flightsPlan = JsonConvert.DeserializeObject<List<FlightPlan>>(json);
            // send all the flight plan to the controller
            for (int i = 0; i < flightsPlan.Count; i++)
            {
                fp.Post(flightsPlan[i]);
            }
            // for all plight plan, we sent get reqest to the controller with the id and check if we gat the same flight plan
            for (int i = 0; i < flightsPlan.Count; i++)
            {
                string name = flightsPlan[i].Company_name + i;

                JsonResult temp = fp.Get(name);
                FlightPlan answer = (FlightPlan)temp.Value;
                // if the flight plan is't not the same, the test is fail.
                if (!isEqualFP(answer, flightsPlan[i]))
                {
                    Assert.Fail();
                }

            }


        }

        bool isEqualFP(FlightPlan one, FlightPlan two)
        {

            if (!one.Company_name.Equals(two.Company_name))
            {
                return false;
            }
            if (one.Initial_location.Latitude != two.Initial_location.Latitude) { return false; }
            if (one.Initial_location.Longitude != two.Initial_location.Longitude) { return false; }
            if (one.Initial_location.Date_time.Date != two.Initial_location.Date_time.Date) { return false; }
            if (one.Passengers != two.Passengers) { return false; }
            for (int i = 0; i < one.Segments.Length; i++)
            {
                if (one.Segments[i].Latitude != two.Segments[i].Latitude) { return false; }
                if (one.Segments[i].Longitude != two.Segments[i].Longitude) { return false; }
                if (one.Segments[i].Timespan_seconds != two.Segments[i].Timespan_seconds) { return false; }
            }
            return true;

        }

        [TestMethod]
        public void TestMethod2()
        {
            FlightPlanController fp = new FlightPlanController();
            // send a fake id, and if we get null from the controller is good.
            JsonResult temp = fp.Get("ffgk");
            Assert.IsNull(temp.Value);

        }
        [TestMethod]
        public void TestMethod3()
        {
            //create server controller
            serversController sc = new serversController();
            Server tar = new Server("i", "//localhost:4580");
            //send new server to the controller
            sc.PostNewServer(tar);
            //make fake server controller
            var serverControlerManger = new Mock<IServerController>();
            // setup the fake controller to return the excepted server
            serverControlerManger.Setup(x => x.GetServers()).Returns(new JsonResult( new List<Server>(1) { new Server("i", "//localhost:4580") }));
            // get all the server from the real controller
            JsonResult jr = sc.GetServers();
            string temp = JsonConvert.SerializeObject(jr.Value);
            List<Server> server = JsonConvert.DeserializeObject<List<Server>>(temp);
            Server check = server[0];
            // get from the fake controller all the servers
            JsonResult js =serverControlerManger.Object.GetServers();
            string temp1 = JsonConvert.SerializeObject(js.Value);
            List<Server> listOfServer = JsonConvert.DeserializeObject<List<Server>>(temp1);
            Server fromMock = listOfServer[0];
            // check if the servers is the same, if not ist throw exception.
            Assert.IsTrue(isSameServer(check, fromMock));
            

        }
        bool isSameServer(Server one, Server two)
        {
            if (!one.ServerId.Equals(two.ServerId))
            {
                return false;
            }
            if (!one.ServerURL.Equals(two.ServerURL))
            {
                return false;
            }
            return true;
        }
    }
}