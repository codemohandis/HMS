﻿using HMS.DataBase;
using HMS.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class HMSUserManager :UserManager<IdentityRoles>
    {
        public HMSUserManager(IUserStore<IdentityRoles> store)
           : base(store)
        {
        }

        public static HMSUserManager Create(IdentityFactoryOptions<HMSUserManager> options, IOwinContext context)
        {
            var manager = new HMSUserManager(new UserStore<IdentityRoles>(context.Get<HMSContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<IdentityRoles>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityRoles>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityRoles>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            // manager.EmailService = new EmailService();
           //  manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =  new DataProtectorTokenProvider<IdentityRoles>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
