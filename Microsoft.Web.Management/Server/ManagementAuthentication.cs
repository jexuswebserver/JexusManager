// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public static class ManagementAuthentication
    {
        public static bool AuthenticateUser(
            string userName,
            string password
            )
        {
            return Provider.AuthenticateUser(userName, password);
        }

        public static ManagementUserInfo CreateUser(
            string userName,
            string password
            )
        {
            return Provider.CreateUser(userName, password);
        }

        public static void DeleteUser(
            string userName
            )
        {
            Provider.DeleteUser(userName);
        }

        public static void DisableUser(
            string userName
            )
        { Provider.DisableUser(userName); }

        public static void EnableUser(
            string userName
            )
        {
            Provider.EnableUser(userName);
        }

        public static ManagementUserInfo GetUser(
            string userName
            )
        {
            return Provider.GetUser(userName);
        }

        public static ManagementUserInfoCollection GetUsers(
            int itemIndex,
            int itemsPerPage
            )
        {
            return Provider.GetUsers(itemIndex, itemsPerPage);
        }

        public static InvalidPasswordReason IsPasswordStrongEnough(
            string password
            )
        {
            return Provider.IsPasswordStrongEnough(password);
        }

        public static void SetPassword(
            string userName,
            string password
            )
        { Provider.SetPassword(userName, password); }

        public static ManagementAuthenticationProvider Provider { get; }
    }
}
