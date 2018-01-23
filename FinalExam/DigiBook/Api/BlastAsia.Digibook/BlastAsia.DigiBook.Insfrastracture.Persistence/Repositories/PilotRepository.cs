using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class PilotRepository
           : RepositoryBase<Pilot>, IPilotRepository
    {
        private readonly IDigiBookDbContext context;
        public PilotRepository(IDigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Pilot Retrieve(string code)
        {
            return this.context.Set<Pilot>().FirstOrDefault(i => i.PilotCode == code);
        }

        public IEnumerable<Pilot> Search(string key)
        {
            var result = this.context.Set<Pilot>()
                .Where(c => c.FirstName.Contains(key) || c.MiddleName.Contains(key) || c.LastName.Contains(key) || c.PilotCode.Contains(key))
                .ToList();
            return result;
        }
    }
}