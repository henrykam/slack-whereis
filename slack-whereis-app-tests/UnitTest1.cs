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

            //ExchangeProvider p = new ExchangeProvider(new Uri("https://exchange.absolute.com/ews/exchange.asmx"), "absolute.com", "SlackExchCalSA", "ET_>L)z5Sp-d@u");
            //var rooms =  p.GetConfRoomsAddresses("Davie");
            //bool isAvailable = p.GetAvailability("BVAN-Davie@absolute.com", out string details);


        
        
        }
    }
}
