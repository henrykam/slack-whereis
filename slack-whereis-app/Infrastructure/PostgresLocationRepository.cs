using HenryKam.SlackWhereIs.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using HenryKam.SlackWhereIs.Model;

namespace HenryKam.SlackWhereIs.Infrastructure
{
    public class PostgresLocationRepository : ILocationRepository
    {
        private LocationDbContext _dbContext { get; set; }
        public PostgresLocationRepository(LocationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Location> GetLocationByName(string name)
        {
            if(name == null)
            {
                return null;
            }
            return _dbContext.Location.Where(n => n.Name.ToLower().StartsWith(name.ToLower()));
        }

        public Location GetLocationById(string id)
        {
            if (id == null)
            {
                return null;
            }
            return _dbContext.Location.FirstOrDefault(n => n.Id.ToString() == id.ToLower());
        }

        public IEnumerable<Location> GetLocations()
        {
            return _dbContext.Location;
        }

        public string AddLocation(Location location)
        {
            var i = _dbContext.Add(location);
            _dbContext.SaveChanges();
            return i.Entity.Id.ToString();
        }

        public string UpdateLocation(Location location)
        {
            var target = _dbContext.Location.FirstOrDefault(f => f.Id == location.Id);
            if (target != null)
            {
                var val = _dbContext.Update(target);
                _dbContext.SaveChanges();
                return target.Id.ToString();
            }
            return "Id not found";
        }

        public string DeleteLocation(string id)
        {
            var target = _dbContext.Location.FirstOrDefault(f => f.Id.ToString() == id);
            if (target != null) {
                var val = _dbContext.Remove(target);
                _dbContext.SaveChanges();
                return target.Id.ToString();
            }
            return "Id not found";
        }
    }
}
