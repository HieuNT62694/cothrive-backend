using System;
using System.Collections.Generic;

namespace cothrive_backend.api.authentication.Domain.Entities
{
    public partial class Users
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Phone { get; set; }
        public bool? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual Companys Company { get; set; }
        public virtual Roles Role { get; set; }
    }
}
