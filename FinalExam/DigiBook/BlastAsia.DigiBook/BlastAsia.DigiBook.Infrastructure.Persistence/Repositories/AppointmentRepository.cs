﻿using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models;
using System.Linq;
using System;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository
         : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IDigiBookDbContext context) : base(context)
        {

        }
        public Pagination<Appointment> Retrieve(int pageNo, int numRec, string filterValue)
        {
            Pagination<Appointment> result = new Pagination<Appointment>();
            //var c = Convert.ToDateTime(filterValue);
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Appointment>().OrderBy(x => x.AppointmentDate)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Appointment>().Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }
            }
            else
            {
                result.Results = context.Set<Appointment>()
                    .Where(x => x.Guest.FirstName.ToLower().Contains(filterValue.ToLower())
                     ||  x.Guest.LastName.ToLower().Contains(filterValue.ToLower())
                     ||  x.Host.FirstName.ToLower().Contains(filterValue.ToLower())
                     ||  x.Host.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.AppointmentDate)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Appointment>()
                     .Where(x => x.Guest.FirstName.ToLower().Contains(filterValue.ToLower())
                      || x.Guest.LastName.ToLower().Contains(filterValue.ToLower())
                      || x.Host.FirstName.ToLower().Contains(filterValue.ToLower())
                      || x.Host.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                    //.OrderBy(x => x.AppointmentDate).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }
            }

            return result;
        }
    }
}