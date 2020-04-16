using HenryKam.SlackWhereIs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Application
{
    public partial interface ILocationRepository
    {
        IEnumerable<Location> GetLocationByName(string name);

        IEnumerable<Location> GetLocationByTag(string tag);

        Location GetLocationById(string id);

        IEnumerable<Location> GetLocations();

        string AddLocation<T>(T location) where T : Location;

        string UpdateLocation<T>(T location) where T : Location;

        string DeleteLocation(string id);
    }
}
