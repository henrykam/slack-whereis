using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore.Model
{
    public abstract class Location
    {
        [Key]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Office { get; set; }

        public string Floor { get; set; }

        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string MapImageUrl { get; set; }
        
        public string LocationImageUrl { get; set; }

        public IEnumerable<LocationTag> LocationTags { get; set; }

        public abstract SlackWhereIs.Model.Location ToModel();



    }


}
