// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;

    public static class TimeSpanExtensions
    {
        public static int GetTotalMinutes(this TimeSpan time)
        {
            return time.Minutes + (time.Hours + time.Days * 24) * 60;
        }

        public static long GetTotalSeconds(this TimeSpan time)
        {
            return time.Seconds + (time.Minutes + (time.Hours + time.Days * 24) * 60) * 60;
        }
    }
}
