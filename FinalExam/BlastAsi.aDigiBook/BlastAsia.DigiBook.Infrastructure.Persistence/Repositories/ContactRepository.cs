using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Linq;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ContactRepository
        : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IDigiBookDbContext context)
            : base(context)
        {
        }

        public PaginationResult<Contact> Retrieve(int pageNo, int numRec, string filterValue)
        {
            PaginationResult<Contact> result = new PaginationResult<Contact>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Contact>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Contact>().Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }
            }
            else
            {
                result.Results = context.Set<Contact>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                    x.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Contact>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                        x.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }
            }

            return result;
        }
    }
}
