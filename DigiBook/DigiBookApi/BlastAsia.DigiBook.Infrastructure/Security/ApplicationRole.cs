using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Security
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
          
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
