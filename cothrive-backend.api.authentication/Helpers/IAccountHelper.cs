using cothrive_backend.api.authentication.Domain.Entities;


namespace cothrive_backend.api.authentication.Helpers
{
    public interface IAccountHelper
    {
        object GenerateJwtToken(string email, Users user, string Role);
    }
}
