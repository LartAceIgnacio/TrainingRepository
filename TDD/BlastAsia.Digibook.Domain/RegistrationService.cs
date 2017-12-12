using System;
using System.Text.RegularExpressions;
using System.Linq;
using BlastAsia.Digibook.Domain.Models;

namespace BlastAsia.Digibook.Domain
{
    public class RegistrationService
    {
        private readonly IAccountRepository repository;

        public RegistrationService(IAccountRepository repository)
        {
            this.repository = repository;
        }
        private readonly int PasswordMinimumLength = 8;
        public bool Register(string username, string password)
        {
            Regex emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

            //Check business rules
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }

            if(password.Length < PasswordMinimumLength)
            {
                throw new MinimumLengthRequiredException();
            }

            if (!emailRegex.IsMatch(username))
            {
                throw new EmailInvalidException();
            }

            if (!password.Any(s => char.IsUpper(s)))
            {
                throw new PasswordNotStrongException();
            }

            if (!password.Any(s => char.IsLower(s)))
            {
                throw new PasswordNotStrongException();
            }

            if (!password.Any(s => char.IsDigit(s)))
            {
                throw new PasswordNotStrongException();
            }
            
            if(!password.Any(s=>char.IsPunctuation(s)))
            {
                throw new PasswordNotStrongException();
            }

            var account = new Account
            {
                Username = username,
                Password = password
            };

            repository.Create(account);

            //call the data access layer to save the record
            return true;
        }
    }
}
