using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        private IDigiBookDbContext context;
        public ContactRepository(IDigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Pagination<Contact> Retrieve(int pageNumber, int recordNumber, string searchKey)
        {
            Pagination<Contact> result = new Pagination<Contact>()
            {
                PageNumber = pageNumber < 0 ? 1 : pageNumber,
                RecordNumber = recordNumber < 0 ? 1 : recordNumber,
                TotalCount = this.context.Set<Contact>().Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Contact>().OrderBy(c => c.LastName)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Contact>().OrderBy(c => c.LastName)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (string.IsNullOrEmpty(searchKey))
            {
                result.Result = this.context.Set<Contact>().OrderBy(c => c.LastName)
                   .Skip(pageNumber).Take(recordNumber).ToList();

                return result;
            }
            else
            {
                result.Result = this.context.Set<Contact>().Where(r => r.FirstName.Contains(searchKey) || r.LastName.Contains(searchKey))
                                                 .OrderBy(c => c.LastName)
                                                 .Skip(pageNumber)
                                                 .Take(recordNumber)
                                                 .ToList();

                result.TotalCount = result.Result.Count();

                return result;
            }
        }
    }
}
