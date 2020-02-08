using AutoMapper;
using AutoMapper.QueryableExtensions;
using cothrive_backend.api.authentication.Application.Common.Exceptions;
using cothrive_backend.api.authentication.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Application.User.Queries.GetUserDetail
{
    public class GetUserDetailHandler : IRequestHandler<GetUserDetailQuery, UserDetailVm>
    {
        private readonly cothriveContext _context;
        private readonly IMapper _mapper;

        public GetUserDetailHandler(cothriveContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserDetailVm> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                    .Where(e => e.Id == request.Id)
                    .ProjectTo<UserDetailVm>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            return _mapper.Map<UserDetailVm>(entity);
        }
    }
}
