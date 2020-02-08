using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Application.Authentication.Queries
{
    public class LoginSuccessModel
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public object Token { get; set; }
        public string Role { get; set; }
        public int IdError { get; set; }
    }
}
