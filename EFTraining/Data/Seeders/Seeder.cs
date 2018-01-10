using System;
using EFTraining.Data.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Data.Seeders
{
    public static class Seeder
    {
        public static void Seed(DigiBookDbContext context)
        {
            if (!context.Employees.Any())
            {
                CreateEmployees(context);
            }

            if (!context.Contacts.Any())
            {
                CreateContacts(context);
            }

            if (!context.Appointments.Any())
            {
                CreateAppointment(context);
            }
        }

        private static void CreateAppointment(DigiBookDbContext context)
        {
            var kimContact = context.Contacts.FirstOrDefault
            (
                c => c.FirstName == "Kim"
            );
            var kyrieContact = context.Contacts.FirstOrDefault
            (
                c => c.FirstName == "Kyrie"
            );

            var ememEmployee = context.Employees.FirstOrDefault
            (
                e => e.FirstName == "Emmanuel"
            );
            var kathEmployee = context.Employees.FirstOrDefault
            (
                e => e.FirstName == "Kathleen"
            );

            context.Appointments.Add
            (
                new Appointment
                {
                    AppointmentDate = DateTime.Today.AddDays(1),
                    DateCreated = DateTime.Today,
                    Guest = kimContact,
                    Host = ememEmployee
                }
            );

            context.Appointments.Add
            (
                new Appointment
                {
                    AppointmentDate = DateTime.Today.AddDays(2),
                    DateCreated = DateTime.Today,
                    Guest = kyrieContact,
                    Host = kathEmployee
                }
            );


            context.SaveChanges();
        }

        private static void CreateContacts(DigiBookDbContext context)
        {
            context.Contacts.Add(
                new Contact
                {
                    FirstName = "Kyrie",
                    LastName = "Irving",
                    Email = "kyrie@outlook.com",
                    MobilePhone = "543-321-155"
                }
            );
            context.Contacts.Add(
                new Contact
                {
                    FirstName = "Kim",
                    LastName = "Domingo",
                    Email = "Kimmie@outlook.com",
                    MobilePhone = "543-321-123"
                }
            );
            context.SaveChanges();
        }

        private static void CreateEmployees(DigiBookDbContext context)
        {
            context.Employees.Add(
                new Employee
                {
                    FirstName = "Emmanuel",
                    LastName = "Magadia",
                    Email = "emmanuelmagadia@outlook.com",
                    OfficePhone = "123-432-69"
                }
            );
            context.Employees.Add(
                new Employee
                {
                    FirstName = "Kathleen",
                    LastName = "Escala",
                    Email = "katalina@outlook.com",
                    OfficePhone = "123-321-213"
                }
            );

            context.SaveChanges();
        }
    }
}