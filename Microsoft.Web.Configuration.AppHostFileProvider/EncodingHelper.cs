// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.ApplicationHost
{
    internal class EncodingHelper
    {
        private static int[] DecodeArray = new int[256]
        {
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 62, 64, 64, 64, 63, 52, 53,
        54, 55, 56, 57, 58, 59, 60, 61, 64, 64,
        64, 64, 64, 64, 64, 0, 1, 2, 3, 4,
        5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
        15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
        25, 64, 64, 64, 64, 64, 64, 26, 27, 28,
        29, 30, 31, 32, 33, 34, 35, 36, 37, 38,
        39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
        49, 50, 51, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64
        };

        private static char[] EncodeArray = new char[64]
        {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
        'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
        'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
        '8', '9', '+', '/'
        };

        private static int EncodeMask_Octal_60 = 48;

        private static int EncodeMask_Octal_17 = 15;

        private static int EncodeMask_Octal_74 = 60;

        private static int EncodeMask_Octal_77 = 63;

        public static string Encode(byte[] data)
        {
            int num = data.Length;
            int num2 = num / 3 * 4;
            if (num % 3 > 0)
            {
                num2 += 4;
            }
            char[] array = new char[num2];
            int num3 = 0;
            int num4 = 0;
            while (num >= 3)
            {
                array[num4++] = EncodeFirstChar(data[num3]);
                array[num4++] = EncodeSecondChar(data[num3], data[num3 + 1]);
                array[num4++] = EncodeThirdChar(data[num3 + 1], data[num3 + 2]);
                array[num4++] = EncodeFourthChar(data[num3 + 2]);
                num3 += 3;
                num -= 3;
            }
            switch (num)
            {
                case 2:
                    array[num4++] = EncodeFirstChar(data[num3]);
                    array[num4++] = EncodeSecondChar(data[num3], data[num3 + 1]);
                    array[num4++] = EncodeThirdChar(data[num3 + 1], 0);
                    array[num4++] = '=';
                    break;
                case 1:
                    array[num4++] = EncodeFirstChar(data[num3]);
                    array[num4++] = EncodeSecondChar(data[num3], 0);
                    array[num4++] = '=';
                    array[num4++] = '=';
                    break;
            }
            return new string(array);
        }

        private static char EncodeFirstChar(byte value)
        {
            return EncodeArray[value >> 2];
        }

        private static char EncodeSecondChar(byte byte1, byte byte2)
        {
            return EncodeArray[((byte1 << 4) & EncodeMask_Octal_60) | ((byte2 >> 4) & EncodeMask_Octal_17)];
        }

        private static char EncodeThirdChar(byte byte2, byte byte3)
        {
            return EncodeArray[((byte2 << 2) & EncodeMask_Octal_74) | ((byte3 >> 6) & 3)];
        }

        private static char EncodeFourthChar(byte byte3)
        {
            return EncodeArray[byte3 & EncodeMask_Octal_77];
        }

        public static byte[] Decode(string data)
        {
            data = data.TrimEnd('=');
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            int num = bytes.Length / 4 * 3;
            if (bytes.Length % 4 > 0)
            {
                num += bytes.Length % 4 - 1;
            }
            if (num == 0)
            {
                return null;
            }
            byte[] array = new byte[num];
            int num2 = bytes.Length;
            int num3 = 0;
            int num4 = 0;
            while (num2 >= 4)
            {
                array[num3++] = DecodeFirstByte(bytes[num4], bytes[num4 + 1]);
                array[num3++] = DecodeSecondByte(bytes[num4 + 1], bytes[num4 + 2]);
                array[num3++] = DecodeThirdByte(bytes[num4 + 2], bytes[num4 + 3]);
                num4 += 4;
                num2 -= 4;
            }
            switch (num2)
            {
                case 3:
                    array[num3++] = DecodeFirstByte(bytes[num4], bytes[num4 + 1]);
                    array[num3++] = DecodeSecondByte(bytes[num4 + 1], bytes[num4 + 2]);
                    break;
                case 2:
                    array[num3++] = DecodeFirstByte(bytes[num4], bytes[num4 + 1]);
                    break;
                default:
                    _ = 1;
                    break;
            }
            return array;
        }

        private static byte DecodeFirstByte(byte byte1, byte byte2)
        {
            return (byte)((DecodeArray[byte1] << 2) | (DecodeArray[byte2] >> 4));
        }

        private static byte DecodeSecondByte(byte byte2, byte byte3)
        {
            return (byte)((DecodeArray[byte2] << 4) | (DecodeArray[byte3] >> 2));
        }

        private static byte DecodeThirdByte(byte byte3, byte byte4)
        {
            return (byte)((DecodeArray[byte3] << 6) | DecodeArray[byte4]);
        }
    }
}
