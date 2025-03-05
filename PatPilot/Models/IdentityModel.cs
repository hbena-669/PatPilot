using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatPilot.Models;

namespace PatPilot.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? EnseigneId { get; set; }

        public Enseigne? Enseigne { get; set; }
    }

}
