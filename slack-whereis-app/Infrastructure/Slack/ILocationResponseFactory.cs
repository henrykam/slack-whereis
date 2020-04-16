using HenryKam.SlackWhereIs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public interface ILocationResponseFactory<in T> where T : Location
    {
        string CreateResponse(T location);
    }
}
