using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Registration.RegistrationExceptions;

namespace BlastAsia.DigiBook.Domain.Registration
{
    public class RegistrationService
    {
        const int _minimumPasswordLength = 8;
        const string _emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        const string _upperCase = @"(?=.*[A-Z])([A-Z])";
        const string _lowerCase = @"[a-z]";
        const string _number = @"[\d]";

        private IAccountRepository repository;

        public RegistrationService(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public bool Register(string username, string password)
        {
  
            if (string.IsNullOrEmpty(username))
            {
                throw new UsernameRequiredException();
            }

            if (!Regex.IsMatch(username, _emailPattern))
            {
                throw new InvalidUserNameFormatException();
            }
            
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordRequiredExeption();
            }

            if (password.Length < _minimumPasswordLength )
            {
                throw new MinimumLengthRequiredException();
            }

            if (!Regex.IsMatch(password, _upperCase))
            {
                throw new StrongPasswordRequiredException();
            }

            if (!Regex.IsMatch(password, _lowerCase))
            {
                throw new StrongPasswordRequiredException();
            }

            if (!Regex.IsMatch(password, _number))
            {
                throw new StrongPasswordRequiredException();
            }

            if (!password.Any((c) => char.IsPunctuation(c) ))
            {
                throw new StrongPasswordRequiredException();
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
