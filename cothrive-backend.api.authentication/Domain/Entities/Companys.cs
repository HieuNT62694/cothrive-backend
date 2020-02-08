using System;
using System.Collections.Generic;

namespace cothrive_backend.api.authentication.Domain.Entities
{
    public partial class Companys
    {
        public Companys()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
