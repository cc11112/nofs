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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util.jar
{

    internal class InitManifest
    {

        private byte[] buf;

        private int pos;

        internal AttributesNS.Name name;

        internal String value;

        java.nio.charset.CharsetDecoder decoder = java.nio.charset.Charset.forName("UTF-8").newDecoder();// ThreadLocalCache.utf8Decoder.get();
        java.nio.CharBuffer cBuf = java.nio.CharBuffer.allocate(72);// ThreadLocalCache.charBuffer.get();

        internal InitManifest(byte[] buf, Attributes main, AttributesNS.Name ver)
        {//throws IOException {

            this.buf = buf;

            // check a version attribute
            if (!readHeader() || (ver != null && !name.equals(ver)))
            {
                throw new java.io.IOException("Missing version attribute: " + ver); //$NON-NLS-1$
            }

            main.put(name, value);
            while (readHeader())
            {
                main.put(name, value);
            }
        }

        internal void initEntries(Map<String, Attributes> entries,
                Map<String, Chunk> chunks)
        {//throws IOException {

            int mark = pos;
            while (readHeader())
            {
                if (!AttributesNS.Name.NAME.equals(name))
                {
                    throw new java.io.IOException("Entry is not named"); //$NON-NLS-1$
                }
                String entryNameValue = value;

                Attributes entry = entries.get(entryNameValue);
                if (entry == null)
                {
                    entry = new Attributes(12);
                }

                while (readHeader())
                {
                    entry.put(name, value);
                }

                if (chunks != null)
                {
                    if (chunks.get(entryNameValue) != null)
                    {
                        // TODO A bug: there might be several verification chunks for
                        // the same name. I believe they should be used to update
                        // signature in order of appearance; there are two ways to fix
                        // this: either use a list of chunks, or decide on used
                        // signature algorithm in advance and reread the chunks while
                        // updating the signature; for now a defensive error is thrown
                        throw new java.io.IOException("A jar verifier does not support more than one entry with the same name"); //$NON-NLS-1$
                    }
                    chunks.put(entryNameValue, new Chunk(mark, pos));
                    mark = pos;
                }

                entries.put(entryNameValue, entry);
            }
        }

        internal int getPos()
        {
            return pos;
        }

        /**
         * Number of subsequent line breaks.
         */
        internal int linebreak = 0;

        /**
         * Read a single line from the manifest buffer.
         */
        private bool readHeader()
        {//throws IOException {
            if (linebreak > 1)
            {
                // break a section on an empty line
                linebreak = 0;
                return false;
            }
            readName();
            linebreak = 0;
            readValue();
            // if the last line break is missed, the line
            // is ignored by the reference implementation
            return linebreak > 0;
        }

        private byte[] wrap(int mark, int pos)
        {
            byte[] buffer = new byte[pos - mark];
            java.lang.SystemJ.arraycopy(buf, mark, buffer, 0, pos - mark);
            return buffer;
        }

        private void readName()
        {//throws IOException {
            int i = 0;
            int mark = pos;

            while (pos < buf.Length)
            {
                byte b = buf[pos++];

                if (b == ':')
                {
                    byte[] nameBuffer = wrap(mark, pos - 1);

                    if (buf[pos++] != ' ')
                    {
                        throw new java.io.IOException("Invalid attribute " + nameBuffer); //$NON-NLS-1$
                    }

                    name = new AttributesNS.Name(nameBuffer);
                    return;
                }

                if (!((b >= 'a' && b <= 'z') || (b >= 'A' && b <= 'Z') || b == '_'
                        || b == '-' || (b >= '0' && b <= '9')))
                {
                    throw new java.io.IOException("Invalid attribute " + b); //$NON-NLS-1$
                }
            }
            if (i > 0)
            {
                throw new java.io.IOException("Invalid attribute " + wrap(mark, buf.Length)); //$NON-NLS-1$
            }
        }

        private void readValue()
        {//throws IOException {
            byte next;
            bool lastCr = false;
            int mark = pos;
            int last = pos;

            decoder.reset();
            cBuf.clear();

            while (pos < buf.Length)
            {
                next = buf[pos++];

                switch (next)
                {
                    case 0:
                        throw new java.io.IOException("NUL character in a manifest"); //$NON-NLS-1$
                    case (byte)'\n':
                        if (lastCr)
                        {
                            lastCr = false;
                        }
                        else
                        {
                            linebreak++;
                        }
                        continue;
                    case (byte)'\r':
                        lastCr = true;
                        linebreak++;
                        continue;
                    case (byte)' ':
                        if (linebreak == 1)
                        {
                            decode(mark, last, false);
                            mark = pos;
                            linebreak = 0;
                            continue;
                        }
                        break;
                }

                if (linebreak >= 1)
                {
                    pos--;
                    break;
                }
                last = pos;
            }

            decode(mark, last, true);
            while (java.nio.charset.CoderResult.OVERFLOW == decoder.flush(cBuf))
            {
                enlargeBuffer();
            }
            value = new String((char[])cBuf.array(), cBuf.arrayOffset(), cBuf.position());
        }

        private void decode(int mark, int pos, bool endOfInput)
        {//throws IOException {
            java.nio.ByteBuffer bBuf = java.nio.ByteBuffer.wrap(buf, mark, pos - mark);
            while (java.nio.charset.CoderResult.OVERFLOW == decoder.decode(bBuf, cBuf, endOfInput))
            {
                enlargeBuffer();
            }
        }

        private void enlargeBuffer()
        {
            java.nio.CharBuffer newBuf = java.nio.CharBuffer.allocate(cBuf.capacity() * 2);
            newBuf.put((char[])cBuf.array(), cBuf.arrayOffset(), cBuf.position());
            cBuf = newBuf;
            //ThreadLocalCache.charBuffer.set(cBuf);
        }
    }
}
