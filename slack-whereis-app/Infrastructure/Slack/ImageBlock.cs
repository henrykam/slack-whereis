using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public class ImageBlock : Block
    {
        public ImageBlock(string title, Uri uri, string altText)
        {
            Title = new ImageTitle() { Text = title, Emoji = true };
            ImageUrl = uri;
            AltText = altText;
        }

        [JsonProperty("type")]
        public override string Type { get; } = "image";

        [JsonProperty("title")]
        public ImageTitle Title { get; set; }

        [JsonProperty("image_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageUrl { get; set; }

        [JsonProperty("alt_text", NullValueHandling = NullValueHandling.Ignore)]
        public string AltText { get; set; }
    }

    public class ImageTitle
    {
        [JsonProperty("type")]
        public string Type { get; } = "plain_text";

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("emoji")]
        public bool Emoji { get; set; }
    }

}
