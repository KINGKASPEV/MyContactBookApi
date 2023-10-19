using ContactBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Utilities
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(AppUser user);
    }
}
