using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain
{
    public class RegistrationService
    {
        private readonly IAccountRepository repository;
        public RegistrationService(IAccountRepository repository)
        {
            this.repository = repository;
        }

        private readonly int passwordMinimumLength = 8;
        private readonly string validEmail = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public bool Register(string username, string password)
        {
            //Check business rules
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }
            if (password.Length < passwordMinimumLength)
            {
                throw new PasswordMinimumLengthRequiredException();
            }
            if (!password.Any(char.IsUpper))
            {
                throw new PasswordStrongTypedRequired();
            }
            if (!password.Any(char.IsLower))
            {
                throw new PasswordStrongTypedRequired();
            }
            if (!password.Any(char.IsPunctuation))
            {
                throw new PasswordStrongTypedRequired();
            }
            if (!password.Any(char.IsDigit))
            {
                throw new PasswordStrongTypedRequired();
            }
            if (!Regex.IsMatch(username, validEmail, RegexOptions.IgnoreCase))
            {
                throw new ValidUserNameRequired();
            }

            //Call the data access layer to save the record
            var account = new Account
            {
                Username = username,
                Password = password
            };
            repository.Create(account);
            return true;
        }
    }
}
