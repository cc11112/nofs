using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util.zip
{
    ///<summary>
    /// <para>
    /// This class uncompresses data that was compressed using the <i>DEFLATE</i>
    /// algorithm (see <a href="http://www.gzip.org/algorithm.txt">specification</a>).
    /// </para>
    /// <para>
    /// Basically this class is part of the API to the stream based ZLIB compression
    /// library and is used as such by {@code InflaterInputStream} and its
    /// descendants.
    /// <para/>
    /// <para>
    /// The typical usage of a {@code Inflater} outside this package consists of a
    /// specific call to one of its constructors before being passed to an instance
    /// of {@code InflaterInputStream}.
    /// <para>
    /// @see InflaterInputStream
    /// @see Deflater
    ///</para>
    ///</para>
    ///</para>
    ///</summary>
    public class Inflater {

        private long streamHandle = -1;

        /**
         * This constructor creates an inflater that expects a header from the input
         * stream. Use {@code Inflater(bool)} if the input comes without a ZLIB
         * header.
         */
        public Inflater() :this(false) {
        }

        /**
         * This constructor allows to create an inflater that expects no header from
         * the input stream.
         *
         * @param noHeader
         *            {@code true} indicates that no ZLIB header comes with the
         *            input.
         */
        public Inflater(bool noHeader) {
            //streamHandle = createStream(noHeader);
        }

#region j
        private bool needsDictionaryJ = false;
        private bool finish = false;
        int inLength;

        int inRead;


        /**
         * Release any resources associated with this {@code Inflater}. Any unused
         * input/output is discarded. This is also called by the finalize method.
         */
        public virtual void end() {
            lock (this) {
            
            }
        }

        ~Inflater () {
            this.finalize();
        }
        
        protected void finalize() {
            end();
        }

        /**
         * Indicates if the {@code Inflater} has inflated the entire deflated
         * stream. If deflated bytes remain and {@code needsInput()} returns {@code
         * true} this method will return {@code false}. This method should be
         * called after all deflated input is supplied to the {@code Inflater}.
         *
         * @return {@code true} if all input has been inflated, {@code false}
         *         otherwise.
         */
        public bool finished() {
            return finish;
        }

        /**
         * Returns the <i>Adler32</i> checksum of either all bytes inflated, or the
         * checksum of the preset dictionary if one has been supplied.
         *
         * @return The <i>Adler32</i> checksum associated with this
         *         {@code Inflater}.
         */
        public virtual int getAdler() {
            lock (this) {
                //TODO
                return -1;
            }
        }

        /**
         * Returns the total number of bytes read by the {@code Inflater}. This
         * method performs the same as {@code getTotalIn()} except that it returns a
         * {@code long} value instead of an integer.
         *
         * @return the total number of bytes read.
         */
        public virtual long getBytesRead() {
            lock (this)
            {
                // Throw NPE here
                if (streamHandle == -1)
                {
                    throw new java.lang.NullPointerException();
                }
                return -1;//getTotalInImpl(streamHandle);
            }
        }

        /**
         * Returns a the total number of bytes read by the {@code Inflater}. This
         * method performs the same as {@code getTotalOut} except it returns a
         * {@code long} value instead of an integer.
         *
         * @return the total bytes written to the output buffer.
         */
        public virtual long getBytesWritten() {
            lock (this) {
                // Throw NPE here
                if (streamHandle == -1) {
                    throw new java.lang.NullPointerException();
                }
                return -1;// getTotalOutImpl(streamHandle);
            }
        }

        /**
         * Returns the number of bytes of current input remaining to be read by the
         * inflater.
         *
         * @return the number of bytes of unread input.
         */
        public virtual int getRemaining() {
            lock (this) {
                return inLength - inRead;
            }
        }

        /**
         * Returns total number of bytes of input read by the {@code Inflater}. The
         * result value is limited by {@code Integer.MAX_VALUE}.
         *
         * @return the total number of bytes read.
         */
        public virtual int getTotalIn() {
            lock (this) {
                return -1;
            }
        }

        /**
         * Returns total number of bytes written to the output buffer by the {@code
         * Inflater}. The result value is limited by {@code Integer.MAX_VALUE}.
         *
         * @return the total bytes of output data written.
         */
        public virtual int getTotalOut() {
            lock(this) {
                return -1;
            }
        }

        /**
         * Inflates bytes from current input and stores them in {@code buf}.
         *
         * @param buf
         *            the buffer where decompressed data bytes are written.
         * @return the number of bytes inflated.
         * @throws DataFormatException
         *             if the underlying stream is corrupted or was not compressed
         *             using a {@code Deflater}.
         */
        public int inflate(byte[] buf) {//throws DataFormatException {
            return inflate(buf, 0, buf.Length);
        }

        /**
         * Inflates up to n bytes from the current input and stores them in {@code
         * buf} starting at {@code off}.
         *
         * @param buf
         *            the buffer to write inflated bytes to.
         * @param off
         *            the offset in buffer where to start writing decompressed data.
         * @param nbytes
         *            the number of inflated bytes to write to {@code buf}.
         * @throws DataFormatException
         *             if the underlying stream is corrupted or was not compressed
         *             using a {@code Deflater}.
         * @return the number of bytes inflated.
         */
        public virtual int inflate(byte[] buf, int off, int nbytes){// throws DataFormatException {
            lock (this) {
                return -1;
            }
        }


        /**
         * Indicates whether the input bytes were compressed with a preset
         * dictionary. This method should be called prior to {@code inflate()} to
         * determine whether a dictionary is required. If so {@code setDictionary()}
         * should be called with the appropriate dictionary prior to calling {@code
         * inflate()}.
         *
         * @return {@code true} if a preset dictionary is required for inflation.
         * @see #setDictionary(byte[])
         * @see #setDictionary(byte[], int, int)
         */
        public virtual bool needsDictionary() {
            lock (this) {
                return needsDictionaryJ;
            }
        }

        /**
         * Indicates that input has to be passed to the inflater.
         *
         * @return {@code true} if {@code setInput} has to be called before
         *         inflation can proceed.
         * @see #setInput(byte[])
         */
        public virtual bool needsInput() {
            lock (this) {
                return inRead == inLength;
            }
        }

        /**
         * Resets the {@code Inflater}. Should be called prior to inflating a new
         * set of data.
         */
        public virtual void reset() {
            lock (this) {
            
            }
        }

        /**
         * Sets the preset dictionary to be used for inflation to {@code buf}.
         * {@code needsDictionary()} can be called to determine whether the current
         * input was deflated using a preset dictionary.
         *
         * @param buf
         *            The buffer containing the dictionary bytes.
         * @see #needsDictionary
         */
        public virtual void setDictionary(byte[] buf) {
            lock (this) {
                setDictionary(buf, 0, buf.Length);
            }
        }

        /**
         * Like {@code setDictionary(byte[])}, allowing to define a specific region
         * inside {@code buf} to be used as a dictionary.
         * <p>
         * The dictionary should be set if the {@link #inflate(byte[])} returned
         * zero bytes inflated and {@link #needsDictionary()} returns
         * <code>true</code>.
         *
         * @param buf
         *            the buffer containing the dictionary data bytes.
         * @param off
         *            the offset of the data.
         * @param nbytes
         *            the length of the data.
         * @see #needsDictionary
         */
        public virtual void setDictionary(byte[] buf, int off, int nbytes) {
            lock (this) {
            
            }
        }


        /**
         * Sets the current input to be decompressed. This method should only be
         * called if {@code needsInput()} returns {@code true}.
         *
         * @param buf
         *            the input buffer.
         * @see #needsInput
         */
        public virtual void setInput(byte[] buf) { 
            lock (this) {
                setInput(buf, 0, buf.Length);
            }
        }

        /**
         * Sets the current input to the region of the input buffer starting at
         * {@code off} and ending at {@code nbytes - 1} where data is written after
         * decompression. This method should only be called if {@code needsInput()}
         * returns {@code true}.
         *
         * @param buf
         *            the input buffer.
         * @param off
         *            the offset to read from the input buffer.
         * @param nbytes
         *            the number of bytes to read.
         * @see #needsInput
         */
        public virtual void setInput(byte[] buf, int off, int nbytes) {
            lock (this) {
            
            }
        }

#endregion
    }
}
