using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public class SectionBlock : Block
    {
        [JsonProperty("type")]
        public override string Type { get; } = "section";

        [JsonProperty("text")]
        public SectionBlockText Text { get; set; }

        [JsonProperty("accessory", NullValueHandling = NullValueHandling.Ignore)]
        public Accessory Accessory { get; set; }
    }

    public abstract partial class SectionBlockText
    {
        public SectionBlockText(string text)
        {
            Text = text;
        }

        [JsonProperty("type")]
        public virtual string Type { get; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class PlainSectionBlockText : SectionBlockText
    {
        public PlainSectionBlockText(string text, bool emoji) : base(text)
        {
            Emoji = emoji;
        }

        [JsonProperty("type")]
        public override string Type { get; } = "plain_text";

        [JsonProperty("emoji")]
        public bool Emoji { get; set; }
    }

    public class MarkdownSectionBlockText : SectionBlockText
    {
        public MarkdownSectionBlockText(string text) : base(text)
        {
        }

        [JsonProperty("type")]
        public override string Type { get; } = "mrkdwn";
    }

    public abstract partial class Accessory
    {
        [JsonProperty("type")]
        public virtual string Type { get; }
    }

    public class ImageAccessory : Accessory
    {
        public ImageAccessory(Uri uri, string altText)
        {
            ImageUrl = uri;
            AltText = altText;
        }

        [JsonProperty("type")]
        public override string Type { get; } = "image";

        [JsonProperty("image_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageUrl { get; set; }

        [JsonProperty("alt_text", NullValueHandling = NullValueHandling.Ignore)]
        public string AltText { get; set; }
    }

}
