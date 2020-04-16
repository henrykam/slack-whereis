using HenryKam.SlackWhereIs.Infrastructure.Slack;
using HenryKam.SlackWhereIs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public static class SlackResponseFactory
    {

        public static SimpleSlackBlockResponse CreateWhereIsResponse(string name, string office, Uri mapUrl, string locationType = " ", UseStatus useStatus = UseStatus.None, string useStatusString = " ", Uri thumbUrl = null, string floor = " ")
        {
            if(thumbUrl == null)
            {
                thumbUrl = new Uri(@"https://api.slack.com/img/blocks/bkb_template_images/notifications.png");
            }

            ContextBlock useStatusBlock = new ContextBlock();
            if (useStatus == UseStatus.InUse)
            {
                useStatusBlock = useStatusBlock.AddElement(new MarkdownContextElement(":x: " +useStatusString));
            }
            else if(useStatus == UseStatus.Available)
            {
                useStatusBlock = useStatusBlock.AddElement(new MarkdownContextElement(":heavy_check_mark: "+useStatusString));
            }
            else
            {
                useStatusBlock = useStatusBlock.AddElement(new MarkdownContextElement(" "));
            }

            SimpleSlackBlockResponse r = new SimpleSlackBlockResponse();
            r.AddBlock(new DividerBlock())
                .AddBlock(new SectionBlock() { Text = new MarkdownSectionBlockText($"*{name}*\n{locationType}\n{floor}\n{office}"), Accessory = new ImageAccessory(thumbUrl, locationType) })
                .AddBlock(useStatusBlock)
                .AddBlock(new ImageBlock("Map", mapUrl, "Map"));

            return r;
        }
    }

}
