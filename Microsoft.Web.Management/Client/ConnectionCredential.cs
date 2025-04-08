// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class ConnectionCredential
    {
        public ConnectionCredential(string userName, string password)
            : this(userName, password, false)
        {
        }

        public ConnectionCredential(string userName, string password, bool useBasicAuthentication)
        {
            UserName = userName;
            Password = password;
            UseBasicAuthentication = useBasicAuthentication;
        }

        public static readonly ConnectionCredential CurrentUserAccount;

        public override bool Equals(object obj)
        {
            var connection = obj as ConnectionCredential;
            return connection != null && connection.UserName == UserName;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public string Domain { get; }
        public string Password { get; }
        public bool UseBasicAuthentication { get; }
        public string UserName { get; }
        public bool UseSystemAccount { get; }
    }
}
