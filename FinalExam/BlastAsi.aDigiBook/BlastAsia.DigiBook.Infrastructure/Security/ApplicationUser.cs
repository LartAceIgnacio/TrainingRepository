using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Security
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual List<Token> Tokens { get; set; }
        public string DisplayName { get; set; }
    }
}