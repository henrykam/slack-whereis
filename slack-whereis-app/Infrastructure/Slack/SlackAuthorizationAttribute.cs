using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public class SlackAuthorizationAttribute : TypeFilterAttribute 
    {
        public SlackAuthorizationAttribute() : base(typeof(SlackAuthorizationActionFilter))
        {
            
        }
    }
}
