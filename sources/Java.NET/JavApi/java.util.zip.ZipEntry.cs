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
     * An instance of {@code ZipEntry} represents an entry within a <i>ZIP-archive</i>.
     * An entry has attributes such as name (= path) or the size of its data. While
     * an entry identifies data stored in an archive, it does not hold the data
     * itself. For example when reading a <i>ZIP-file</i> you will first retrieve
     * all its entries in a collection and then read the data for a specific entry
     * through an input stream.
     *
     * @see ZipFile
     * @see ZipOutputStream
     */
    public class ZipEntry : java.lang.Cloneable {
        protected internal String name, comment;

        protected internal long compressedSize = -1, crc = -1, size = -1;

        protected internal int compressionMethod = -1, time = -1, modDate = -1;

        protected internal byte[] extra;

        protected internal int nameLen = -1;
        protected internal long mLocalHeaderRelOffset = -1;

        /**
         * Zip entry state: Deflated.
         */
        public const int DEFLATED = 8;

        /**
         * Zip entry state: Stored.
         */
        public const int STORED = 0;

        /**
         * Constructs a new {@code ZipEntry} with the specified name.
         *
         * @param name
         *            the name of the ZIP entry.
         * @throws IllegalArgumentException
         *             if the name length is outside the range (> 0xFFFF).
         */
        public ZipEntry(String name) {
            if (name == null) {
                throw new java.lang.NullPointerException();
            }
            if (name.length() > 0xFFFF) {
                throw new java.lang.IllegalArgumentException();
            }
            this.name = name;
        }

        /**
         * Gets the comment for this {@code ZipEntry}.
         *
         * @return the comment for this {@code ZipEntry}, or {@code null} if there
         *         is no comment. If we're reading an archive with
         *         {@code ZipInputStream} the comment is not available.
         */
        public virtual String getComment() {
            return comment;
        }

        /**
         * Gets the compressed size of this {@code ZipEntry}.
         *
         * @return the compressed size, or -1 if the compressed size has not been
         *         set.
         */
        public virtual long getCompressedSize() {
            return compressedSize;
        }

        /**
         * Gets the checksum for this {@code ZipEntry}.
         *
         * @return the checksum, or -1 if the checksum has not been set.
         */
        public virtual long getCrc() {
            return crc;
        }

        /**
         * Gets the extra information for this {@code ZipEntry}.
         *
         * @return a byte array containing the extra information, or {@code null} if
         *         there is none.
         */
        public virtual byte[] getExtra() {
            return extra;
        }

        /**
         * Gets the compression method for this {@code ZipEntry}.
         *
         * @return the compression method, either {@code DEFLATED}, {@code STORED}
         *         or -1 if the compression method has not been set.
         */
        public virtual int getMethod() {
            return compressionMethod;
        }

        /**
         * Gets the name of this {@code ZipEntry}.
         *
         * @return the entry name.
         */
        public virtual String getName() {
            return name;
        }

        /**
         * Gets the uncompressed size of this {@code ZipEntry}.
         *
         * @return the uncompressed size, or {@code -1} if the size has not been
         *         set.
         */
        public virtual long getSize() {
            return size;
        }

        /**
         * Gets the last modification time of this {@code ZipEntry}.
         *
         * @return the last modification time as the number of milliseconds since
         *         Jan. 1, 1970.
         */
        public virtual long getTime() {
            if (time != -1) {
                GregorianCalendar cal = new GregorianCalendar();
                // We don't need to set milliseconds to zero. With new instance of calendar it is!
                //cal.set(Calendar.MILLISECOND, 0);
                cal.set(1980 + ((modDate >> 9) & 0x7f), 
                        ((modDate >> 5) & 0xf) - 1,
                        modDate & 0x1f, 
                        (time >> 11) & 0x1f, 
                        (time >> 5) & 0x3f,
                        (time & 0x1f) << 1);
                return cal.getTime().getTime();
            }
            return -1;
        }

        /**
         * Determine whether or not this {@code ZipEntry} is a directory.
         *
         * @return {@code true} when this {@code ZipEntry} is a directory, {@code
         *         false} otherwise.
         */
        public virtual bool isDirectory() {
            return name.charAt(name.length() - 1) == '/';
        }

        /**
         * Sets the comment for this {@code ZipEntry}.
         *
         * @param string
         *            the comment for this entry.
         */
        public virtual void setComment(String s) {
            if (s == null || s.length() <= 0xFFFF) {
                comment = s;
            } else {
                throw new java.lang.IllegalArgumentException();
            }
        }

        /**
         * Sets the compressed size for this {@code ZipEntry}.
         *
         * @param value
         *            the compressed size (in bytes).
         */
        public virtual void setCompressedSize(long value) {
            compressedSize = value;
        }

        /**
         * Sets the checksum for this {@code ZipEntry}.
         *
         * @param value
         *            the checksum for this entry.
         * @throws IllegalArgumentException
         *             if {@code value} is < 0 or > 0xFFFFFFFFL.
         */
        public virtual void setCrc(long value) {
            if (value >= 0 && value <= 0xFFFFFFFFL) {
                crc = value;
            } else {
                throw new java.lang.IllegalArgumentException();
            }
        }

        /**
         * Sets the extra information for this {@code ZipEntry}.
         *
         * @param data
         *            a byte array containing the extra information.
         * @throws IllegalArgumentException
         *             when the length of data is greater than 0xFFFF bytes.
         */
        public virtual void setExtra(byte[] data) {
            if (data == null || data.Length <= 0xFFFF) {
                extra = data;
            } else {
                throw new java.lang.IllegalArgumentException();
            }
        }

        /**
         * Sets the compression method for this {@code ZipEntry}.
         *
         * @param value
         *            the compression method, either {@code DEFLATED} or {@code
         *            STORED}.
         * @throws IllegalArgumentException
         *             when value is not {@code DEFLATED} or {@code STORED}.
         */
        public virtual void setMethod(int value) {
            if (value != STORED && value != DEFLATED) {
                throw new java.lang.IllegalArgumentException();
            }
            compressionMethod = value;
        }

        /**
         * Sets the uncompressed size of this {@code ZipEntry}.
         *
         * @param value
         *            the uncompressed size for this entry.
         * @throws IllegalArgumentException
         *             if {@code value} < 0 or {@code value} > 0xFFFFFFFFL.
         */
        public virtual void setSize(long value) {
            if (value >= 0 && value <= 0xFFFFFFFFL) {
                size = value;
            } else {
                throw new java.lang.IllegalArgumentException();
            }
        }

        /**
         * Sets the modification time of this {@code ZipEntry}.
         *
         * @param value
         *            the modification time as the number of milliseconds since Jan.
         *            1, 1970.
         */
        public virtual void setTime(long value) {
            GregorianCalendar cal = new GregorianCalendar();
            cal.setTime(new Date(value));
            int year = cal.get(Calendar.YEAR);
            if (year < 1980) {
                modDate = 0x21;
                time = 0;
            } else {
                modDate = cal.get(Calendar.DATE);
                modDate = (cal.get(Calendar.MONTH) + 1 << 5) | modDate;
                modDate = ((cal.get(Calendar.YEAR) - 1980) << 9) | modDate;
                time = cal.get(Calendar.SECOND) >> 1;
                time = (cal.get(Calendar.MINUTE) << 5) | time;
                time = (cal.get(Calendar.HOUR_OF_DAY) << 11) | time;
            }
        }

        /**
         * Returns the string representation of this {@code ZipEntry}.
         *
         * @return the string representation of this {@code ZipEntry}.
         */
        public override String ToString() {
            return name;
        }

        /**
         * Constructs a new {@code ZipEntry} using the values obtained from {@code
         * ze}.
         *
         * @param ze
         *            the {@code ZipEntry} from which to obtain values.
         */
        public ZipEntry(ZipEntry ze) {
            name = ze.name;
            comment = ze.comment;
            time = ze.time;
            size = ze.size;
            compressedSize = ze.compressedSize;
            crc = ze.crc;
            compressionMethod = ze.compressionMethod;
            modDate = ze.modDate;
            extra = ze.extra;
            nameLen = ze.nameLen;
            mLocalHeaderRelOffset = ze.mLocalHeaderRelOffset;
        }

        /**
         * Returns a shallow copy of this entry.
         *
         * @return a copy of this entry.
         */
        public Object clone() {
            return new ZipEntry(this);
        }

        /**
         * Returns the hash code for this {@code ZipEntry}.
         *
         * @return the hash code of the entry.
         */
        
        public override int GetHashCode() {
            return name.GetHashCode();
        }

        /*
         * Internal constructor.  Creates a new ZipEntry by reading the
         * Central Directory Entry from "in", which must be positioned at
         * the CDE signature.
         *
         * On exit, "in" will be positioned at the start of the next entry.
         */
        internal ZipEntry(LittleEndianReader ler, java.io.InputStream inJ) 
            //throws IOException 
        {

            /*
             * We're seeing performance issues when we call readShortLE and
             * readIntLE, so we're going to read the entire header at once
             * and then parse the results out without using any function calls.
             * Uglier, but should be much faster.
             *
             * Note that some lines look a bit different, because the corresponding
             * fields or locals are long and so we need to do & 0xffffffffl to avoid
             * problems induced by sign extension.
             */

            byte[] hdrBuf = ler.hdrBuf;
            myReadFully(inJ, hdrBuf);

            long sig = (hdrBuf[0] & 0xff) | ((hdrBuf[1] & 0xff) << 8) |
                ((hdrBuf[2] & 0xff) << 16) | ((hdrBuf[3] << 24) & 0xffffffffL);
            if (sig != ZipFile.CENSIG) {
                 throw new java.util.zip.ZipException();//Messages.getString("archive.3A"));
            }

            compressionMethod = (hdrBuf[10] & 0xff) | ((hdrBuf[11] & 0xff) << 8);
            time = (hdrBuf[12] & 0xff) | ((hdrBuf[13] & 0xff) << 8);
            modDate = (hdrBuf[14] & 0xff) | ((hdrBuf[15] & 0xff) << 8);
            crc = (hdrBuf[16] & 0xff) | ((hdrBuf[17] & 0xff) << 8)
                    | ((hdrBuf[18] & 0xff) << 16)
                    | ((hdrBuf[19] << 24) & 0xffffffffL);
            compressedSize = (hdrBuf[20] & 0xff) | ((hdrBuf[21] & 0xff) << 8)
                    | ((hdrBuf[22] & 0xff) << 16)
                    | ((hdrBuf[23] << 24) & 0xffffffffL);
            size = (hdrBuf[24] & 0xff) | ((hdrBuf[25] & 0xff) << 8)
                    | ((hdrBuf[26] & 0xff) << 16)
                    | ((hdrBuf[27] << 24) & 0xffffffffL);
            nameLen = (hdrBuf[28] & 0xff) | ((hdrBuf[29] & 0xff) << 8);
            int extraLen = (hdrBuf[30] & 0xff) | ((hdrBuf[31] & 0xff) << 8);
            int commentLen = (hdrBuf[32] & 0xff) | ((hdrBuf[33] & 0xff) << 8);
            mLocalHeaderRelOffset = (hdrBuf[42] & 0xff) | ((hdrBuf[43] & 0xff) << 8)
                    | ((hdrBuf[44] & 0xff) << 16)
                    | ((hdrBuf[45] << 24) & 0xffffffffL);

            byte[] nameBytes = new byte[nameLen];
            myReadFully(inJ, nameBytes);

            byte[] commentBytes = null;
            if (commentLen > 0) {
                commentBytes = new byte[commentLen];
                myReadFully(inJ, commentBytes);
            }

            if (extraLen > 0) {
                extra = new byte[extraLen];
                myReadFully(inJ, extra);
            }

            try {
                /*
                 * The actual character set is "IBM Code Page 437".  As of
                 * Sep 2006, the Zip spec (APPNOTE.TXT) supports UTF-8.  When
                 * bit 11 of the GP flags field is set, the file name and
                 * comment fields are UTF-8.
                 *
                 * TODO: add correct UTF-8 support.
                 */
                name = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(commentBytes);//new String(nameBytes, "ISO-8859-1");
                if (commentBytes != null) {
                    comment = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(commentBytes);// new String(commentBytes, "ISO-8859-1");
                } else {
                    comment = null;
                }
            } catch (java.io.UnsupportedEncodingException uee) {
                throw new java.lang.InternalError(uee.getMessage());
            }
        }

        private void myReadFully(java.io.InputStream inJ, byte[] b) //throws IOException 
        {
            int len = b.Length;
            int off = 0;

            while (len > 0) {
                int count = inJ.read(b, off, len);
                if (count <= 0) {
                    throw new java.io.EOFException();
                }
                off += count;
                len -= count;
            }
        }

        /*
         * Read a four-byte int in little-endian order.
         */
        internal static long readIntLE(java.io.RandomAccessFile raf) //throws IOException 
        {
            int b0 = raf.read();
            int b1 = raf.read();
            int b2 = raf.read();
            int b3 = raf.read();

            if (b3 < 0) {
                throw new java.io.EOFException();//Messages.getString("archive.3B"));
            }
            return b0 | (b1 << 8) | (b2 << 16) | (b3 << 24); // ATTENTION: DOES SIGN EXTENSION: IS THIS WANTED?
        }

    }
    internal class LittleEndianReader {
        private byte[] b = new byte[4];
        protected internal byte[] hdrBuf = new byte[ZipFile.CENHDR];

        /*
            * Read a two-byte short in little-endian order.
            */
        internal int readShortLE(java.io.InputStream inJ) //throws IOException 
        {
            if (inJ.read(b, 0, 2) == 2) {
                return (b[0] & 0XFF) | ((b[1] & 0XFF) << 8);
            } else {
                throw new java.io.EOFException();//Messages.getString("archive.3C"));
            }
        }

        /*
            * Read a four-byte int in little-endian order.
            */
        internal long readIntLE(java.io.InputStream inJ) //throws IOException 
        {
            if (inJ.read(b, 0, 4) == 4) {
                return (   ((b[0] & 0XFF))
                            | ((b[1] & 0XFF) << 8)
                            | ((b[2] & 0XFF) << 16)
                            | ((b[3] & 0XFF) << 24))
                        & 0XFFFFFFFFL; // Here for sure NO sign extension is wanted.
            } else {
                throw new java.io.EOFException();//Messages.getString("archive.3D"));
            }
        }
    }
}
