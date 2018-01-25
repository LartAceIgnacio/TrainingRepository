using System;
using BlastAsia.DigiBook.Domain.Models.Luigis;

namespace BlastAsia.DigiBook.Domain.Luigis
{
    public class LuigiService : ILuigiService
    {
        private ILuigiRepository luigiRepository;

        public LuigiService(ILuigiRepository luigiRepository)
        {
            this.luigiRepository = luigiRepository;
        }

        public Luigi Save(Guid id, Luigi luigi)
        {
            if (string.IsNullOrEmpty(luigi.FirstName))
            {
                throw new FirstNameRequired("Firstname is required");
            }

            Luigi result = null;
            var found = luigiRepository
                .Retrieve(luigi.LuigiId);

            if(found == null)
            {
                result = luigiRepository.Create(luigi);
            }
            else
            {
                result = luigiRepository
                    .Update(luigi.LuigiId, luigi);
            }
            return result;

        }
    }
}