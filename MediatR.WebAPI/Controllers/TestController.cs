using MediatR.MediatR;
using MediatR.WebAPI.Application;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.WebAPI.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly ISender sender;
    public TestController(ISender sender)
    {
        this.sender = sender;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new TestCommand("Test Name"), cancellationToken);
        return Ok(result);
    }
}
