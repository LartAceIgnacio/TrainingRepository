using System;
using BlastAsia.DigiBook.Domain.Models.Names;

namespace BlastAsia.DigiBook.Domain.Names
{
    public class NameService : INameService
    {
        private readonly INameRepository nameRepository;
        public NameService(INameRepository nameRepository)
        {
            this.nameRepository = nameRepository;
        }

        public Name Save(Guid id, Name name)
        {
            if(string.IsNullOrEmpty(name.NameFirst))
            {
                throw new FirstNameRequiredException();
            }
            if (string.IsNullOrEmpty(name.NameLast))
            {
                throw new LastNameRequiredException();
            }
            Name result = null;
            var found = nameRepository.Retrieve(id);
            if(found == null)
            {
                result = nameRepository.Create(name);
            }
            else
            {
                result = nameRepository.Update(id, name);
            }
            
            return result;
        }
    }
}