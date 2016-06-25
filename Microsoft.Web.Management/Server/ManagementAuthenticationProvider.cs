// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Security.Principal;

namespace Microsoft.Web.Management.Server
{
    public abstract class ManagementAuthenticationProvider
    {
        public abstract bool AuthenticateUser(
            string userName,
            string password
            );

        protected virtual IPrincipal CreatePrincipal(
            string userName
            )
        {
            return null;
        }

        public abstract ManagementUserInfo CreateUser(
            string userName,
            string password
            );

        public abstract void DeleteUser(
            string userName
            );

        public abstract void DisableUser(
            string userName
            );

        public abstract void EnableUser(
            string userName
            );

        public abstract ManagementUserInfo GetUser(
            string userName
            );

        public abstract ManagementUserInfoCollection GetUsers(
            int itemIndex,
            int itemsPerPage
            );

        public virtual void Initialize(
            IDictionary<string, string> args
            )
        { }

        public abstract InvalidPasswordReason IsPasswordStrongEnough(
            string password
            );

        public abstract void SetPassword(
            string userName,
            string newPassword
            );
    }
}