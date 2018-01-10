using System;
using Microsoft.AspNetCore.Identity;

namespace EFTraining.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {

        }
        public ApplicationUser(string userName) : base (userName)
        {
            
        }
    }
}