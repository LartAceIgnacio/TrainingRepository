using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using System.Linq;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class ContactRepository
        : RepositoryBase<Contact>, IContactRepository
    {
        private readonly IDigiBookDbContext context;

        public ContactRepository(IDigiBookDbContext context)
            : base(context)
        {
            this.context = context;
        }
        public Pagination<Contact> Retrieve(int pageNumber, int recordNumber, string keyWord)
        {
            Pagination<Contact> result = new Pagination<Contact>
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = this.context.Set<Contact>().Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Contact>().Skip(0).Take(10).OrderBy(c => c.LastName).ToList();
                return result;
            }

            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Contact>().Skip(0).Take(10).OrderBy(c => c.LastName).ToList();
                return result;
            }

            if (string.IsNullOrEmpty(keyWord))
            {
                result.Result = this.context.Set<Contact>().OrderBy(c => c.LastName).Skip(pageNumber)
                                                  .Take(recordNumber)
                                                  .ToList();
                return result;
            }
            else
            {
                result.Result = this.context.Set<Contact>().Where(r => r.FirstName.Contains(keyWord) || r.LastName.Contains(keyWord))
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
