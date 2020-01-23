using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Messaging.Interface;
using AirPro.Notifications.WebJob.Models;
using Dapper;

namespace AirPro.Notifications.WebJob
{
    internal class Contacts
    {
        private readonly IDbConnection _connection;

        internal Contacts(IDbConnection connection)
        {
            _connection = connection;
        }

        internal async Task<IEnumerable<INotificationDestination>> GetStatementContacts(Guid shopGuid)
        {
            return (from u in await GetNotificationUsers(shopGuid)
                    where u.ShopStatementNotification && !u.DisableShopStatementNotification
                    select
                        new NotificationDestinationModel
                        {
                            Email = (u.EmailConfirmed ? u.Email : null),
                            Text = (u.PhoneNumberConfirmed ? u.PhoneNumber : null),
                            TimeZoneInfoId = u.TimeZoneInfoId
                        }).ToArray();
        }

        internal async Task<IEnumerable<INotificationDestination>> GetReportingContacts(Guid shopGuid)
        {
            return (from u in await GetNotificationUsers(shopGuid)
                    where u.ShopReportNotification
                    select
                    new NotificationDestinationModel
                    {
                        Email = (u.EmailConfirmed ? u.Email : null),
                        Text = (u.PhoneNumberConfirmed ? u.PhoneNumber : null),
                        TimeZoneInfoId = u.TimeZoneInfoId
                    }).ToArray();
        }

        internal async Task<IEnumerable<INotificationDestination>> GetBillingContacts(Guid shopGuid)
        {
            return (from u in await GetNotificationUsers(shopGuid)
                    where u.ShopBillingNotification && !u.DisableShopBillingNotification
                    select
                    new NotificationDestinationModel
                    {
                        Email = (u.EmailConfirmed ? u.Email : null),
                        Text = (u.PhoneNumberConfirmed ? u.PhoneNumber : null),
                        TimeZoneInfoId = u.TimeZoneInfoId
                    }).ToArray();
        }

        private async Task<IEnumerable<UserMembershipModel>> GetNotificationUsers(Guid shopGuid)
        {
            // Load Users.
            var users = await _connection.QueryAsync<UserMembershipModel>("EXEC Notification.usp_GetUserMemberships @ShopGuid", new { ShopGuid = shopGuid });

            // Filters Users.
            return users.Where(u => u.EmailConfirmed || u.PhoneNumberConfirmed).ToArray();
        }
    }
}