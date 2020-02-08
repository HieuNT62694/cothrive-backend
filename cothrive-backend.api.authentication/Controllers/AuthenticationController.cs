using cothrive_backend.api.authentication.Application.Authentication.Queries;
using cothrive_backend.api.authentication.Application.User.Queries.GetUserDetail;
using cothrive_backend.api.authentication.Application.User.Queries.GetUsersList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cothrive_backend.api.authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest queries)
        {
            var response = await Mediator.Send(queries);
            if (response == null)
            {
                return BadRequest("Invalid User Name of Password");
            }
            if (response.IdError == 1)
            {
                return Forbid();
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var vm = await Mediator.Send(new GetUserDetailQuery { Id = id });

            return Ok(vm);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetUsersListQuery()));
        }
        [HttpGet]
        [Route("private")]

        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }
    }
}