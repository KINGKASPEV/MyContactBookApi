using ContactBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.DTO
{
    public class PaginationDTO
    {
        public int count { get; set; }
        public int perPage { get; set; }
        public int CurrrentPage { get; set; }
        public IEnumerable<Contact> contacts { get; set; }
        public PaginationDTO(int count, int perPage, int CurrentPage, IEnumerable<Contact> contacts)
        {
            this.count = count;
            this.perPage = perPage;
            this.CurrrentPage = CurrrentPage;
            this.contacts = contacts;
        }

    }
}
