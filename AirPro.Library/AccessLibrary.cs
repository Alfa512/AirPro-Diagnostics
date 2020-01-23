using System;
using System.Threading.Tasks;
using System.Web;
using AirPro.Entities;
using AirPro.Entities.Access;
using AirPro.Logging;

namespace AirPro.Library
{
    public class AccessLibrary : BaseLibrary
    {
        public static async Task LogLoginAttempt(HttpRequestBase request, string loginName, Guid? userGuid = null, bool lockedOut = false, bool twoFactorChallenge = false, bool twoFactorVerified = false)
        {
            try
            {
                using (var context = new EntityDbContext())
                {
                    LoginEntityModel login = new LoginEntityModel
                    {
                        UserGuid = userGuid,
                        LoginName = loginName,
                        UserAgent = request.UserAgent,
                        UserHostAddress = request.UserHostAddress,
                        UserHostName = request.UserHostName,
                        AccountLockedOut = lockedOut,
                        TwoFactorChallenge = twoFactorChallenge,
                        TwoFactorVerified = twoFactorVerified,
                        LoginAttemptDt = DateTimeOffset.UtcNow
                    };

                    context.Entry(login).State = System.Data.Entity.EntityState.Added;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                throw;
            }
        }
    }
}
