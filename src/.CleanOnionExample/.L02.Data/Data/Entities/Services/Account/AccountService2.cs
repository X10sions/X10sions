using Common.Exceptions;
namespace CleanOnionExample.Data.Entities.Services;
using Task = System.Threading.Tasks.Task;

internal sealed class AccountService2 : IAccountService2 {
  public AccountService2(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

  private readonly IRepositoryManager _repositoryManager;

  public async Task<IEnumerable<Account.GetQuery>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) {
    var accounts = await _repositoryManager.AccountRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);
    var accountsDto = accounts.Adapt<IEnumerable<Account.GetQuery>>();
    return accountsDto;
  }

  public async Task<Account.GetQuery> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken) {
    var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    var account = await _repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);
    if (account is null) {
      throw new AccountNotFoundException(accountId);
    }
    if (account.OwnerId != owner.Id) {
      throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
    }
    var accountDto = account.Adapt<Account.GetQuery>();
    return accountDto;
  }

  public async Task<Account.GetQuery> CreateAsync(Guid ownerId, Account.UpdateCommand accountForCreationDto, CancellationToken cancellationToken = default) {
    var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    var account = accountForCreationDto.Adapt<Account>();
    account.OwnerId = owner.Id;
    await _repositoryManager.AccountRepository.InsertAsync(account, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return account.Adapt<Account.GetQuery>();
  }

  public async Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default) {
    var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    var account = await _repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);
    if (account is null) {
      throw new AccountNotFoundException(accountId);
    }
    if (account.OwnerId != owner.Id) {
      throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
    }
    await _repositoryManager.AccountRepository.DeleteAsync(account.Id, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }
}
