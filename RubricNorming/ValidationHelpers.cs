using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubricNorming
{
    public static class ValidationHelpers
    {

        public static bool IsValidLength(this string stringToTest, int maxLength)
        {
            bool isValid = false;

            if (stringToTest.Length <= maxLength || stringToTest == "" || stringToTest == null)
            {
                isValid = true;
            }

            return isValid;
        }

    }
}
