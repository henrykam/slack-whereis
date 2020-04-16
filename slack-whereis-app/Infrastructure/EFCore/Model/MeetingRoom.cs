using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore.Model
{
    public class MeetingRoom : LocationWithAvailability
    {


        public override SlackWhereIs.Model.Location ToModel()
        {
            return new SlackWhereIs.Model.MeetingRoom()
            {
                Id = Id,
                Description = Description,
                EmailAddress = EmailAddress,
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
