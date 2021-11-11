using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebookProject
{
    public class Validation
    {
        public bool Required { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public char[] ValidSymbols { get; set; }

        public Validation(bool required, int minLength, int maxLength, char[] validSymbols)
        {
            Required = required;
            MinLength = minLength;
            MaxLength = maxLength;
            ValidSymbols = validSymbols;
        }

        public bool TryValidate(string inputString, out string result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(inputString))
            {
                if (Required)
                {
                    result = "Это поле является обязательным!";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (inputString.Length < MinLength)
            {
                result = $"Минимальная длина: {MinLength}!";
                return false;
            }
            else if (inputString.Length > MaxLength)
            {
                result = $"Максимальная длина: {MaxLength}!";
                return false;
            }

            if (!inputString.All(symbol => ValidSymbols.Contains(symbol)))
            {
                result = $"Используйте только: {new string(ValidSymbols)}!";
                return false;
            }

            return true;
        }
    }
}