using System;
using EFTraining.Data.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Data.Seeder
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
                CreateAppointments(context);
            }
        }

        private static void CreateAppointments(DigiBookDbContext context)
        {
            var maureenContact = context.Contacts.FirstOrDefault(
                c => c.Firstname == "Maureen"
            );
            var maeContact = context.Contacts.FirstOrDefault(
               c => c.Firstname == "Mae"
           );

            var olsenEmployee = context.Employees.FirstOrDefault(
               e => e.Firstname == "Olsen"
           );
            var renzEmployee = context.Employees.FirstOrDefault(
                e => e.Firstname == "Renz"
            );

            context.Appointments.Add(
                new Appointment
                {
                    AppointmentDate = DateTime.Parse("01/15/2018"),
                    DateCreated = DateTime.Today,
                    Guest = maureenContact,
                    Host = olsenEmployee
                }
            );

            context.Appointments.Add(
                new Appointment
                {
                    AppointmentDate = DateTime.Parse("01/15/2018"),
                    DateCreated = DateTime.Today,
                    Guest = maeContact,
                    Host = renzEmployee
                }
            );

            context.SaveChanges();
        }

        private static void CreateContacts(DigiBookDbContext context)
        {
            context.Contacts.Add(
                new Contact
                {
                    Firstname = "Maureen",
                    Lastname = "Sebastian",
                    Email = "maureen@gmail.com",
                    MobilePhone = "4848766"
                }
            );

            context.Contacts.Add(
                new Contact
                {
                    Firstname = "Mae",
                    Lastname = "Wong",
                    Email = "mae@gmail.com",
                    MobilePhone = "4848876"
                }
            );

            context.SaveChanges();
        }

        private static void CreateEmployees(DigiBookDbContext context)
        {
            context.Employees.Add(
                new Employee
                {
                    Firstname = "Olsen",
                    Lastname = "Mtaencio",
                    Email = "jhnkrl15@gmail.com",
                    OfficePhone = "4848766"
                }
            );

            context.Employees.Add(
                new Employee
                {
                    Firstname = "Renz",
                    Lastname = "Nebran",
                    Email = "Renz@gmail.com",
                    OfficePhone = "4848876"
                }
            );

            context.SaveChanges();
        }
    }
}