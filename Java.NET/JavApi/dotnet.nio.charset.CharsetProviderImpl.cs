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
 *  
 */
using System;
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.dotnet.nio.charset
{
    public class CharsetProviderImpl : java.nio.charset.spi.CharsetProvider {

        public CharsetProviderImpl() {
        }

        /**
         * Answers an iterator over the list of available charsets.
         * 
         * @return available charsets.
         */
        public override java.util.Iterator<java.nio.charset.Charset> charsets() {
            java.util.ArrayList<java.nio.charset.Charset> charset = new java.util.ArrayList<java.nio.charset.Charset>();
            EncodingInfo [] ei = Encoding.GetEncodings();
            foreach (EncodingInfo info in ei)
            {
                CharsetImpl ci = new CharsetImpl(info.GetEncoding());
                charset.add(ci);
            }
            return charset.iterator();
        }

        /**
         * Answers the charset with the given canonical or alias name.
         * 
         * Subsequent requests for the same charset will answer the same instance.
         * If the charset is unavailable the method returns <code>null</code>.
         * 
         * @param charsetName
         *            the name of a character set.
         * @return the charset requested, or <code>null</code> if unavailable.
         */
        public override java.nio.charset.Charset charsetForName(String charsetName) {
            return new CharsetImpl(Encoding.GetEncoding(charsetName));
        }



    }

    internal class CharsetImpl : java.nio.charset.Charset
    {
        private readonly Encoding encoding;
        internal CharsetImpl (Encoding enc) : base (enc.WebName, new String [] {}) {
            this.encoding = enc;
        }
        public Encoding getEncoding()
        {
            return this.encoding;
        }
        /**
         * Determines whether this charset is a super set of the given charset.
         * 
         * @param charset
         *            a given charset.
         * @return true if this charset is a super set of the given charset,
         *         false if it's unknown or this charset is not a superset of
         *         the given charset.
         */
        public override bool contains(java.nio.charset.Charset charset)
        {
            return false;
        }

        /**
         * Gets a new instance of an encoder for this charset.
         * 
         * @return a new instance of an encoder for this charset.
         */
        public override java.nio.charset.CharsetEncoder newEncoder()
        {
            return new CharsetEncoderImpl(this);
        }

        /**
         * Gets a new instance of a decoder for this charset.
         * 
         * @return a new instance of a decoder for this charset.
         */
        public override java.nio.charset.CharsetDecoder newDecoder()
        {
            return new CharsetDecoderImpl(this);
        }

    }
    internal class CharsetDecoderImpl : java.nio.charset.CharsetDecoder {
        private readonly CharsetImpl cs;
        public CharsetDecoderImpl(CharsetImpl cs) : base (cs,CharsetEncoderImpl.fiveValue,CharsetEncoderImpl.fiveValue) /// HACK: calculate 5 values
        {
            this.cs = cs;
        }
        protected override java.nio.charset.CoderResult decodeLoop(java.nio.ByteBuffer inJ, java.nio.CharBuffer outJ)
        {
            Encoding enc = this.cs.getEncoding();
            byte[] input = new byte[inJ.capacityJ-inJ.positionJ];
            inJ.get(input);
            char[] output = new char[input.Length * CharsetEncoderImpl.fiveValue];
            int size = enc.GetDecoder().GetChars(input, 0, input.Length, output, 0, true);
            outJ.put(output, 0, size);
            return java.nio.charset.CoderResult.UNDERFLOW;
        }
    }
    internal class CharsetEncoderImpl : java.nio.charset.CharsetEncoder {
        public const int fiveValue = 5;//! HACK: calculate 5 values
        private readonly CharsetImpl cs;
        public CharsetEncoderImpl(CharsetImpl cs) : base (cs,1,fiveValue) 
        {
            this.cs = cs;
        }
        protected override java.nio.charset.CoderResult encodeLoop(java.nio.CharBuffer inJ, java.nio.ByteBuffer outJ)
        {
            Encoding enc = this.cs.getEncoding();
            char[] input = new char[inJ.capacityJ-inJ.positionJ];
            inJ.get(input);
            byte[] output = new byte[input.Length * fiveValue];
            int size = enc.GetEncoder().GetBytes(input, 0, input.Length, output, 0, true);
            outJ.put(output, 0, size);
            outJ = java.nio.ByteBuffer.wrap((byte[])outJ.array(), 0, size);
            return java.nio.charset.CoderResult.UNDERFLOW;
        }
    }
}
