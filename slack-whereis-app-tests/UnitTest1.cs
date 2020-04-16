using HenryKam.SlackWhereIs.Infrastructure.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using HenryKam.SlackWhereIs.Infrastructure;
using HenryKam.SlackWhereIs.Infrastructure.Exchange;
using HenryKam.SlackWhereIs.Model;

namespace slack_whereis_app_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {


            var s = SlackResponseFactory.CreateWhereIsResponse("Robson", "Vancouver Office", new Uri("https://slack-whereis.s3.ca-central-1.amazonaws.com/maps/saturna.png"), "Meeting Room", UseStatus.Available, "Available until 5:00 PM", floor: "16th Floor");

            var j = JsonConvert.SerializeObject(s);

            ExchangeProvider p = new ExchangeProvider(new ExchangeConfig() { Server = "https://exchange.absolute.com/ews/exchange.asmx", Domain = "absolute.com", Username = "SlackExchCalSA", Password = "ET_>L)z5Sp-d@u" });
            bool isAvailable = p.GetAvailability("hkam@absolute.com", out string details);




        }
    }
}
