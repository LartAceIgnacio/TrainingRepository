using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using BlastAsia.DigiBook.Domain.Models;
using Mapster;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Security;
using BlastAsia.DigiBook.API.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Route("api/User")]
    public class UserController : BaseApiController
    {
        #region Constructor
        public UserController(
            DigiBookDbContext context,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            )
            : base(context, roleManager, userManager, configuration)
        {
        }
        #endregion

        #region RESTful Conventions
        /// <summary>
        /// POST: api/user
        /// </summary>
        /// <returns>Creates a new User and return it accordingly.
        ///</ returns >
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // check if the Username/Email already exists
            ApplicationUser user = await
                    UserManager.FindByNameAsync(model.UserName);
            if (user != null) return BadRequest("Username already exists");

            user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null) return BadRequest("Email already exists.");

            var now = DateTime.Now;

            // create a new Item with the client-sent json data
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.DisplayName,
                CreatedDate = now,
                LastModifiedDate = now
            };
            // Add the user to the Db with the choosen password
            await UserManager.CreateAsync(user, model.Password);
            // Assign the user to the 'RegisteredUser' role.
            await UserManager.AddToRoleAsync(user, "RegisteredUser");
            // Remove Lockout and E-Mail confirmation
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            // persist the changes into the Database.
            DbContext.SaveChanges();
            // return the newly-created User to the client.
            return Json(user.Adapt<UserViewModel>(),
                JsonSettings);
        }
        #endregion
    }
}
