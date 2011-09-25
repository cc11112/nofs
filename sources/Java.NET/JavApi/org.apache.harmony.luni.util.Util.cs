/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */
using System;
using System.Text;
using java = biz.ritter.javapi;

namespace org.apache.harmony.luni.util
{
    public sealed class Util
    {
        public static String toASCIILowerCase(String s)
        {
            int len = s.length();
            StringBuilder buffer = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                char c = s.charAt(i);
                if ('A' <= c && c <= 'Z')
                {
                    buffer.append((char)(c + ('a' - 'A')));
                }
                else
                {
                    buffer.append(c);
                }
            }
            return buffer.toString();
        }

        public static String toASCIIUpperCase(String s)
        {
            int len = s.length();
            StringBuilder buffer = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                char c = s.charAt(i);
                if ('a' <= c && c <= 'z')
                {
                    buffer.append((char)(c - ('a' - 'A')));
                }
                else
                {
                    buffer.append(c);
                }
            }
            return buffer.toString();
        }
        public static String convertUTF8WithBuf(byte[] buf, char[] outJ, int offset,
                int utfSize) {//throws UTFDataFormatException {
            int count = 0, s = 0, a;
            while (count < utfSize) {
                if ((outJ[s] = (char) buf[offset + count++]) < '\u0080')
                    s++;
                else if (((a = outJ[s]) & 0xe0) == 0xc0) {
                    if (count >= utfSize)
                        throw new java.io.UTFDataFormatException("Second byte at "+count+" does not match UTF8 Specification");
                    int b = buf[count++];
                    if ((b & 0xC0) != 0x80)
                        throw new java.io.UTFDataFormatException("Second byte at " + count + " does not match UTF8 Specification");
                    outJ[s++] = (char) (((a & 0x1F) << 6) | (b & 0x3F));
                } else if ((a & 0xf0) == 0xe0) {
                    if (count + 1 >= utfSize)
                        throw new java.io.UTFDataFormatException("Third byte at " + count + " does not match UTF8 Specification");
                    int b = buf[count++];
                    int c = buf[count++];
                    if (((b & 0xC0) != 0x80) || ((c & 0xC0) != 0x80))
                        throw new java.io.UTFDataFormatException("Second or third byte at " + count + " does not match UTF8 Specification");
                    outJ[s++] = (char) (((a & 0x0F) << 12) | ((b & 0x3F) << 6) | (c & 0x3F));
                } else {
                    throw new java.io.UTFDataFormatException("Input at " + count + " does not match UTF8 Specification");
                }
            }
            return new String(outJ, 0, s);
        }
    }
}
