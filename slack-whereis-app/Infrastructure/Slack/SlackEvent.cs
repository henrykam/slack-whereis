using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure
{
    public class SlackEvent
    {
        public string Type { get; set; }
        public string Token { get; set; }
        public string Challenge { get; set; }

    }
}
