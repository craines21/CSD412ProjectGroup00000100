using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412ProjectGroup00000100.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Poll> Polls { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
