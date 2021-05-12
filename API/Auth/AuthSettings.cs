using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth
{
    public class AuthSettings
    {
        public string Secret { get; set; }
        public int TokenTimeout { get; set; }
    }
}
