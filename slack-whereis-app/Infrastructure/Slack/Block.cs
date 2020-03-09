using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public abstract partial class Block
    {
        [JsonProperty("type")]
        public virtual string Type { get; }

        public static List<Block> FromJson(string json) => JsonConvert.DeserializeObject<List<Block>>(json, Converter.Settings);
    }
}
