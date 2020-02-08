using AutoMapper;
using cothrive_backend.api.authentication.Application.Common.Mappings;
using cothrive_backend.api.authentication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Application.User.Queries.GetUserDetail
{
    public class UserDetailVm : IMapFrom<Users>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Phone { get; set; }
        public bool Gender { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Users, UserDetailVm>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
        }

    }
}
