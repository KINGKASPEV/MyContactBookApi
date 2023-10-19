using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.DTO
{
    public class UserToReturnDTO
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
