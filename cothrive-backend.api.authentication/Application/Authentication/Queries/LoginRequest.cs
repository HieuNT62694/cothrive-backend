using MediatR;
using System.ComponentModel.DataAnnotations;


namespace cothrive_backend.api.authentication.Application.Authentication.Queries
{
    public class LoginRequest : IRequest<LoginSuccessModel>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
