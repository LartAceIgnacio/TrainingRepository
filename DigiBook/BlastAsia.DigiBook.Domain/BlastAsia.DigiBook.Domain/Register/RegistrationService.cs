using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain
{
    public class RegistrationService
    {
        private readonly IAccountRepository _repository;
        public RegistrationService(IAccountRepository repository)
        {
            _repository = repository;
        }
        private readonly int MinimumPasswordLength = 8;
        private readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public bool Register(string username, string password)
        {

            // Business Rules
            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }
            if (!Regex.IsMatch(username,strRegex, RegexOptions.IgnoreCase))
            {
                throw new ValidEmailException();
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }
            if (password.Length < MinimumPasswordLength)
            {
                throw new MinimumLengthException();
            }
            if (!password.Any(c => char.IsUpper(c)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(c=> char.IsLower(c)))
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
            //call data access layer to save the record
            var account = new Account
            {
                Username = username,
                Password = password
            };
            _repository.Create(account);
            return true;
        }
     }
}
