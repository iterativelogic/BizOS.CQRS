using System;
using System.Threading;
using System.Threading.Tasks;

namespace Security.Application.Contracts
{
  public interface IUserVerifyService
  {
    Task<bool> VerifyUserAsync(Guid tenantId, string userName, string password, CancellationToken cancellationToken);
  }
}