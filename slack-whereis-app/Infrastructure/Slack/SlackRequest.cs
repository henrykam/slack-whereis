﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure
{
    public class SlackRequest
    {
        public string Token { get; set; }
        public string Team_Id { get; set; }
        public string Team_Domain { get; set; }
        public string Channel_Id { get; set; }
        public string Channel_Name { get; set; }
        public string User_Id { get; set; }
        public string User_Name { get; set; }
        public string Command { get; set; }

        [StringLength(100)]
        public string Text { get; set; }
        public string Response_Url { get; set; }
        public string Trigger_Id { get; set; }
    }
}
