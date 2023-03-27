namespace CleanOnionExample.Controllers;

[ApiController, Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiVersionController<T> : ControllerBase {
  private IMediator _mediatorInstance;
  private ILogger<T> _loggerInstance;
  protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
  protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
}

[ApiController, Route("api/[controller]")]
public abstract class BaseApiController<T> : ControllerBase {
  private IMediator _mediatorInstance;
  private ILogger<T> _loggerInstance;
  protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
  protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
}