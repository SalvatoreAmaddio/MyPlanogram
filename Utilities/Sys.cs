using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Testing.Utilities
{
    public static class Sys
    {
        public static bool IsNumeric(string s)=>int.TryParse(s, out _); 

        public static string RemoveAllEmptySpaces(string s)=>
        String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));

    }
}
