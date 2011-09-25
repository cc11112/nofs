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
    /**
     * The {@code Manifest} class is used to obtain attribute information for a
     * {@code JarFile} and its entries.
     */
    public class Manifest : java.lang.Cloneable {
        /**
         * Manifest bytes are used for delayed entry parsing.
         */
        private InitManifest im;

        /**
         * The end of the main attributes section in the manifest is needed in
         * verification.
         */
        private int mainEnd;

        internal static readonly int LINE_LENGTH_LIMIT = 72;

        private static readonly byte[] LINE_SEPARATOR = new byte[] { (byte)'\r', (byte)'\n' };

        private static readonly byte[] VALUE_SEPARATOR = new byte[] { (byte)':', (byte)' ' };

        private static readonly AttributesNS.Name NAME_ATTRIBUTE = new AttributesNS.Name(
                "Name"); //$NON-NLS-1$

        private Attributes mainAttributes = new Attributes();

        private HashMap<String, Attributes> entries = new HashMap<String, Attributes>();
        private HashMap<String, Chunk> chunks;

        /**
         * Creates a new {@code Manifest} instance. The new instance will have the
         * same attributes as those found in the parameter {@code Manifest}.
         *
         * @param man
         *            {@code Manifest} instance to obtain attributes from.
         */
        public Manifest(Manifest man) {
            mainAttributes = (Attributes) man.mainAttributes.clone();
            entries = (HashMap<String, Attributes>) ((HashMap<String, Attributes>) man
                    .getEntries()).clone();
        }
        protected internal Manifest(java.io.InputStream isJ, bool readChunks) {//throws IOException {
            if (readChunks) {
                chunks = new HashMap<String, Chunk>();
            }
            read(isJ);
        }
        /**
         * Returns a map containing the {@code Attributes} for each entry in the
         * {@code Manifest}.
         *
         * @return the map of entry attributes.
         */
        public Map<String, Attributes> getEntries()
        {
            initEntries();
            return entries;
        }

        private void initEntries()
        {
            if (im == null)
            {
                return;
            }
        }


        /**
         * Creates a copy of this {@code Manifest}. The returned {@code Manifest}
         * will equal the {@code Manifest} from which it was cloned.
         *
         * @return a copy of this instance.
         */
        public Object clone() {
            return new Manifest(this);
        }


        /**
         * Returns the {@code Attributes} associated with the parameter entry
         * {@code name}.
         *
         * @param name
         *            the name of the entry to obtain {@code Attributes} from.
         * @return the Attributes for the entry or {@code null} if the entry does
         *         not exist.
         */
        public Attributes getAttributes(String name)
        {
            return getEntries().get(name);
        }
        /**
         * Constructs a new {@code Manifest} instance obtaining attribute
         * information from the specified input stream.
         *
         * @param is
         *            The {@code InputStream} to read from.
         * @throws IOException
         *             If an error occurs reading the {@code Manifest}.
         */
        public void read(java.io.InputStream isJ) {//throws IOException {
            byte[] buf;
            // Try to read get a reference to the bytes directly
            try {
                buf = org.apache.harmony.luni.util.InputStreamHelper.expose(isJ);
            } catch (java.lang.UnsupportedOperationException uoe) {
                buf = readFully(isJ);
            }

            if (buf.Length == 0) {
                return;
            }

            // a workaround for HARMONY-5662
            // replace EOF and NUL with another new line
            // which does not trigger an error
            byte b = buf[buf.Length - 1];
            if (0 == b || 26 == b) {
                buf[buf.Length - 1] = (byte)'\n';
            }

            // Attributes.Name.MANIFEST_VERSION is not used for
            // the second parameter for RI compatibility
            im = new InitManifest(buf, mainAttributes, null);
            mainEnd = im.getPos();
            // FIXME
            im.initEntries(entries, chunks);
            im = null;
        }

        /*
         * Helper to read the entire contents of the manifest from the
         * given input stream.  Usually we can do this in a single read
         * but we need to account for 'infinite' streams, by ensuring we
         * have a line feed within a reasonable number of characters.
         */
        private byte[] readFully(java.io.InputStream isJ){// throws IOException {
            // Initial read
            byte[] buffer = new byte[4096];
            int count = isJ.read(buffer);
            int nextByte = isJ.read();

            // Did we get it all in one read?
            if (nextByte == -1) {
                byte[] dest = new byte[count];
                java.lang.SystemJ.arraycopy(buffer, 0, dest, 0, count);
                return dest;
            }

            // Does it look like a manifest?
            if (!containsLine(buffer, count)) {
                // archive.2E=Manifest is too long
                throw new java.io.IOException("Manifest is too long"); //$NON-NLS-1$
            }

            // Requires additional reads
            java.io.ByteArrayOutputStream baos = new java.io.ByteArrayOutputStream(count * 2);
            baos.write(buffer, 0, count);
            baos.write(nextByte);
            while (true) {
                count = isJ.read(buffer);
                if (count == -1) {
                    return baos.toByteArray();
                }
                baos.write(buffer, 0, count);
            }
        }
        /*
         * Check to see if the buffer contains a newline or carriage
         * return character within the first 'length' bytes.  Used to
         * check the validity of the manifest input stream.
         */
        private bool containsLine(byte[] buffer, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (buffer[i] == 0x0A || buffer[i] == 0x0D)
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal class Chunk
    {
        protected internal int start;
        protected internal int end;

        protected internal Chunk(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
