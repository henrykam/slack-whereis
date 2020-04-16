using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Model
{
    public class Location
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Office { get; set; }

        public string Floor { get; set; }

        public string Description { get; set; }

        public string MapImageUrl { get; set; }

        public string LocationImageUrl { get; set; }

        public virtual IEnumerable<string> Tags { get; set; }
    }
}
