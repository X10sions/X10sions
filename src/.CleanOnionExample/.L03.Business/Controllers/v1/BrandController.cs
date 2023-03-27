using CleanOnionExample.Data.Entities.Services;

namespace CleanOnionExample.Controllers.v1;

public class BrandController : BaseApiVersionController<BrandController> {
  [HttpGet]
  public async Task<IActionResult> GetAll() {
    var brands = await _mediator.Send(new GetAllBrandsCached.Query());
    return Ok(brands);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id) {
    var brand = await _mediator.Send(new GetBrandById.Query() { Id = id });
    return Ok(brand);
  }

  // POST api/<controller>
  [HttpPost]
  public async Task<IActionResult> Post(CreateBrand.Command command) {
    return Ok(await _mediator.Send(command));
  }

  // PUT api/<controller>/5
  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, UpdateBrand.Command command) {
    if (id != command.Id) {
      return BadRequest();
    }
    return Ok(await _mediator.Send(command));
  }

  // DELETE api/<controller>/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id) {
    return Ok(await _mediator.Send(new DeleteBrandCommand { Id = id }));
  }
}