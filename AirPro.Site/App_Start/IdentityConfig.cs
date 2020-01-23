using AirPro.Entities;
using AirPro.Entities.Access;
using AirPro.Messaging;
using AirPro.Messaging.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AirPro.Site
{
    internal class NotificationMessage : INotificationMessage
    {
        public IEnumerable<INotificationMessageAttachment> Attachments { get; set; }
        public IEnumerable<INotificationDestination> Destinations { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string TextMessage { get; set; }
    }

    internal class NotificationDestination : INotificationDestination
    {
        public string Email { get; set; }
        public string Text { get; set; }
        public string TimeZoneInfoId { get; set; }
    }

    internal class MessageSettings : IMessagingSettings
    {
        public string TwilioSid { get; set; }
        public string TwilioToken { get; set; }
        public string TwilioFromPhone { get; set; }
        public string SendGridAccount { get; set; }
        public string SendGridAccountKey { get; set; }
        public string SendGridFromAddress { get; set; }
        public string SendGridFromName { get; set; }
    }

    internal static class ServiceSettings
    {
        public static string OverrideEmail => ConfigurationManager.AppSettings["OverrideEmail"];

        public static MessageSettings Settings => new MessageSettings
        {
            SendGridAccount = ConfigurationManager.AppSettings["SendGridAccount"],
            SendGridAccountKey = ConfigurationManager.AppSettings["SendGridApiKey"],
            SendGridFromAddress = ConfigurationManager.AppSettings["MailFromAddress"],
            SendGridFromName = ConfigurationManager.AppSettings["MailFromName"],
            TwilioSid = ConfigurationManager.AppSettings["TwilioSid"],
            TwilioToken = ConfigurationManager.AppSettings["TwilioToken"],
            TwilioFromPhone = ConfigurationManager.AppSettings["TwilioFromPhone"]
        };
    }

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var messaging = new Messages(ServiceSettings.Settings);

            var msg = new NotificationMessage
            {
                Destinations = new[] {new NotificationDestination {Email = message.Destination}},
                EmailSubject = message.Subject,
                EmailBody = message.Body
            };

            await messaging.SendMailMessageAsync(msg, ServiceSettings.OverrideEmail).ConfigureAwait(false);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var messaging = new Messages(ServiceSettings.Settings);

            var msg = new NotificationMessage
            {
                Destinations = new[] {new NotificationDestination {Text = message.Destination}},
                TextMessage = message.Body
            };

            await messaging.SendTextMessageAsync(msg, ServiceSettings.OverrideEmail).ConfigureAwait(false);
        }
    }

    // Configure the application user manager which is used in this application.
    public class ApplicationUserManager : UserManager<UserEntityModel, Guid>
    {
        public ApplicationUserManager(IUserStore<UserEntityModel, Guid> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var userStore = new UserStore<UserEntityModel, RoleEntityModel, Guid, UserLoginEntityModel, UserRoleEntityModel, UserClaimEntityModel>(context.Get<EntityDbContext>());
            var manager = new ApplicationUserManager(userStore);

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<UserEntityModel, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<UserEntityModel, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<UserEntityModel, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<UserEntityModel, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.  
    public class ApplicationSignInManager : SignInManager<UserEntityModel, Guid>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserEntityModel user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public override Guid ConvertIdFromString(string id)
        {
            return string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id);
        }

        public override string ConvertIdToString(Guid id)
        {
            return id.Equals(Guid.Empty) ? string.Empty : id.ToString();
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
