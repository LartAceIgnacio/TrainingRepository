using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain
{
    public class RegistrationService
    {
        private readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        private readonly int MinimumLength = 8;

        private readonly IAccountRepository _repository;

        public RegistrationService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public bool Register(string username, string password)
        {
            // Check business ruels

            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }

            if (password.Length < MinimumLength)
            {
                throw new MinimumLengthRequiredException();
            }

            if (!password.Any(Char.IsUpper))
            {
                throw new StrongPasswordRequired();
            }

            if (!password.Any(Char.IsLower))
            {
                throw new StrongPasswordRequired();
            }

            if (!password.Any(Char.IsPunctuation))
            {
                throw new StrongPasswordRequired();
            }

            if (!password.Any(Char.IsDigit))
            {
                throw new StrongPasswordRequired();
            }

            if (!Regex.IsMatch(username,strRegex, RegexOptions.IgnoreCase))
            {
                throw new StrongPasswordRequired();
            }

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
