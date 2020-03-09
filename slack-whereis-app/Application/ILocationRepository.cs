using HenryKam.SlackWhereIs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Application
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocationByName(string name);

        Location GetLocationById(string id);

        IEnumerable<Location> GetLocations();

        string AddLocation(Location location);

        string DeleteLocation(string id);
    }
}
