// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public interface ICredentialBuilder
    {
        CredentialInfo SetCredentials(
            IServiceProvider serviceProvider,
            bool verifyValidWindowsUserPassword
            );

        CredentialInfo SetCredentials(
            IServiceProvider serviceProvider,
            bool verifyValidWindowsUserPassword,
            EventHandler showHelp
            );

        CredentialInfo SpecifyCredentials(
            IServiceProvider serviceProvider,
            string dialogTitleText,
            string userName,
            string password,
            bool isModify,
            bool verifyValidWindowsUserPassword,
            string description,
            string setSpecificCredentialsText,
            string otherOptionText
            );

        CredentialInfo SpecifyCredentials(
            IServiceProvider serviceProvider,
            string dialogTitleText,
            string userName,
            string password,
            bool isModify,
            bool verifyValidWindowsUserPassword,
            string description,
            string setSpecificCredentialsText,
            string otherOptionText,
            EventHandler<CredentialInfoEventArgs> commitHandler
            );

        CredentialInfo SpecifyCredentials(
            IServiceProvider serviceProvider,
            string dialogTitleText,
            string userName,
            string password,
            bool isModify,
            bool verifyValidWindowsUserPassword,
            string description,
            string setSpecificCredentialsText,
            string otherOptionText,
            EventHandler<CredentialInfoEventArgs> commitHandler,
            EventHandler showHelp
            );

        CredentialInfo SpecifyCredentials(
            IServiceProvider serviceProvider,
            string dialogTitleText,
            string userName,
            string password,
            bool isModify,
            bool verifyValidWindowsUserPassword,
            string description,
            string setSpecificCredentialsText,
            string otherOptionText,
            EventHandler<CredentialInfoEventArgs> commitHandler,
            EventHandler showHelp,
            bool readOnly
            );
    }
}