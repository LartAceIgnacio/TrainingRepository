using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly string emailregex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public bool Register(string username, string password)
        {
            // Call the data access layer to save the record.

            if (string.IsNullOrEmpty(username)) // Blank Username
            {
                throw new UsernameRequiredException();
            }

            if (!Regex.IsMatch(username, emailregex, RegexOptions.IgnoreCase))// Valid Email
            {
                throw new EmailRequiredException(); 
            }

            if (string.IsNullOrEmpty(password)) // Blank Password
            {
                throw new PasswordRequiredException();
            }

            if (password.Length < passwordMinimumLength) //At least 8 characters
            {
                throw new PasswordMinimumLenghtException();
            }

            if (!password.Any(c => char.IsUpper(c))) //Must have Upper case
            {
                throw new StrongPasswordException();
            }

            if (!password.Any(c => char.IsLower(c))) // Must Have Lower Case
            {
                throw new StrongPasswordException();
            }
            
            if (!password.Any(c => ! char.IsLetterOrDigit(c))) //Must Have Special Character
            {
                throw new StrongPasswordException();
            }

            if (!password.Any(c => char.IsDigit(c))) // Must Have A Number
            {
                throw new StrongPasswordException();
            }

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
