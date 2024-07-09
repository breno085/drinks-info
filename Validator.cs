using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinks_info
{
    public class Validator
    {
        public static bool IsIdValid(string stringInput)
        {
            if (String.IsNullOrEmpty(stringInput))
            {
                return false;
            }

            foreach (char c in stringInput)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        //3 passo
        //criar a classe responsável por validar os inputs
        internal static bool IsStringValid(string? stringInput)
        {
            if (String.IsNullOrEmpty(stringInput))
            {
                return false;
            }

            foreach (char c in stringInput)
            {
                if (!Char.IsLetter(c) && c != '/' && c != ' ')
                {
                    return false;
                }
            }

            return true;
        }
    }
}