using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore.Model
{
    public class LocationTag
    {
        public long LocationId { get; set; }
        public Location Location { get; set; }
        public long TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
