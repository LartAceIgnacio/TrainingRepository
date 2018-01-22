using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        private IDigiBookDbContext context;
        public ContactRepository(IDigiBookDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public Pagination<Contact> Retrieve(int pageNumber, int recordNumber, string query)
        {
            Pagination<Contact> result = new Pagination<Contact>
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = this.context.Set<Contact>().Count()
            };

            if (pageNumber < 0)
            {
                result.Results = this.context.Set<Contact>()
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.LastName)
                    .ToList();
                return result;
            }
            if (recordNumber < 0)
            {
                result.Results = this.context.Set<Contact>()
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.LastName)
                    .ToList();
                return result;
            }
            if (string.IsNullOrEmpty(query))
            {
                result.Results = this.context.Set<Contact>()
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .OrderBy(c => c.LastName)
                    .ToList();
                return result;
            }
            else
            {
                result.Results = this.context.Set<Contact>()
                    .Where(c => c.FirstName.Contains(query) || c.LastName.Contains(query))
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .OrderBy(c => c.LastName)
                    .ToList();
                result.TotalCount = result.Results.Count();
                return result;
            }

        }

    }
}
