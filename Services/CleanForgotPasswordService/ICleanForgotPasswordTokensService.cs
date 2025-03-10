namespace reymani_web_api.Services.CleanForgotPasswordService;


using System.Threading;
using System.Threading.Tasks;

public interface ICleanForgotPasswordTokensService
{
  Task CleanExpiredTokensAsync(CancellationToken cancellationToken = default);
}
