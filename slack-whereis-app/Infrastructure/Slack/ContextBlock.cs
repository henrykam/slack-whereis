using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public class ContextBlock : Block
    {
        private List<ContextElement> elements = new List<ContextElement>();

        [JsonProperty("type")]
        public override string Type { get; } = "context";

        [JsonProperty("elements")]
        public IEnumerable<ContextElement> Elements { get => elements; }

        public ContextBlock AddElement(ContextElement contextElement)
        {
            elements.Add(contextElement);
            return this;
        }
    }

    public abstract partial class ContextElement
    {
        [JsonProperty("type")]
        public virtual string Type { get; }
    }

    public partial class MarkdownContextElement : ContextElement
    {
        public MarkdownContextElement(string text)
        {
            Text = text;
        }

        [JsonProperty("type")]
        public override string Type { get; } = "mrkdwn";

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class ImageContextElement : ContextElement
    {
        public ImageContextElement(Uri imageUrl, string altText)
        {
            AltText = altText;
            ImageUrl = imageUrl;
        }

        [JsonProperty("type")]
        public override string Type { get; } = "image";

        [JsonProperty("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("alt_text")]
        public string AltText { get; set; }
    }
}
