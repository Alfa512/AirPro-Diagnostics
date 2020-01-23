using System;
using System.Linq;
using System.Security.Principal;
using AirPro.Entities;
using AirPro.Entities.Access;
using AirPro.Logging;

namespace AirPro.Library
{
    public abstract class BaseLibrary : IDisposable
    {
        protected EntityDbContext Db { get; }
        public UserEntityModel User { get; }
        protected IIdentity UserIdentity { get; }

        protected BaseLibrary() : this(new EntityDbContext()) { }

        protected BaseLibrary(EntityDbContext context)
        {
            try
            {
                if (context == null) throw new ArgumentNullException(nameof(context));
                Db = context;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                throw;
            }
        }

        protected BaseLibrary(EntityDbContext context, IIdentity user) : this(context)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                if (!user.IsAuthenticated) throw new Exception(@"Identity Not Authenticated.");

                if (!Db.Users.Any(u => u.UserName == user.Name))
                    throw new Exception(@"User Account Not Found.");

                var result = Db.Users.Single(u => u.UserName == user.Name);
                if (result.GetAccountLocked) throw new Exception(@"User Account is Locked Out.");

                User = result;
                UserIdentity = user;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                throw;
            }
        }

        public virtual void Dispose() { }
    }
}
