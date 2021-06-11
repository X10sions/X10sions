using Microsoft.AspNetCore.Identity;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityStore {
    IdentityErrorDescriber ErrorDescriber { get; }

    //Func<T, string> GetRoleOrUserDescription { get; }
    //Task<bool> CreateAsync_Insert(T roleOrUser, CancellationToken cancellationToken = default);
    //Task<bool> DeleteAsync_Delete(T roleOrUser, CancellationToken cancellationToken = default);
  }
}