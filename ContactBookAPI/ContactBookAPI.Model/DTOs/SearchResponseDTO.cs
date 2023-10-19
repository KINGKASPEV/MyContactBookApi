using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Model.DTOs
{
    public class SearchResponseDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
