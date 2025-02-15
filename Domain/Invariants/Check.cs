using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Invariants
{
    public static class Check
    {
        public static string IsNotNullOrWhiteSpace(string? input, string paramName, int? minLength = null, int? maxLength = null)
        {
            input = input?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new DomainException($"{paramName} cannot be null or whitespace.");
            }

            if (minLength.HasValue && input.Length < minLength.Value)
            {
                throw new DomainException($"{paramName} must be at least {minLength.Value} characters long.");
            }

            if (maxLength.HasValue && input.Length > maxLength.Value)
            {
                throw new DomainException($"{paramName} must be no more than {maxLength.Value} characters long.");
            }

            return input;
        }

        public static string IsValidEmailFormat(string? email, string paramName, int? minLength = null, int? maxLength = null)
        {
            email = email?.Trim();

            IsNotNullOrWhiteSpace(email, paramName, minLength, maxLength);

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email ?? "", emailRegex))
            {
                throw new DomainException($"{paramName} is not in a valid email format.");
            }

            return email ?? "";
        }

        public static string IsValidPasswordFormat(string password, string paramName, int minLength, int maxLength)
        {
            password = IsNotNullOrWhiteSpace(password, paramName, minLength, maxLength);

            var passwordRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");

            if (!passwordRegex.IsMatch(password))
            {
                throw new DomainException($"{paramName} must contain at least one uppercase letter, one lowercase letter, and one digit.");
            }

            return password;
        }
        public static string? IsValidMaxLength(string? input, string paramName, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.Trim();

            if (input.Length > maxLength)
            {
                throw new DomainException($"{paramName} must be no more than {maxLength} characters long.");
            }

            return input;
        }

        public static decimal EnsurePositive(decimal value)
        {
            if (value <= 0)
            {
                throw new DomainException("Value must be positive.");
            }

            return value;
        }

        public static decimal EnsurePositiveAllowZero(decimal value)
        {
            if (value < 0)
            {
                throw new DomainException("Value must be positive or zero.");
            }

            return value;
        }

        public static void EnsureDatesAreInOrder(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new DomainException($"startDate cannot be earlier than endDate.");
            }
        }

        public static object EnsureNotNull(object value, string paramName)
        {
            if (value is null)
            {
                throw new DomainException($"{paramName} cannot be null.");
            }

            return value;
        }

    }
}
