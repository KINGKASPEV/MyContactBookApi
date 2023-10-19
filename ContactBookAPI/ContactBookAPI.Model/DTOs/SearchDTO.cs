using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Model.DTOs
{
    public class SearchDTO
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
