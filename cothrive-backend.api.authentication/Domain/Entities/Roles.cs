using System;
using System.Collections.Generic;

namespace cothrive_backend.api.authentication.Domain.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
