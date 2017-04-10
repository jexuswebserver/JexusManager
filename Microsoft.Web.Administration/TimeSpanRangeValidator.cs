// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class TimeSpanRangeValidator : ConfigurationValidatorBase
    {
        private readonly bool _exclude;

        private readonly long _minimum;

        private readonly long _maximum;

        private readonly long _granularity;

        public TimeSpanRangeValidator(string range)
        {
            var items = range.Split(',');
            _minimum = long.Parse(items[0]);
            _maximum = long.Parse(items[1]);
            _granularity = long.Parse(items[2]);
            _exclude = items.Length > 3 && items[3] == "exclude";
        }

        public override void Validate(object value)
        {
            var data = (TimeSpan)value;
            if (data == Timeout.InfiniteTimeSpan)
            {
                return;
            }

            var seconds = data.Ticks / 10000000L;
            if (_exclude)
            {
                if (seconds > _minimum && seconds < _maximum && (seconds - _minimum) % _granularity != 0)
                {
                    throw new COMException(
                        string.Format(
                            "Timespan value must not be between {0} and {1} seconds inclusive, with a granularity of {2} seconds\r\n",
                            TimeSpan.FromSeconds(_minimum),
                            TimeSpan.FromSeconds(_maximum),
                            _granularity));
                }
            }
            else
            {
                if (seconds < _minimum || seconds > _maximum || (seconds - _minimum) % _granularity != 0)
                {
                    if (_minimum != 1L || (seconds % _granularity != 0))
                    {
                        // IMPORTANT: workaround sessionState / timeout's definition using 1 issue.
                        throw new COMException(
                            string.Format(
                                "Timespan value must be between {0} and {1} seconds inclusive, with a granularity of {2} seconds\r\n",
                                TimeSpan.FromSeconds(_minimum),
                                TimeSpan.FromSeconds(_maximum),
                                _granularity));
                    }
                }
            }
        }
    }
}
