using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;

namespace CleanOnionExample.Controllers;

public class OwnersController : BaseApiController<OwnersController> {
  public OwnersController(IServiceManager serviceManager) => _serviceManager = serviceManager;

  private readonly IServiceManager _serviceManager;

  [HttpGet]
  public async Task<IActionResult> GetOwners(CancellationToken cancellationToken) {
    var owners = await _serviceManager.OwnerService.GetAllAsync(cancellationToken);
    return Ok(owners);
  }

  [HttpGet("{ownerId:guid}")]
  public async Task<IActionResult> GetOwnerById(Guid ownerId, CancellationToken cancellationToken) {
    var ownerDto = await _serviceManager.OwnerService.GetByIdAsync(ownerId, cancellationToken);
    return Ok(ownerDto);
  }

  [HttpPost]
  public async Task<IActionResult> CreateOwner([FromBody] UpdateOwnerCommand ownerForCreationDto) {
    var ownerDto = await _serviceManager.OwnerService.CreateAsync(ownerForCreationDto);
    return CreatedAtAction(nameof(GetOwnerById), new { ownerId = ownerDto.Id }, ownerDto);
  }

  [HttpPut("{ownerId:guid}")]
  public async Task<IActionResult> UpdateOwner(Guid ownerId, [FromBody] UpdateOwnerCommand ownerForUpdateDto, CancellationToken cancellationToken) {
    await _serviceManager.OwnerService.UpdateAsync(ownerId, ownerForUpdateDto, cancellationToken);
    return NoContent();
  }

  [HttpDelete("{ownerId:guid}")]
  public async Task<IActionResult> DeleteOwner(Guid ownerId, CancellationToken cancellationToken) {
    await _serviceManager.OwnerService.DeleteAsync(ownerId, cancellationToken);
    return NoContent();
  }
}
