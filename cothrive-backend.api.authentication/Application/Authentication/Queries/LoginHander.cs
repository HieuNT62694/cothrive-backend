using cothrive_backend.api.authentication.Domain.Entities;
using cothrive_backend.api.authentication.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Application.Authentication.Queries
{
    public class LoginHander : IRequestHandler<LoginRequest, LoginSuccessModel>
    {
        private readonly IAccountHelper _helper;
        private readonly cothriveContext _context;

        public LoginHander(IAccountHelper helper, cothriveContext context)
        {
            _helper = helper;
            _context = context;
        }
        public async Task<LoginSuccessModel> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var accounts = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.RoleId == 1);

            bool checkPassword = false;
            if (accounts != null)
            {
                LoginSuccessModel resultReturn = new LoginSuccessModel();
                if (accounts.IsActive == false)
                {
                    resultReturn.IdError = 1;
                    return resultReturn;
                }
                checkPassword = BCrypt.Net.BCrypt.Verify(request.Password, accounts.Password);
                if (checkPassword)
                {
                    string role = _context.Roles.AsNoTracking().FirstOrDefault(r => r.Id == accounts.RoleId).Role;
                    string fullname = accounts.FirstName + " " + accounts.LastName;
                    resultReturn.Id = accounts.Id;
                    resultReturn.FullName = fullname;
                    resultReturn.Role = role;
                    resultReturn.Token = _helper.GenerateJwtToken(request.Email, accounts, role);
                    return resultReturn;
                }

            }
            return null;
        }
    }
}
