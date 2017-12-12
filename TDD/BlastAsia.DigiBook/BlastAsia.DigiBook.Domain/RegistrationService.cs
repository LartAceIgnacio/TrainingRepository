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


        private readonly int MinimumPasswordLength = 8;
        private readonly String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                  + "@"
                                  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        public bool Register(string username, string password)
        {

            if (string.IsNullOrEmpty(username))
            {
                throw new UserNameRequiredException();
            }
            if (!Regex.IsMatch(username, theEmailPattern, RegexOptions.IgnoreCase))
            {
                throw new InvalidEmailException();
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }
            if (password.Length < MinimumPasswordLength)
            {
                throw new MinimumLengthRequiredException();
            }
            if (!password.Any(c => char.IsUpper(c)))
            {
                throw new StrongPasswordRequiredException();
            }
            if (!password.Any(c => char.IsLower(c)))
            {
                throw new StrongPasswordRequiredException();
            }
            if (!password.Any(c => char.IsPunctuation(c)))
            {
                throw new StrongPasswordRequiredException();
            }
            if (!password.Any(c => char.IsDigit(c)))
            {
                throw new StrongPasswordRequiredException();
            }

            // Call the data access layer to save the record

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
