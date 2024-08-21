using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Services.Dtos
{
    public class LoginInfo
    {
        public bool IsLoggedIn { get;set; }
        public string Username { get;set; }
        public UserType? UserType { get;set; }
    }
}
