using Microsoft.AspNetCore.Mvc;

namespace randomization.tool.Controllers;

[ApiController]
[Route("[controller]")]
public class AliveController : Controller
{
    [HttpGet]
    public string AliveEndpoint()
        => DateTime.Now.ToString();
}
