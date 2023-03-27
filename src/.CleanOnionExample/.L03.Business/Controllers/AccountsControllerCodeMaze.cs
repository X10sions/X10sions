using CleanOnionExample.Data.Entities;

namespace CleanOnionExample.Controllers;

[ApiController, Route("api/owners/{ownerId:guid}/accountsCodeMaze")]
public class AccountsControllerCodeMaze : BaseApiController<AccountsControllerCodeMaze> {
  private readonly IServiceManager _serviceManager;

  public AccountsControllerCodeMaze(IServiceManager serviceManager) => _serviceManager = serviceManager;

  [HttpGet]
  public async Task<IActionResult> GetAccounts(Guid ownerId, CancellationToken cancellationToken) {
    var accountsDto = await _serviceManager.AccountService2.GetAllByOwnerIdAsync(ownerId, cancellationToken);
    return Ok(accountsDto);
  }

  [HttpGet("{accountId:guid}")]
  public async Task<IActionResult> GetAccountById(Guid ownerId, Guid accountId, CancellationToken cancellationToken) {
    var accountDto = await _serviceManager.AccountService2.GetByIdAsync(ownerId, accountId, cancellationToken);
    return Ok(accountDto);
  }

  [HttpPost]
  public async Task<IActionResult> CreateAccount(Guid ownerId, [FromBody] Account.UpdateCommand accountForCreationDto, CancellationToken cancellationToken) {
    var response = await _serviceManager.AccountService2.CreateAsync(ownerId, accountForCreationDto, cancellationToken);
    return CreatedAtAction(nameof(GetAccountById), new { ownerId = response.OwnerId, accountId = response.Id }, response);
  }

  [HttpDelete("{accountId:guid}")]
  public async Task<IActionResult> DeleteAccount(Guid ownerId, Guid accountId, CancellationToken cancellationToken) {
    await _serviceManager.AccountService2.DeleteAsync(ownerId, accountId, cancellationToken);
    return NoContent();
  }
}
