using LinqToDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class LinqToDbUserStore<TKey> :
      UserStoreBase<IdentityUserNav<TKey>, IdentityRoleNav<TKey>, TKey, IdentityUserClaimNav<TKey>, IdentityUserRoleNav<TKey>, IdentityUserLoginNav<TKey>, IdentityUserTokenNav<TKey>, IdentityRoleClaimNav<TKey>>,
      IProtectedUserStore<IdentityUserNav<TKey>>
      where TKey : IEquatable<TKey> {

    public LinqToDbUserStore(IIdentityDataConnection<TKey> dataConnection, IdentityErrorDescriber describer = null) : base(describer ?? new IdentityErrorDescriber()) {
      DataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
    }

    public IIdentityDataConnection<TKey> DataConnection { get; private set; }

    private ITable<IdentityUserNav<TKey>> UsersSet => DataConnection.GetTable<IdentityUserNav<TKey>>();
    private ITable<IdentityRoleNav<TKey>> Roles => DataConnection.GetTable<IdentityRoleNav<TKey>>();
    private ITable<IdentityUserClaimNav<TKey>> UserClaims => DataConnection.GetTable<IdentityUserClaimNav<TKey>>();
    private ITable<IdentityUserRoleNav<TKey>> UserRoles => DataConnection.GetTable<IdentityUserRoleNav<TKey>>();
    private ITable<IdentityUserLoginNav<TKey>> UserLogins => DataConnection.GetTable<IdentityUserLoginNav<TKey>>();
    private ITable<IdentityUserTokenNav<TKey>> UserTokens => DataConnection.GetTable<IdentityUserTokenNav<TKey>>();

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
      return UsersSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public override Task<IdentityUserNav<TKey>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
    }

    public override IQueryable<IdentityUserNav<TKey>> Users => UsersSet;

    protected override Task<IdentityRoleNav<TKey>> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken) => Roles.SingleOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);

    protected override Task<IdentityUserRoleNav<TKey>> FindUserRoleAsync(TKey userId, TKey roleId, CancellationToken cancellationToken)
      => UserRoles.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.RoleId.Equals(roleId), cancellationToken);

    protected override Task<IdentityUserNav<TKey>> FindUserAsync(TKey userId, CancellationToken cancellationToken) => Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

    protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    public override async Task AddToRoleAsync(IdentityUserNav<TKey> user, string normalizedRoleName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
        throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
      }
      var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
      if (roleEntity == null) {
        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, normalizedRoleName));
      }
      UserRoles.DataContext.Insert(CreateUserRole(user, roleEntity));
    }

    public override async Task RemoveFromRoleAsync(IdentityUserNav<TKey> user, string normalizedRoleName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
        throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
      }
      var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
      if (roleEntity != null) {
        var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
        if (userRole != null) {
          UserRoles.DataContext.Delete(userRole);
        }
      }
    }

    public override async Task<IList<string>> GetRolesAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      var userId = user.Id;
      var query = from userRole in UserRoles
                  join role in Roles on userRole.RoleId equals role.Id
                  where userRole.UserId.Equals(userId)
                  select role.Name;
      return await query.ToListAsync(cancellationToken);
    }

    public override async Task<bool> IsInRoleAsync(IdentityUserNav<TKey> user, string normalizedRoleName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
        throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
      }
      var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
      if (role != null) {
        var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
        return userRole != null;
      }
      return false;
    }

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

    public override async Task<IList<IdentityUserNav<TKey>>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (string.IsNullOrEmpty(normalizedRoleName)) {
        throw new ArgumentNullException(nameof(normalizedRoleName));
      }

      var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

      if (role != null) {
        var query = from userrole in UserRoles
                    join user in Users on userrole.UserId equals user.Id
                    where userrole.RoleId.Equals(role.Id)
                    select user;

        return await query.ToListAsync(cancellationToken);
      }
      return new List<IdentityUserNav<TKey>>();
    }

    protected override Task<IdentityUserTokenNav<TKey>> FindTokenAsync(IdentityUserNav<TKey> user, string loginProvider, string name, CancellationToken cancellationToken)
      => (from ut in UserTokens
          where ut.UserId.Equals(user.Id)
          && ut.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase)
          && ut.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
          select ut).FirstOrDefaultAsync(cancellationToken);

    protected override Task AddUserTokenAsync(IdentityUserTokenNav<TKey> token) {
      UserTokens.DataContext.Insert(token);
      return Task.CompletedTask;
    }

    protected override Task RemoveUserTokenAsync(IdentityUserTokenNav<TKey> token) {
      UserTokens.DataContext.Delete(token);
      return Task.CompletedTask;
    }
  }

  //public class IdentityUserNavStore<TKey> :
  //  UserStoreBase<IdentityUserNav<TKey>, IdentityRoleNav<TKey>, TKey, IdentityUserClaimNav<TKey>, IdentityUserRoleNav<TKey>, IdentityUserLoginNav<TKey>, IdentityUserTokenNav<TKey>, IdentityRoleClaimNav<TKey>>,
  //  IProtectedUserStore<IdentityUserNav<TKey>>
  //  where TKey : IEquatable<TKey> {

  //  public IdentityUserNavStore(IIdentityUserAndRoleContext<TKey> dataConnection, IdentityErrorDescriber describer = null) : base(describer ?? new IdentityErrorDescriber()) {
  //    DataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
  //  }

  //  public IIdentityUserAndRoleContext<TKey> DataConnection { get; private set; }

  //  private IIdentityDatabaseTable<IdentityUserNav<TKey>> UsersSet => DataConnection.GetTable<IdentityUserNav<TKey>>();
  //  private IIdentityDatabaseTable<IdentityRoleNav<TKey>> Roles => DataConnection.GetTable<IdentityRoleNav<TKey>>();
  //  private IIdentityDatabaseTable<IdentityUserClaimNav<TKey>> UserClaims => DataConnection.GetTable<IdentityUserClaimNav<TKey>>();
  //  private IIdentityDatabaseTable<IdentityUserRoleNav<TKey>> UserRoles => DataConnection.GetTable<IdentityUserRoleNav<TKey>>();
  //  private IIdentityDatabaseTable<IdentityUserLoginNav<TKey>> UserLogins => DataConnection.GetTable<IdentityUserLoginNav<TKey>>();
  //  private IIdentityDatabaseTable<IdentityUserTokenNav<TKey>> UserTokens => DataConnection.GetTable<IdentityUserTokenNav<TKey>>();

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
  //    return UsersSet.xFirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
  //  }

  //  public override Task<IdentityUserNav<TKey>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    return Users.xFirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
  //  }

  //  public override IQueryable<IdentityUserNav<TKey>> Users => UsersSet;

  //  protected override Task<IdentityRoleNav<TKey>> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken) => Roles.SingleOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);

  //  protected override Task<IdentityUserRoleNav<TKey>> FindUserRoleAsync(TKey userId, TKey roleId, CancellationToken cancellationToken)
  //    => UserRoles.xFirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.RoleId.Equals(roleId), cancellationToken);

  //  protected override Task<IdentityUserNav<TKey>> FindUserAsync(TKey userId, CancellationToken cancellationToken) => Users.xSingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

  //  protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

  //  protected override Task<IdentityUserLoginNav<TKey>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) => UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

  //  public override async Task AddToRoleAsync(IdentityUserNav<TKey> user, string normalizedRoleName, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
  //      throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
  //    }
  //    var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
  //    if (roleEntity == null) {
  //      throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, normalizedRoleName));
  //    }
  //    UserRoles.DataContext.Insert(CreateUserRole(user, roleEntity));
  //  }

  //  public override async Task RemoveFromRoleAsync(IdentityUserNav<TKey> user, string normalizedRoleName, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
  //      throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
  //    }
  //    var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
  //    if (roleEntity != null) {
  //      var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
  //      if (userRole != null) {
  //        UserRoles.xDeleteAsync(userRole);
  //      }
  //    }
  //  }

  //  public override async Task<IList<string>> GetRolesAsync(IdentityUserNav<TKey> user, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    var userId = user.Id;
  //    var query = from userRole in UserRoles
  //                join role in Roles on userRole.RoleId equals role.Id
  //                where userRole.UserId.Equals(userId)
  //                select role.Name;
  //    return await query.xToListAsync(cancellationToken);
  //  }

  //  public override async Task<bool> IsInRoleAsync(IdentityUserNav<TKey> user, string normalizedRoleName, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (user == null) {
  //      throw new ArgumentNullException(nameof(user));
  //    }
  //    if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
  //      throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
  //    }
  //    var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
  //    if (role != null) {
  //      var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
  //      return userRole != null;
  //    }
  //    return false;
  //  }

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
  //    return Users.xFirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
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
  //    return await query.xToListAsync(cancellationToken);
  //  }

  //  public override async Task<IList<IdentityUserNav<TKey>>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default) {
  //    cancellationToken.ThrowIfCancellationRequested();
  //    ThrowIfDisposed();
  //    if (string.IsNullOrEmpty(normalizedRoleName)) {
  //      throw new ArgumentNullException(nameof(normalizedRoleName));
  //    }

  //    var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

  //    if (role != null) {
  //      var query = from userrole in UserRoles
  //                  join user in Users on userrole.UserId equals user.Id
  //                  where userrole.RoleId.Equals(role.Id)
  //                  select user;

  //      return await query.xToListAsync(cancellationToken);
  //    }
  //    return new List<IdentityUserNav<TKey>>();
  //  }

  //  protected override Task<IdentityUserTokenNav<TKey>> FindTokenAsync(IdentityUserNav<TKey> user, string loginProvider, string name, CancellationToken cancellationToken)
  //    => (from ut in UserTokens
  //        where ut.UserId.Equals(user.Id)
  //        && ut.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase)
  //        && ut.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
  //        select ut).xFirstOrDefaultAsync(cancellationToken);

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
