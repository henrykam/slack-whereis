using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore.Model
{
    public abstract class LocationWithAvailability : Location
    {
        public string EmailAddress { get; set; }

    }


}
