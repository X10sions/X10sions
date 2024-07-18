using Common.Exceptions;
using Common.Domain.Repositories;
using Mapster;
using X10sions.Fake.Features.Account;
using X10sions.Fake.Features.Owner;
namespace CleanOnionExample.Data.Entities.Services;

internal sealed class AccountService2(IOwnerRepository ownerRepository, IAccountRepository accountRepository) : IAccountService2 {

  public async Task<IEnumerable<Account.GetQuery>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) {
    var accounts = await accountRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);
    var accountsDto = accounts.Adapt<IEnumerable<Account.GetQuery>>();
    return accountsDto;
  }

  public async Task<Account.GetQuery> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken) {
    var owner = await ownerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    var account = await accountRepository.GetByIdAsync(accountId, cancellationToken);
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
    var owner = await ownerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    var account = accountForCreationDto.Adapt<Account>();
    account.OwnerId = owner.Id;
    await accountRepository.InsertAsync(account, cancellationToken);
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return account.Adapt<Account.GetQuery>();
  }

  public async Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default) {
    var owner = await ownerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    var account = await accountRepository.GetByIdAsync(accountId, cancellationToken);
    if (account is null) {
      throw new AccountNotFoundException(accountId);
    }
    if (account.OwnerId != owner.Id) {
      throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
    }
    await accountRepository.DeleteByIdAsync(account.Id, cancellationToken);
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }
}
