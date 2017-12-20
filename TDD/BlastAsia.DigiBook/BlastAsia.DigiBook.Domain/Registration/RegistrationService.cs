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
        private readonly int PasswordMinimumLength = 8;
        private readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public bool Register(string username, string password)
        {
            // Check business rules
            if (string.IsNullOrEmpty(username))
            {
                throw new UserNameRequiredException();
            }
            if (!Regex.IsMatch(username, strRegex, RegexOptions.IgnoreCase))
            {
                throw new ValidEmailRequiredException();
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }

            if (password.Length < PasswordMinimumLength)
            {
                throw new PasswordMinimumLengthRequiredException();
            }

            if (!password.Any(c => char.IsUpper(c)))
            {
                throw new StrongPasswordRequiredException();
            }

            if (!password.Any(c => char.IsLower(c)))
            {
                throw new StrongPasswordRequiredException();
            }

            if (!password.Any(c => char.IsDigit(c)))
            {
                throw new StrongPasswordRequiredException();
            }

            if (!password.Any(c => char.IsPunctuation(c)))
            {
                throw new StrongPasswordRequiredException();
            }
            // Call the data ccess layer to save the record
            var account = new Account
            {
                Username = username,
                Password = username
            };
            repository.Create(account);

            return true;
        }
        
    }

}
