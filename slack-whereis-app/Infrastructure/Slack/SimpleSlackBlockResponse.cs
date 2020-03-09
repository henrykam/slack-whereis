using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{


    public class SimpleSlackBlockResponse
    {
        private List<Block> blocks = new List<Block>();

        [JsonProperty("blocks")]
        public IEnumerable<Block> Blocks { get => blocks; }

        public SimpleSlackBlockResponse AddBlock(Block block)
        {
            blocks.Add(block);
            return this;
        }

        public void RemoveBlock(Block block)
        {
            blocks.Remove(block);
            return;
        }

        public SimpleSlackBlockResponse Append(SimpleSlackBlockResponse res)
        {
            blocks.AddRange(res.Blocks);
            return this;
        }
    }
  
    //public partial class Option
    //{
    //    [JsonProperty("text")]
    //    public TextObject Text { get; set; }

    //    [JsonProperty("value")]
    //    public string Value { get; set; }
    //}

    //public partial class TextObject
    //{
    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("text")]
    //    public string Text { get; set; }

    //    [JsonProperty("emoji", NullValueHandling = NullValueHandling.Ignore)]
    //    public bool? Emoji { get; set; }
    //}

    public static class Serialize
    {
        public static string ToJson(this List<Block> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Block);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
