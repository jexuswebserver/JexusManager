// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Windows.Win32.Networking.WinSock;

partial struct SOCKADDR_IN
{
    public static explicit operator SOCKADDR_IN(SOCKADDR_STORAGE storage)
    {
        var pointer = Marshal.AllocCoTaskMem(Marshal.SizeOf<SOCKADDR_STORAGE>());
        Marshal.StructureToPtr(storage, pointer, false);
        var result = Marshal.PtrToStructure<SOCKADDR_IN>(pointer);
        Marshal.FreeCoTaskMem(pointer);
        return result;
    }
}
