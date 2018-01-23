using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain
{
    public class RegistrationService
    {
        private readonly IAccountRepository repository;
        public RegistrationService(IAccountRepository repositor)
        {
            this.repository = repositor;
        }

        private readonly int minimumPasswordLength = 8;
        private readonly string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public bool Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }
            if (!Regex.IsMatch(username, pattern, RegexOptions.IgnoreCase))
            {
                throw new EmailInvalidFormatException();
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredException();
            }
            if (password.Length < minimumPasswordLength)
            {
                throw new MinimumPasswordLengthException();
            }
            if (!password.Any(ch => char.IsUpper(ch)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(ch => char.IsLower(ch)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new StrongPasswordException();
            }
            if (!password.Any(ch => char.IsDigit(ch)))
            {
                throw new StrongPasswordException();
            }

            var account = new Account { };

            repository.Create(account);
            return true;
        }
    }
}
