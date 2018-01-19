using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Records;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ContactRepository
        : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IDigiBookDbContext context)
             : base(context)
        {
        }
        public Record<Contact> Fetch(int pageNo, int numRec, string filterValue)
        {
            Record<Contact> fetchResult = new Record<Contact>();
            if (string.IsNullOrEmpty(filterValue))
            {
                fetchResult.Result = context.Set<Contact>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Contact>().Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }
                return fetchResult;
            }
            else
            {
                fetchResult.Result = context.Set<Contact>().Where(x => x.FirstName.Contains(filterValue) || x.LastName.Contains(filterValue))
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Contact>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                        x.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }
                return fetchResult;
            }
        }
    }
}