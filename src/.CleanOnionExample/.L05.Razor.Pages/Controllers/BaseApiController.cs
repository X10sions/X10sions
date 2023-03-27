using Microsoft.AspNetCore.Mvc;

namespace CleanOnionExample.Controllers;

[ApiController, Route("api/[controller]")] public abstract class BaseApiController : Controller { }
[ApiController, Route("api/[controller]")] public abstract class BaseApiControllerBase : ControllerBase { }