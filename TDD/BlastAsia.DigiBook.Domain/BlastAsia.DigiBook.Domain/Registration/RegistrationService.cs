using BlastAsia.DigiBook.Domain.ExceptionsThrown;
using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain
{
    public class RegistrationService
    {
        // Global variables used by methods
        private readonly int MinimumPasswordRequirements = 8;
        private bool withSpecialCharacter = false;
        private readonly string regex = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        // construtor hide constructor
        private readonly IAccountRepository _repository;
        //construtor injection
        public RegistrationService(IAccountRepository repository)
        {
            _repository = repository; // 
        }

        public bool Register(string username, string password)
        {

            // check if blank or space
            if (string.IsNullOrWhiteSpace(username)) {
                throw new UsernameRequiredException();
            }

            // check if blank or space
            if (string.IsNullOrWhiteSpace(password)) {
                throw new PasswordRequiredException();
            }

            // Check email format if correct
            //var x = Regex.IsMatch(username, regex);
            if (!Regex.IsMatch(username, regex)) {
                throw new InvalidUsernameException();
            }

            // check if at least 8 char
            if (password.Length < MinimumPasswordRequirements) {
                throw new MinimumPasswordLengthRequiredException();
            }

            //check for uppercase
            if (!Regex.Match(password, "[A-Z]").Success) {
                throw new StrongPasswordException();
            }

            //check for lowercase
            if (!Regex.Match(password, "[a-z]").Success) {
                throw new StrongPasswordException();

            }

            //check for number
            if (!Regex.Match(password, "[0-9]").Success) {
                throw new StrongPasswordException();
            }

            //check for special characters 
            if (!password.Any(c => char.IsPunctuation(c))) // using linQ
            {
                throw new StrongPasswordException();
            }

            #region checkSpecialCharacters using loop
            //foreach (char character in password)
            //{
            //    if (!Char.IsLetterOrDigit(character))
            //    {
            //        // if its not a letter or a digit then it is a special character
            //        withSpecialCharacter = true;
            //    }
            //}
            //// if no thrown exceptions , check again for special characters result
            //if (!withSpecialCharacter) {
            //    throw new StrongPasswordException();
            //}
            #endregion

            // if passed, then execute _repository.Create();
            var account = new Account {
                Username = username,
                Password = password
            };
            _repository.Create(account);

            return true;
        }

    }
}
