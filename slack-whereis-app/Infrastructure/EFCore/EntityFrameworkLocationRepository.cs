using HenryKam.SlackWhereIs.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Infrastructure.EFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore
{
    public class EntityFrameworkLocationRepository : ILocationRepository, IDisposable
    {
        private SlackWhereIsDbContext _dbContext { get; set; }
        public EntityFrameworkLocationRepository(SlackWhereIsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<SlackWhereIs.Model.Location> GetLocationByName(string name)
        {
            if(name == null)
            {
                return null;
            }
            return _dbContext.Location.Include(i => i.LocationTags).ThenInclude(i => i.Tag).Where(n => n.Name.ToLower().StartsWith(name.ToLower())).Select(s=>s.ToModel());
        }

        public IEnumerable<SlackWhereIs.Model.Location> GetLocationByTag(string tag)
        {
            if (tag == null)
            {
                return null;
            }
            //return _dbContext.Tag.Include(i=>i.LocationTags).ThenInclude(i=>i.Location).Where(w => w.Value == tag).SelectMany(sm => sm.LocationTags.Select(s => s.Location.ToModel()));
            return _dbContext.Location.Include(i => i.LocationTags).ThenInclude(i => i.Tag).Where(w => w.LocationTags.Any(a => a.Tag.Value == tag))?.Select(s=>s.ToModel());
        }
               
        public SlackWhereIs.Model.Location GetLocationById(string id)
        {
            if (id == null)
            {
                return null;
            }
            return _dbContext.Location.Include(i => i.LocationTags).ThenInclude(i => i.Tag).FirstOrDefault(n => n.Id.ToString() == id.ToLower())?.ToModel();
        }

        public IEnumerable<SlackWhereIs.Model.Location> GetLocations()
        {
            return _dbContext.Location.Include(i=>i.LocationTags).ThenInclude(i=>i.Tag).Select(s=>s.ToModel());
        }

        public string AddLocation<T>(T location) where T : SlackWhereIs.Model.Location
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // insert new tags
                //var newTags = location.Tags.Where(q => !_dbContext.Tag.Any(a => a.Value == q));
                var dto = location.

                var i = _dbContext.Add(location);
                _dbContext.SaveChanges();               
                transaction.Commit();
                return i.Entity.Id.ToString();
            }
        }

        public string UpdateLocation(SlackWhereIs.Model.Location location)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var existing = _dbContext.Location.Find(location.Id);
                _dbContext.Entry(existing).CurrentValues.SetValues(location);
                //_dbContext.Location.Update(location);
                _dbContext.SaveChanges();

                transaction.Commit();
                return location.Id.ToString();
            }
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

        public void Dispose()
        {
            _dbContext.Dispose();
            
        }
    }
}
