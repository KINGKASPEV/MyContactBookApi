using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Model.DTOs
{
    public class PhotoToAddDto
    {
        public IFormFile PhotoFile { get; set; }

    }
}
