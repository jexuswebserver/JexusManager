// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class IntegerRangeValidator : ConfigurationValidatorBase
    {
        private readonly string[] _items;

        private bool _initialized;

        private readonly bool _excluded;

        private uint _minUint;

        private uint _maxUint;

        private int _minInt;

        private int _maxInt;

        public IntegerRangeValidator(string range)
        {
            _items = range.Split(',');
            _excluded = _items.Length > 2 && _items[2] == "exclude";
        }

        public override void Validate(object value)
        {
            if (value is uint)
            {
                if (!_initialized)
                {
                    _minUint = uint.Parse(_items[0]);
                    _maxUint = uint.Parse(_items[1]);
                    _initialized = true;
                }

                var data = (uint)value;
                if (_excluded)
                {
                    if (data > _minUint && data < _maxUint)
                    {
                        throw new COMException(string.Format("Integer value must not be between {0} and {1} inclusive\r\n", _minUint, _maxUint));
                    }
                }
                else
                {
                    if (data < _minUint || data > _maxUint)
                    {
                        throw new COMException(string.Format("Integer value must be between {0} and {1} inclusive\r\n", _minUint, _maxUint));
                    }
                }
            }
            else if (value is int)
            {
                if (!_initialized)
                {
                    _minInt = int.Parse(_items[0]);
                    _maxInt = int.Parse(_items[1]);
                    _initialized = true;
                }

                var data = (int)value;
                if (_excluded)
                {
                    if (data > _minInt && data < _maxInt)
                    {
                        throw new COMException(string.Format("Integer value must be between {0} and {1} exclusive\r\n", _minInt, _maxInt));
                    }
                }
                else
                {
                    if (data < _minInt || data > _maxInt)
                    {
                        throw new COMException(string.Format("Integer value must be between {0} and {1} inclusive\r\n", _minInt, _maxInt));
                    }
                }
            }
        }
    }
}
