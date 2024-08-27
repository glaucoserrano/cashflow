using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredJson),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resgister(
        [FromServices] IRegisterUserUseCase useCase, 
        [FromBody] RequestRegisterJson request
        )
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
