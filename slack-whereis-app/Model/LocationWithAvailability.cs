using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Model
{
    public abstract class LocationWithAvailability : Location
    {
        public string EmailAddress { get; set; }
    }

    public enum UseStatus
    {
        None,
        InUse,
        Available
    }


}
