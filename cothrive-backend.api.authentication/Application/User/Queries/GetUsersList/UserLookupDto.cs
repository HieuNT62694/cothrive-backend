using AutoMapper;
using cothrive_backend.api.authentication.Application.Common.Mappings;
using cothrive_backend.api.authentication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Application.User.Queries.GetUsersList
{
    public class UserLookupDto : IMapFrom<Users>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string CompanyName { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Users, UserLookupDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.LastName + ", " + s.FirstName))
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.Role))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.CompanyName));
        }
    }
}
