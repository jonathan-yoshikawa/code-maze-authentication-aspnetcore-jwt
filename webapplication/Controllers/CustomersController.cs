using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    // GET api/values
    [HttpGet, Authorize(Roles = "Manager")]
    public IEnumerable<string> Get()
    {
        return new string[] { "John Doe", "Jane Doe", "Jonathan Yoshikawa", "Dayana Yoshikawa", "Noboru Yoshikawa" };
    }
}