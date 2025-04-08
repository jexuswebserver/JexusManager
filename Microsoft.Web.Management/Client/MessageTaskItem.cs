// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public sealed class MessageTaskItem : TaskItem
    {
        public MessageTaskItem(
            MessageTaskItemType messageType,
            string text,
            string category
            ) : this(messageType, text, category, string.Empty)
        { }

        public MessageTaskItem(
            MessageTaskItemType messageType,
            string text,
            string category,
            string description
            ) : this(messageType, text, category, description, string.Empty, null)
        { }

        public MessageTaskItem(
            MessageTaskItemType messageType,
            string text,
            string category,
            string description,
            string methodName,
            Object userData
            ) : base(text, category, description)
        {
            MessageType = messageType;
            MethodName = methodName;
            UserData = userData;
        }

        public MessageTaskItemType MessageType { get; }
        public string MethodName { get; }
        public Object UserData { get; }
    }
}
