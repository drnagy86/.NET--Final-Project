using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public static class DictionaryValidationHelpers
    {

        public static bool AreDifferentDictionaries(Dictionary<Facet, List<Criteria>> firstDictionary, Dictionary<Facet, List<Criteria>> secondDictionary)
        {
            return firstDictionary.Equals(secondDictionary); 
        }


    }
}
