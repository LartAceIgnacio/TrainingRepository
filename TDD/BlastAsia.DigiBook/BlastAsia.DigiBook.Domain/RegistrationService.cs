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
      private readonly int PasswordMinimum = 8;
      private readonly string EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public bool Register(string username, string password)
        {

            //Check business rules
            if (string.IsNullOrEmpty(username))
            {
                throw new UserNameRequiredException();
            }
            if (!Regex.IsMatch(username,EmailPattern,RegexOptions.IgnoreCase))
            {
                throw new UsernameValidEmailRequired();
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }

            if (password.Length < PasswordMinimum)
            {
                throw new MinimumLengthRequiredException();
            }
    
            if (!password.Any(c => char.IsUpper(c)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(c => char.IsLower(c)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(c => char.IsPunctuation(c)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(c => char.IsDigit(c)))
            {
                throw new StrongPasswordException();
            }

            // Call data access layer to save the record
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
