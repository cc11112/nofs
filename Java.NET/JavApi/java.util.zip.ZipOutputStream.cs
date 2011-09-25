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

namespace biz.ritter.javapi.util.zip
{

    /**
     * This class provides an implementation of {@code FilterOutputStream} that
     * compresses data entries into a <i>ZIP-archive</i> output stream.
     * <p>
     * {@code ZipOutputStream} is used to write {@code ZipEntries} to the underlying
     * stream. Output from {@code ZipOutputStream} conforms to the {@code ZipFile}
     * file format.
     * <p>
     * While {@code DeflaterOutputStream} can write a compressed <i>ZIP-archive</i>
     * entry, this extension can write uncompressed entries as well. In this case
     * special rules apply, for this purpose refer to the <a
     * href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">file format
     * specification</a>.
     *
     * @see ZipEntry
     * @see ZipFile
     */

    public class ZipOutputStream : DeflaterOutputStream{//, ZipConstants {

        /**
         * Indicates deflated entries.
         */
        public static readonly int DEFLATED = 8;

        /**
         * Indicates uncompressed entries.
         */
        public static readonly int STORED = 0;

        protected internal static readonly int ZIPDataDescriptorFlag = 8;

        protected internal static readonly int ZIPLocalHeaderVersionNeeded = 20;

        private String comment;

        private java.util.Vector<String> entries = new java.util.Vector<String>();

        private int compressMethod = DEFLATED;

        private int compressLevel = Deflater.DEFAULT_COMPRESSION;

        private java.io.ByteArrayOutputStream cDir = new java.io.ByteArrayOutputStream();

        private ZipEntry currentEntry;

        private readonly CRC32 crc = new CRC32();

        private int offset = 0, curOffset = 0, nameLength;

        private byte[] nameBytes;

        /**
         * Constructs a new {@code ZipOutputStream} with the specified output
         * stream.
         *
         * @param p1
         *            the {@code OutputStream} to write the data to.
         */
        public ZipOutputStream(java.io.OutputStream p1) :base(p1, new Deflater(Deflater.DEFAULT_COMPRESSION, true)){
        }

        /**
         * Closes the current {@code ZipEntry}, if any, and the underlying output
         * stream. If the stream is already closed this method does nothing.
         *
         * @throws IOException
         *             If an error occurs closing the stream.
         */
        
        public override void close() {//throws IOException {
            if (outJ != null) {
                finish();
                outJ.close();
                outJ = null;
            }
        }

        /**
         * Closes the current {@code ZipEntry}. Any entry terminal data is written
         * to the underlying stream.
         *
         * @throws IOException
         *             If an error occurs closing the entry.
         */
        public void closeEntry(){// throws IOException {
            if (cDir == null) {
                throw new java.io.IOException("Stream is closed"); //$NON-NLS-1$
            }
            if (currentEntry == null) {
                return;
            }
            if (currentEntry.getMethod() == DEFLATED) {
                base.finish();
            }

            // Verify values for STORED types
            if (currentEntry.getMethod() == STORED) {
                if (crc.getValue() != currentEntry.crc) {
                    throw new ZipException("Crc mismatch"); //$NON-NLS-1$
                }
                if (currentEntry.size != crc.tbytes) {
                    throw new ZipException("Size mismatch"); //$NON-NLS-1$
                }
            }
            curOffset = ZipFile.LOCHDR;

            // Write the DataDescriptor
            if (currentEntry.getMethod() != STORED) {
                curOffset += ZipFile.EXTHDR;
                writeLong(outJ, ZipFile.EXTSIG);
                writeLong(outJ, currentEntry.crc = crc.getValue());
                writeLong(outJ, currentEntry.compressedSize = def.getTotalOut());
                writeLong(outJ, currentEntry.size = def.getTotalIn());
            }
            // Update the CentralDirectory
            writeLong(cDir, ZipFile.CENSIG);
            writeShort(cDir, ZIPLocalHeaderVersionNeeded); // Version created
            writeShort(cDir, ZIPLocalHeaderVersionNeeded); // Version to extract
            writeShort(cDir, currentEntry.getMethod() == STORED ? 0
                    : ZIPDataDescriptorFlag);
            writeShort(cDir, currentEntry.getMethod());
            writeShort(cDir, currentEntry.time);
            writeShort(cDir, currentEntry.modDate);
            writeLong(cDir, crc.getValue());
            if (currentEntry.getMethod() == DEFLATED) {
                curOffset += (int) writeLong(cDir, def.getTotalOut());
                writeLong(cDir, def.getTotalIn());
            } else {
                curOffset += (int) writeLong(cDir, crc.tbytes);
                writeLong(cDir, crc.tbytes);
            }
            curOffset += writeShort(cDir, nameLength);
            if (currentEntry.extra != null) {
                curOffset += (int) writeShort(cDir, currentEntry.extra.Length);
            } else {
                writeShort(cDir, 0);
            }
            String c;
            if ((c = currentEntry.getComment()) != null) {
                writeShort(cDir, c.length());
            } else {
                writeShort(cDir, 0);
            }
            writeShort(cDir, 0); // Disk Start
            writeShort(cDir, 0); // Internal File Attributes
            writeLong(cDir, 0); // External File Attributes
            writeLong(cDir, offset);
            cDir.write(nameBytes);
            nameBytes = null;
            if (currentEntry.extra != null) {
                cDir.write(currentEntry.extra);
            }
            offset += curOffset;
            if (c != null) {
                cDir.write(c.getBytes());
            }
            currentEntry = null;
            crc.reset();
            def.reset();
            done = false;
        }

        /**
         * Indicates that all entries have been written to the stream. Any terminal
         * information is written to the underlying stream.
         *
         * @throws IOException
         *             if an error occurs while terminating the stream.
         */
        
        public override void finish() {//throws IOException {
            if (outJ == null) {
                throw new java.io.IOException("Stream is closed"); //$NON-NLS-1$
            }
            if (cDir == null) {
                return;
            }
            if (entries.size() == 0) {
                throw new ZipException("No entries"); //$NON-NLS-1$;
            }
            if (currentEntry != null) {
                closeEntry();
            }
            int cdirSize = cDir.size();
            // Write Central Dir End
            writeLong(cDir, ZipFile.ENDSIG);
            writeShort(cDir, 0); // Disk Number
            writeShort(cDir, 0); // Start Disk
            writeShort(cDir, entries.size()); // Number of entries
            writeShort(cDir, entries.size()); // Number of entries
            writeLong(cDir, cdirSize); // Size of central dir
            writeLong(cDir, offset); // Offset of central dir
            if (comment != null) {
                writeShort(cDir, comment.length());
                cDir.write(comment.getBytes());
            } else {
                writeShort(cDir, 0);
            }
            // Write the central dir
            outJ.write(cDir.toByteArray());
            cDir = null;

        }

        /**
         * Writes entry information to the underlying stream. Data associated with
         * the entry can then be written using {@code write()}. After data is
         * written {@code closeEntry()} must be called to complete the writing of
         * the entry to the underlying stream.
         *
         * @param ze
         *            the {@code ZipEntry} to store.
         * @throws IOException
         *             If an error occurs storing the entry.
         * @see #write
         */
        public void putNextEntry(ZipEntry ze) {//throws java.io.IOException {
            if (currentEntry != null) {
                closeEntry();
            }
            if (ze.getMethod() == STORED
                    || (compressMethod == STORED && ze.getMethod() == -1)) {
                if (ze.crc == -1) {
                    /* [MSG "archive.20", "Crc mismatch"] */
                    throw new ZipException("Crc mismatch"); //$NON-NLS-1$
                }
                if (ze.size == -1 && ze.compressedSize == -1) {
                    /* [MSG "archive.21", "Size mismatch"] */
                    throw new ZipException("Size mismatch"); //$NON-NLS-1$
                }
                if (ze.size != ze.compressedSize && ze.compressedSize != -1
                        && ze.size != -1) {
                    /* [MSG "archive.21", "Size mismatch"] */
                    throw new ZipException("Size mismatch"); //$NON-NLS-1$
                }
            }
            /* [MSG "archive.1E", "Stream is closed"] */
            if (cDir == null) {
                throw new java.io.IOException("Stream is closed"); //$NON-NLS-1$
            }
            if (entries.contains(ze.name)) {
                /* [MSG "archive.29", "Entry already exists: {0}"] */
                throw new ZipException("Entry already exists: "+ ze.name); //$NON-NLS-1$
            }
            nameLength = utf8Count(ze.name);
            if (nameLength > 0xffff) {
                /* [MSG "archive.2A", "Name too long: {0}"] */
                throw new java.lang.IllegalArgumentException("Name too long: "+ ze.name); //$NON-NLS-1$
            }

            def.setLevel(compressLevel);
            currentEntry = ze;
            entries.add(currentEntry.name);
            if (currentEntry.getMethod() == -1) {
                currentEntry.setMethod(compressMethod);
            }
            writeLong(outJ, ZipFile.LOCSIG); // Entry header
            writeShort(outJ, ZIPLocalHeaderVersionNeeded); // Extraction version
            writeShort(outJ, currentEntry.getMethod() == STORED ? 0
                    : ZIPDataDescriptorFlag);
            writeShort(outJ, currentEntry.getMethod());
            if (currentEntry.getTime() == -1) {
                currentEntry.setTime(java.lang.SystemJ.currentTimeMillis());
            }
            writeShort(outJ, currentEntry.time);
            writeShort(outJ, currentEntry.modDate);

            if (currentEntry.getMethod() == STORED) {
                if (currentEntry.size == -1) {
                    currentEntry.size = currentEntry.compressedSize;
                } else if (currentEntry.compressedSize == -1) {
                    currentEntry.compressedSize = currentEntry.size;
                }
                writeLong(outJ, currentEntry.crc);
                writeLong(outJ, currentEntry.size);
                writeLong(outJ, currentEntry.size);
            } else {
                writeLong(outJ, 0);
                writeLong(outJ, 0);
                writeLong(outJ, 0);
            }
            writeShort(outJ, nameLength);
            if (currentEntry.extra != null) {
                writeShort(outJ, currentEntry.extra.Length);
            } else {
                writeShort(outJ, 0);
            }
            nameBytes = toUTF8Bytes(currentEntry.name, nameLength);
            outJ.write(nameBytes);
            if (currentEntry.extra != null) {
                outJ.write(currentEntry.extra);
            }
        }

        /**
         * Sets the {@code ZipFile} comment associated with the file being written.
         *
         * @param comment
         *            the comment associated with the file.
         */
        public void setComment(String comment) {
            if (comment.length() > 0xFFFF) {
                throw new java.lang.IllegalArgumentException("String is too long"); //$NON-NLS-1$
            }
            this.comment = comment;
        }

        /**
         * Sets the compression level to be used for writing entry data. This level
         * may be set on a per entry basis. The level must have a value between -1
         * and 8 according to the {@code Deflater} compression level bounds.
         *
         * @param level
         *            the compression level (ranging from -1 to 8).
         * @see Deflater
         */
        public void setLevel(int level) {
            if (level < Deflater.DEFAULT_COMPRESSION
                    || level > Deflater.BEST_COMPRESSION) {
                throw new java.lang.IllegalArgumentException();
            }
            compressLevel = level;
        }

        /**
         * Sets the compression method to be used when compressing entry data.
         * method must be one of {@code STORED} (for no compression) or {@code
         * DEFLATED}.
         *
         * @param method
         *            the compression method to use.
         */
        public void setMethod(int method) {
            if (method != STORED && method != DEFLATED) {
                throw new java.lang.IllegalArgumentException();
            }
            compressMethod = method;

        }

        private long writeLong(java.io.OutputStream os, long i) {//throws java.io.IOException {
            // Write out the long value as an unsigned int
            os.write((int) (i & 0xFF));
            os.write((int) (i >> 8) & 0xFF);
            os.write((int) (i >> 16) & 0xFF);
            os.write((int) (i >> 24) & 0xFF);
            return i;
        }

        private int writeShort(java.io.OutputStream os, int i) {//throws java.io.IOException {
            os.write(i & 0xFF);
            os.write((i >> 8) & 0xFF);
            return i;

        }

        /**
         * Writes data for the current entry to the underlying stream.
         *
         * @exception IOException
         *                If an error occurs writing to the stream
         */
        public override void write(byte[] buffer, int off, int nbytes)
        {//throws java.io.IOException {
            // avoid int overflow, check null buf
            if ((off < 0 || (nbytes < 0) || off > buffer.Length)
                    || (buffer.Length - off < nbytes)) {
                throw new java.lang.IndexOutOfBoundsException();
            }

            if (currentEntry == null) {
                /* [MSG "archive.2C", "No active entry"] */
                throw new ZipException("No active entry"); //$NON-NLS-1$
            }

            if (currentEntry.getMethod() == STORED) {
                outJ.write(buffer, off, nbytes);
            } else {
                base.write(buffer, off, nbytes);
            }
            crc.update(buffer, off, nbytes);
        }

        static int utf8Count(String value) {
            int total = 0;
            for (int i = value.length(); --i >= 0;) {
                char ch = value.charAt(i);
                if (ch < 0x80) {
                    total++;
                } else if (ch < 0x800) {
                    total += 2;
                } else {
                    total += 3;
                }
            }
            return total;
        }

        static byte[] toUTF8Bytes(String value, int length) {
            byte[] result = new byte[length];
            int pos = result.Length;
            for (int i = value.length(); --i >= 0;) {
                char ch = value.charAt(i);
                if (ch < 0x80) {
                    result[--pos] = (byte) ch;
                } else if (ch < 0x800) {
                    result[--pos] = (byte) (0x80 | (ch & 0x3f));
                    result[--pos] = (byte) (0xc0 | (ch >> 6));
                } else {
                    result[--pos] = (byte) (0x80 | (ch & 0x3f));
                    result[--pos] = (byte) (0x80 | ((ch >> 6) & 0x3f));
                    result[--pos] = (byte) (0xe0 | (ch >> 12));
                }
            }
            return result;
        }
    }
}
