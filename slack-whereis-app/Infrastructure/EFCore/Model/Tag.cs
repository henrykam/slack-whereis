using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore.Model
{
    public class Tag : IEquatable<Tag>, IEquatable<string>
    {
        [Key]
        public long Id { get; set; }
        public string Value { get; set; }
        
        public IEnumerable<LocationTag> LocationTags { get;set;}

        public bool Equals([AllowNull] Tag other)
        {
            return Equals(Id, other.Id);
        }

        public bool Equals([AllowNull] string other)
        {
            return Value == other;
        }
    }
}
