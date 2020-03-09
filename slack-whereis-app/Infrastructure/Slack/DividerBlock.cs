using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public class DividerBlock : Block
    {
        [JsonProperty("type")]
        public override string Type { get; } = "divider";
    }
}
