using AutoMapper;
using AutoMapper.QueryableExtensions;
using cothrive_backend.api.authentication.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Application.User.Queries.GetUsersList
{
    public class GetUsersListQuery : IRequest<UsersListVm>
    {
        public class GetEmployeesListQueryHandler : IRequestHandler<GetUsersListQuery, UsersListVm>
        {
            private readonly cothriveContext _context;
            private readonly IMapper _mapper;

            public GetEmployeesListQueryHandler(cothriveContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UsersListVm> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
            {
                var users = await _context.Users
                    .ProjectTo<UserLookupDto>(_mapper.ConfigurationProvider)
                    .OrderBy(e => e.Name)
                    .ToListAsync(cancellationToken);

                var vm = new UsersListVm
                {
                    Users = users
                };

                return vm;
            }
        }
    }
}
