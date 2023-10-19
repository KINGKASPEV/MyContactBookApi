using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Utilities
{
    static class MyExtension
    {
        public static int CalculateDOB(this int DOB)
        {
            var age = DateTime.Now.Year - DOB;
            return age;
        }
    }
}
