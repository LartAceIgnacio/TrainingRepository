using System;
using System.Text.RegularExpressions;
using System.Linq;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain
{
    public class RegistrationService
    {
        private readonly IAccountRepository repository;

        public RegistrationService(IAccountRepository repository)
        {
            this.repository = repository;
        }

        private readonly int PasswordMinimumLength = 8;
        private readonly string EmailValid = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";


        public bool Register(string username, string password)
        {
            //check business rules
            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }
            if (!Regex.IsMatch(username,EmailValid,RegexOptions.IgnoreCase))
            {
                throw new ThrowsValidEmailRequiredException();
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }
            if (password.Length < PasswordMinimumLength)
            {
                throw new PasswordMinimunLenghtRequiredException();
            }
            if (!password.Any(c => char.IsUpper(c)))
            {
                throw new PasswordStrongRequiredException();
            }
            if (!password.Any(c => char.IsLower(c)))
            {
                throw new PasswordStrongRequiredException();
            }
            if (!password.Any(c => char.IsPunctuation(c)))
            {
                throw new PasswordStrongRequiredException();
            }
            if (!password.Any(c => char.IsDigit(c)))
            {
                throw new PasswordStrongRequiredException();
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
