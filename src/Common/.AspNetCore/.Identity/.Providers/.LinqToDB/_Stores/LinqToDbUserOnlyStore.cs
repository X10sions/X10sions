using LinqToDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class LinqToDbUserOnlyStore<TKey> :
      UserStoreBase<IdentityUserNav<TKey>, TKey, IdentityUserClaimNav<TKey>, IdentityUserLoginNav<TKey>, IdentityUserTokenNav<TKey>>,
      IUserLoginStore<IdentityUserNav<TKey>>,
      IUserClaimStore<IdentityUserNav<TKey>>,
      IUserPasswordStore<IdentityUserNav<TKey>>,
      IUserSecurityStampStore<IdentityUserNav<TKey>>,
      IUserEmailStore<IdentityUserNav<TKey>>,
      IUserLockoutStore<IdentityUserNav<TKey>>,
      IUserPhoneNumberStore<IdentityUserNav<TKey>>,
      IQueryableUserStore<IdentityUserNav<TKey>>,
      IUserTwoFactorStore<IdentityUserNav<TKey>>,
      IUserAuthenticationTokenStore<IdentityUserNav<TKey>>,
      IUserAuthenticatorKeyStore<IdentityUserNav<TKey>>,
      IUserTwoFactorRecoveryCodeStore<IdentityUserNav<TKey>>,
      IProtectedUserStore<IdentityUserNav<TKey>>
      where TKey : IEquatable<TKey> {
    public LinqToDbUserOnlyStore(IIdentityDataConnection<TKey> dataConnection, IdentityErrorDescriber describer = null) : base(describer ?? new IdentityErrorDescriber()) {
      DataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
    }

    public IIdentityDataConnection<TKey> DataConnection { get; private set; }

    //public ITable<PLP05_SupplierAddress> PLP05 => GetTable<PLP05_SupplierAddress>();
    protected ITable<IdentityUserNav<TKey>> UsersSet => DataConnection.GetTable<IdentityUserNav<TKey>>();
    protected ITable<IdentityUserClaimNav<TKey>> UserClaims => DataConnection.GetTable<IdentityUserClaimNav<TKey>>();
    protected ITable<IdentityUserLoginNav<TKey>> UserLogins => DataConnection.GetTable<IdentityUserLoginNav<TKey>>();
    protected ITable<IdentityUserTokenNav<TKey>> UserTokens => DataConnection.GetTable<IdentityUserTokenNav<TKey>>();

    public bool AutoSaveChanges { get; set; } = true;

    //protected Task SaveChanges(CancellationToken cancellationToken) => AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;

    public override async Task<IdentityResult> CreateAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      await DataConnection.InsertAsync(user);
      //await SaveChanges(cancellationToken);
      return IdentityResult.Success;
    }

    public override async Task<IdentityResult> UpdateAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }

      //Context.Attach(user);
      user.ConcurrencyStamp = Guid.NewGuid().ToString();
      await DataConnection.UpdateAsync(user);
      //try {
      //  await SaveChanges(cancellationToken);
      //} catch (DbUpdateConcurrencyException) {
      //  return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      //}
      return IdentityResult.Success;
    }

    public override async Task<IdentityResult> DeleteAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      await DataConnection.DeleteAsync(user);
      //try {
      //  await SaveChanges(cancellationToken);
      //} catch (DbUpdateConcurrencyException) {
      //  return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      //}
      return IdentityResult.Success;
    }

    public override Task<IdentityUserNav<TKey>> FindByIdAsync(string userId, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      var id = ConvertIdFromString(userId);
      return UsersSet.FirstOrDefaultAsync(x => x.Id.Equals(new object[] { id }), cancellationToken);
    }

    public override Task<IdentityUserNav<TKey>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
    }

    public override IQueryable<IdentityUserNav<TKey>> Users => UsersSet;

    protected override Task<IdentityUserNav<TKey>> FindUserAsync(TKey userId, CancellationToken cancellationToken) => Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

    protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    public override async Task<IList<Claim>> GetClaimsAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }

      return await UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
    }

    public override Task AddClaimsAsync(IdentityUserNav<TKey> user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (claims == null) {
        throw new ArgumentNullException(nameof(claims));
      }
      foreach (var claim in claims) {
        UserClaims.DataContext.Insert(CreateUserClaim(user, claim));
      }
      return Task.FromResult(false);
    }

    public override async Task ReplaceClaimAsync(IdentityUserNav<TKey> user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      if (newClaim == null) {
        throw new ArgumentNullException(nameof(newClaim));
      }
      var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
      foreach (var matchedClaim in matchedClaims) {
        matchedClaim.ClaimValue = newClaim.Value;
        matchedClaim.ClaimType = newClaim.Type;
      }
    }

    public override async Task RemoveClaimsAsync(IdentityUserNav<TKey> user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (claims == null) {
        throw new ArgumentNullException(nameof(claims));
      }
      foreach (var claim in claims) {
        var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
        foreach (var c in matchedClaims) {
          UserClaims.DataContext.Delete(c);
        }
      }
    }

    public override Task AddLoginAsync(IdentityUserNav<TKey> user, UserLoginInfo login,
        CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (login == null) {
        throw new ArgumentNullException(nameof(login));
      }
      UserLogins.DataContext.Insert(CreateUserLogin(user, login));
      return Task.FromResult(false);
    }

    public override async Task RemoveLoginAsync(IdentityUserNav<TKey> user, string loginProvider, string providerKey,
        CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
      if (entry != null) {
        UserLogins.DataContext.Delete(entry);
      }
    }

    public override async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      var userId = user.Id;
      return await UserLogins.Where(l => l.UserId.Equals(userId))
          .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken);
    }

    public override async Task<IdentityUserNav<TKey>> FindByLoginAsync(string loginProvider, string providerKey,
        CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
      if (userLogin != null) {
        return await FindUserAsync(userLogin.UserId, cancellationToken);
      }
      return null;
    }

    public override Task<IdentityUserNav<TKey>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      return Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public override async Task<IList<IdentityUserNav<TKey>>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      var query = from userclaims in UserClaims
                  join user in Users on userclaims.UserId equals user.Id
                  where userclaims.ClaimValue == claim.Value
                  && userclaims.ClaimType == claim.Type
                  select user;
      return await query.ToListAsync(cancellationToken);
    }

    protected override Task<IdentityUserTokenNav<TKey>> FindTokenAsync(IdentityUserNav<TKey> user, string loginProvider, string name, CancellationToken cancellationToken)
        => UserTokens.FirstOrDefaultAsync(x => x.UserId.Equals(user.Id)
        && x.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase)
        && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase), cancellationToken);

    protected override Task AddUserTokenAsync(IdentityUserTokenNav<TKey> token) {
      UserTokens.DataContext.Insert(token);
      return Task.CompletedTask;
    }


    protected override Task RemoveUserTokenAsync(IdentityUserTokenNav<TKey> token) {
      UserTokens.DataContext.Delete(token);
      return Task.CompletedTask;
    }
  }

  //public class IdentityUserOnlyNavStore<TKey> :
  //   UserStoreBase<IdentityUserNav<TKey>, TKey, IdentityUserClaimNav<TKey>, IdentityUserLoginNav<TKey>, IdentityUserTokenNav<TKey>>,
  //   IUserLoginStore<IdentityUserNav<TKey>>,
  //   IUserClaimStore<IdentityUserNav<TKey>>,
  //   IUserPasswordStore<IdentityUserNav<TKey>>,
  //   IUserSecurityStampStore<IdentityUserNav<TKey>>,
  //   IUserEmailStore<IdentityUserNav<TKey>>,
  //   IUserLockoutStore<IdentityUserNav<TKey>>,
  //   IUserPhoneNumberStore<IdentityUserNav<TKey>>,
  //   IQueryableUserStore<IdentityUserNav<TKey>>,
  //   IUserTwoFactorStore<IdentityUserNav<TKey>>,
  //   IUserAuthenticationTokenStore<IdentityUserNav<TKey>>,
  //   IUserAuthenticatorKeyStore<IdentityUserNav<TKey>>,
  //   IUserTwoFactorRecoveryCodeStore<IdentityUserNav<TKey>>,
  //   IProtectedUserStore<IdentityUserNav<TKey>>
  //   where TKey : IEquatable<TKey> {

  //  public IdentityUserOnlyNavStore(IIdentityDataConnection<TKey> dataConnection, IdentityErrorDescriber describer = null) : base(describer ?? new IdentityErrorDescriber()) {
  //    DataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
  //  }

  //  public IIdentityDataConnection<TKey> DataConnection { get; private set; }

  //  //public ITable<PLP05_SupplierAddress> PLP05 => GetTable<PLP05_SupplierAddress>();
  //  protected ITable<IdentityUserNav<TKey>> UsersSet => DataConnection.GetTable<IdentityUserNav<TKey>>();
  //  protected ITable<IdentityUserClaimNav<TKey>> UserClaims => DataConnection.GetTable<IdentityUserClaimNav<TKey>>();
  //  protected ITable<IdentityUserLoginNav<TKey>> UserLogins => DataConnection.GetTable<IdentityUserLoginNav<TKey>>();
  //  protected ITable<IdentityUserTokenNav<TKey>> UserTokens => DataConnection.GetTable<IdentityUserTokenNav<TKey>>();

  //  public bool AutoSaveChanges { get; set; } = true;

  //  //protected Task SaveChanges(CancellationToken cancellationToken) => AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;

  //  public override async Task<IdentityResult> CreateAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    await DataConnection.InsertAsync(user);
  //    //await SaveChanges(cancellationToken);
  //    return IdentityResult.Success;
  //  }

  //  public override async Task<IdentityResult> UpdateAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }

  //    //Context.Attach(user);
  //    user.ConcurrencyStamp = Guid.NewGuid().ToString();
  //    await DataConnection.UpdateAsync(user);
  //    //try {
  //    //  await SaveChanges(cancellationToken);
  //    //} catch (DbUpdateConcurrencyException) {
  //    //  return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
  //    //}
  //    return IdentityResult.Success;
  //  }

  //  public override async Task<IdentityResult> DeleteAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    await DataConnection.DeleteAsync(user);
  //    //try {
  //    //  await SaveChanges(cancellationToken);
  //    //} catch (DbUpdateConcurrencyException) {
  //    //  return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
  //    //}
  //    return IdentityResult.Success;
  //  }

  //  public override Task<IdentityUserNav<TKey>> FindByIdAsync(string userId, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    var id = ConvertIdFromString(userId);
  //    return UsersSet.FirstOrDefaultAsync(x => x.Id.Equals(new object[] { id }), cancellationToken);
  //  }

  //  public override Task<IdentityUserNav<TKey>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
  //  }

  //  public override IQueryable<IdentityUserNav<TKey>> Users => UsersSet;

  //  protected override Task<IdentityUserNav<TKey>> FindUserAsync(TKey userId, CancellationToken cancellationToken) => Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

  //  protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

  //  protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

  //  public override async Task<IList<Claim>> GetClaimsAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }

  //    return await UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
  //  }

  //  public override Task AddClaimsAsync(IdentityUserNav<TKey> user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) {
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (claims == null) {
  //      throw new ArgumentNullException(nameof(claims));
  //    }
  //    foreach (var claim in claims) {
  //      UserClaims.DataContext.Insert(CreateUserClaim(user, claim));
  //    }
  //    return Task.FromResult(false);
  //  }

  //  public override async Task ReplaceClaimAsync(IdentityUserNav<TKey> user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default) {
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (claim == null) {
  //      throw new ArgumentNullException(nameof(claim));
  //    }
  //    if (newClaim == null) {
  //      throw new ArgumentNullException(nameof(newClaim));
  //    }
  //    var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
  //    foreach (var matchedClaim in matchedClaims) {
  //      matchedClaim.ClaimValue = newClaim.Value;
  //      matchedClaim.ClaimType = newClaim.Type;
  //    }
  //  }

  //  public override async Task RemoveClaimsAsync(IdentityUserNav<TKey> user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) {
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (claims == null) {
  //      throw new ArgumentNullException(nameof(claims));
  //    }
  //    foreach (var claim in claims) {
  //      var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
  //      foreach (var c in matchedClaims) {
  //        UserClaims.DataContext.Delete(c);
  //      }
  //    }
  //  }

  //  public override Task AddLoginAsync(IdentityUserNav<TKey> user, UserLoginInfo login,
  //      CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (login == null) {
  //      throw new ArgumentNullException(nameof(login));
  //    }
  //    UserLogins.DataContext.Insert(CreateUserLogin(user, login));
  //    return Task.FromResult(false);
  //  }

  //  public override async Task RemoveLoginAsync(IdentityUserNav<TKey> user, string loginProvider, string providerKey,
  //      CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
  //    if (entry != null) {
  //      UserLogins.DataContext.Delete(entry);
  //    }
  //  }

  //  public override async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    var userId = user.Id;
  //    return await UserLogins.Where(l => l.UserId.Equals(userId))
  //        .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken);
  //  }

  //  public override async Task<IdentityUserNav<TKey>> FindByLoginAsync(string loginProvider, string providerKey,
  //      CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
  //    if (userLogin != null) {
  //      return await FindUserAsync(userLogin.UserId, cancellationToken);
  //    }
  //    return null;
  //  }

  //  public override Task<IdentityUserNav<TKey>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    return Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
  //  }

  //  public override async Task<IList<IdentityUserNav<TKey>>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (claim == null) {
  //      throw new ArgumentNullException(nameof(claim));
  //    }
  //    var query = from userclaims in UserClaims
  //                join user in Users on userclaims.UserId equals user.Id
  //                where userclaims.ClaimValue == claim.Value
  //                && userclaims.ClaimType == claim.Type
  //                select user;
  //    return await query.ToListAsync(cancellationToken);
  //  }

  //  protected override Task<IdentityUserTokenNav<TKey>> FindTokenAsync(IdentityUserNav<TKey> user, string loginProvider, string name, CancellationToken cancellationToken)
  //      => UserTokens.FirstOrDefaultAsync(x => x.UserId.Equals(user.Id)
  //      && x.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase)
  //      && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase), cancellationToken);

  //  protected override Task AddUserTokenAsync(IdentityUserTokenNav<TKey> token) {
  //    UserTokens.DataContext.Insert(token);
  //    return Task.CompletedTask;
  //  }

  //  protected override Task RemoveUserTokenAsync(IdentityUserTokenNav<TKey> token) {
  //    UserTokens.DataContext.Delete(token);
  //    return Task.CompletedTask;
  //  }
  //}
}
