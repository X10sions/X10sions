using CleanOnionExample.Data.Entities.Services;

namespace CleanOnionExample.Controllers;

[Authorize]
[ApiVersion("1.0")]
public class CustomerController : BaseApiVersionController<CustomerController> {
  private IMediator _mediator;
  protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

  [HttpPost]
  public async Task<IActionResult> Create(CreateCustomerCommand command) {
    return Ok(await Mediator.Send(command));
  }

  [HttpGet]
  [Route("")]
  public async Task<IActionResult> GetAll() {
    return Ok(await Mediator.Send(new GetAllCustomerQuery()));
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id) {
    return Ok(await Mediator.Send(new GetCustomerByIdQuery { Id = id }));
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id) {
    return Ok(await Mediator.Send(new DeleteCustomerByIdCommand { Id = id }));
  }


  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, UpdateCustomerCommand command) {
    if (id != command.Id) {
      return BadRequest();
    }
    return Ok(await Mediator.Send(command));
  }
}
