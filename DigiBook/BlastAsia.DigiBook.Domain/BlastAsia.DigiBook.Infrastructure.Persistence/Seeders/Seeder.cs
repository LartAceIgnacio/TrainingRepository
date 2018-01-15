using BlastAsia.DigiBook.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Seeders
{
    public static class Seeder
    {
        public static void Seed(DigiBookDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            //if (!context.Employees.Any()) {
            //    CreateEmployees(context);
            //}

            //if (!context.Contacts.Any()) {
            //    CreateContacts(context);
            //}

            //if (!context.Appointments.Any()) {
            //    CreateAppointments(context);
            //}

            if (!context.Users.Any())
            {
                CreateUsers(context, userManager, roleManager)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private static async Task CreateUsers(DigiBookDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // local variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            string role_Administrator = "Administrator";
            string role_RegisteredUser = "RegisteredUser";

            //Create Roles (if they doesn't exist yet)
            if (!await roleManager.RoleExistsAsync(role_Administrator))
            {
                await roleManager.CreateAsync(new
                ApplicationRole(role_Administrator));
            }
            if (!await roleManager.RoleExistsAsync(role_RegisteredUser))
            {
                await roleManager.CreateAsync(new
                ApplicationRole(role_RegisteredUser));
            }
            // Create the "Admin" ApplicationUser account
            var user_Admin = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            // Insert "Admin" into the Database and assign the "Administrator" and "RegisteredUser" roles to him.
            if (await userManager.FindByNameAsync(user_Admin.UserName) == null)
            {
                await userManager.CreateAsync(user_Admin, "P@ss4Admin");
                await userManager.AddToRoleAsync(user_Admin,
                role_RegisteredUser);
                await userManager.AddToRoleAsync(user_Admin,
                role_Administrator);
                // Remove Lockout and E-Mail confirmation.
                user_Admin.EmailConfirmed = true;
                user_Admin.LockoutEnabled = false;
            }

#if DEBUG
            // Create some sample registered user accounts
            var user_Ryan = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var user_Solice = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Vodan = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            // Insert sample registered users into the Database and also assign the "Registered" role to him.
            if (await userManager.FindByNameAsync(user_Ryan.UserName) == null)
            {
                await userManager.CreateAsync(user_Ryan, "P@ss4Ryan");
                await userManager.AddToRoleAsync(user_Ryan, role_RegisteredUser);
                // Remove Lockout and E-Mail confirmation. 
                user_Ryan.EmailConfirmed = true;
                user_Ryan.LockoutEnabled = false;
            }
            if (await userManager.FindByNameAsync(user_Solice.UserName) == null)
            {
                await userManager.CreateAsync(user_Solice, "P@ss4Solice");
                await userManager.AddToRoleAsync(user_Solice, role_RegisteredUser);
                // Remove Lockout and E-Mail confirmation. 
                user_Solice.EmailConfirmed = true;
                user_Solice.LockoutEnabled = false;
            }
            if (await userManager.FindByNameAsync(user_Vodan.UserName) == null)
            {
                await userManager.CreateAsync(user_Vodan, "P@ss4Vodan");
                await userManager.AddToRoleAsync(user_Vodan, role_RegisteredUser);
                // Remove Lockout and E-Mail confirmation. 
                user_Vodan.EmailConfirmed = true;
                user_Vodan.LockoutEnabled = false;
            }
#endif
            await context.SaveChangesAsync();
        }

        //private static void CreateAppointments(DigiBookDbContext context)
        //{
        //    var billContact = context.Contacts.FirstOrDefault(
        //        c => c.FirstName == "Bill"
        //    );
        //    var larryContact = context.Contacts.FirstOrDefault(
        //        c => c.FirstName == "Larry"
        //    );
        //    var royEmployee = context.Employees.FirstOrDefault(
        //        e => e.EmailAddress == "rsaberon@blastasia.com"
        //    );
        //    var linusEmployee = context.Employees.FirstOrDefault(
        //        e => e.EmailAddress == "ltorvalds@linux.com"
        //    );

        //    context.Appointments.Add(
        //        new Appointment {
        //            AppointmentDate = DateTime.Parse("03/26/1974"),
        //            //DateCreated = DateTime.Today,
        //            //Guest = billContact,
        //            //Host = royEmployee

        //        }
        //    );
        //    context.Appointments.Add(
        //        new Appointment {
        //            AppointmentDate = DateTime.Parse("05/16/1974"),
        //            //DateCreated = DateTime.Today,
        //            //Guest = larryContact,
        //            //Host = linusEmployee

        //        }
        //    );
        //    context.SaveChanges();
        //}

        //private static void CreateContacts(DigiBookDbContext context)
        //{
        //    context.Contacts.Add(
        //        new Contact {
        //            FirstName = "Bill",
        //            LastName = "Gates",
        //            MobilePhone = "0922-7876533"
        //        }
        //    );

        //    context.Contacts.Add(
        //        new Contact {
        //            FirstName = "Larry",
        //            LastName = "Elieson",
        //            MobilePhone = "09079144456"
        //        }
        //    );

        //    context.SaveChanges();
        //}

        //private static void CreateEmployees(DigiBookDbContext context)
        //{
        //    context.Employees.Add(
        //        new Employee {
        //            FirstName = "Roy",
        //            LastName = "Saberon",
        //            EmailAddress = "rsaberon@blastasia.com",
        //            OfficePhone = "9144456"
        //        }
        //    );


        //    context.Employees.Add(
        //        new Employee {
        //            FirstName = "Linus",
        //            LastName = "Torvalds",
        //            EmailAddress = "ltorvalds@linux.com",
        //            OfficePhone = "999-8888"
        //        }
        //    );

        //    context.SaveChanges();
        //}
    }
}