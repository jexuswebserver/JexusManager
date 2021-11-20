// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Windows.Win32.Networking.WinSock;

partial struct SOCKADDR_STORAGE
{
    public static explicit operator SOCKADDR_STORAGE(SOCKADDR_IN address)
    {
        var length = Marshal.SizeOf<SOCKADDR_STORAGE>();
        var pointer = Marshal.AllocCoTaskMem(length);
        unsafe
        {
            Unsafe.InitBlockUnaligned((void*)pointer, 0, (uint)length);
        }
        Marshal.StructureToPtr(address, pointer, false);
        var result = Marshal.PtrToStructure<SOCKADDR_STORAGE>(pointer);
        Marshal.FreeCoTaskMem(pointer);
        return result;
    }
}
