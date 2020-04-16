using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore.Model
{
    public class Department : Location
    {
        public override SlackWhereIs.Model.Location ToModel()
        {
            return new SlackWhereIs.Model.Department()
            {
                Id = Id,
                Description = Description,
                Floor = Floor,
                LocationImageUrl = LocationImageUrl,
                MapImageUrl = MapImageUrl,
                Name = Name,
                Office = Office,
                Tags = LocationTags?.Select(s => s.Tag.Value)
            };
        }
    }
}
