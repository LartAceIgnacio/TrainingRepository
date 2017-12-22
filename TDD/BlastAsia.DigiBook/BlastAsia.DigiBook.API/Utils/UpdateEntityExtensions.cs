using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class UpdateEntityExtensions
    {
        public static Contact ApplyChanges(this Contact contact, Contact from)
        {
            contact.Firstname = from.Firstname;
            contact.Lastname = from.Lastname;
            contact.CityAddress = from.CityAddress;
            contact.StreetAddress = from.StreetAddress;
            contact.EmailAddress = from.EmailAddress;
            contact.MobilePhone = from.MobilePhone;
            contact.ZipCode = from.ZipCode;
            
            return contact;
        }

        public static Employee ApplyChanges(this Employee employee, Employee from)
        {
            employee.Firstname = from.Firstname;
            employee.Lastname = from.Lastname;
            employee.EmailAddress = from.EmailAddress;
            employee.MobilePhone = from.MobilePhone;
            employee.Extension = from.Extension;
            employee.OfficePhone = employee.OfficePhone;
            //employee.Photo = employee.Photo;

            return employee;
        }

        public static Appointment ApplyChanges(this Appointment appointment, Appointment from)
        {
            appointment.AppointmentDate = from.AppointmentDate;
            appointment.StartTime = from.StartTime;
            appointment.EndTime = from.EndTime;
            appointment.GuestId = from.GuestId;
            appointment.HostId = from.HostId;
            appointment.IsCancelled = from.IsCancelled;
            appointment.IsDone = from.IsDone;
            appointment.Notes = from.Notes;

            return appointment;
        }

        public static Venue ApplyChanges(this Venue venue, Venue from)
        {
            venue.VenueName = from.VenueName;
            venue.Description = from.Description;

            return venue;
        }
    }
}
