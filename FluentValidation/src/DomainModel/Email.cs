using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            if (Validate(value).IsFailure)
                throw new Exception();
            Value = value;
        }

        public static Result Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Failure<Email>("Value is required");

            string email = input.Trim();

            if (email.Length > 150)
                return Result.Failure<Email>("Value is too big");

            if (Regex.IsMatch(email, @"^(.+)@(.+)$") == false)
                return Result.Failure<Email>("Value is invalid");

            return Result.Success();
        }


        public static Result<Email, Error> Create(string input)
        {
            //if (string.IsNullOrWhiteSpace(input))
            //    return Errors.General.ValueIsRequired();

            string email = input.Trim();

            //if (email.Length > 150)
            //    return Errors.General.InvalidLength();

            //if (Regex.IsMatch(email, @"^(.+)@(.+)$") == false)
            //    return Errors.General.ValueIsInvalid();

            return new Email(email);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
