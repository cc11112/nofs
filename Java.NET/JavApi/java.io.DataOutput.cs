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
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
	
	/**
	 * Defines an interface for classes that are able to write typed data to some
	 * target. Typically, this data can be read in by a class which implements
	 * DataInput. Types that can be written include byte, 16-bit short, 32-bit int,
	 * 32-bit float, 64-bit long, 64-bit double, byte strings, and {@link DataInput
	 * MUTF-8} encoded strings.
	 * 
	 * @see DataOutputStream
	 * @see RandomAccessFile
	 */
	public interface DataOutput {
	
	    /**
	     * Writes the entire contents of the byte array {@code buffer} to this
	     * stream.
	     * 
	     * @param buffer
	     *            the buffer to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readFully(byte[])
	     * @see DataInput#readFully(byte[], int, int)
	     */
	    void write(byte[] buffer);
	
	    /**
	     * Writes {@code count} bytes from the byte array {@code buffer} starting at
	     * offset {@code index}.
	     * 
	     * @param buffer
	     *            the buffer to write.
	     * @param offset
	     *            the index of the first byte in {@code buffer} to write.
	     * @param count
	     *            the number of bytes from the {@code buffer} to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readFully(byte[])
	     * @see DataInput#readFully(byte[], int, int)
	     */
	    void write(byte[] buffer, int offset, int count);
	
	    /**
	     * Writes the specified 8-bit byte.
	     * 
	     * @param oneByte
	     *            the byte to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readByte()
	     */
	    void write(int oneByte);
	
	    /**
	     * Writes the specified boolean.
	     * 
	     * @param val
	     *            the boolean value to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readBoolean()
	     */
	    void writeBoolean(bool val);
	
	    /**
	     * Writes the specified 8-bit byte.
	     * 
	     * @param val
	     *            the byte value to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readByte()
	     * @see DataInput#readUnsignedByte()
	     */
	    void writeByte(int val);
	
	    /**
	     * Writes the low order 8-bit bytes from the specified string.
	     * 
	     * @param str
	     *            the string containing the bytes to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readFully(byte[])
	     * @see DataInput#readFully(byte[],int,int)
	     */
	    void writeBytes(String str);
	
	    /**
	     * Writes the specified 16-bit character. Only the two least significant
	     * bytes of the integer {@code oneByte} are written, with the higher one
	     * written first. This represents the Unicode value of the char.
	     * 
	     * @param val
	     *            the character to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readChar()
	     */
	    void writeChar(int val);
	
	    /**
	     * Writes the 16-bit characters contained in {@code str}.
	     * 
	     * @param str
	     *            the string that contains the characters to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readChar()
	     */
	    void writeChars(String str);
	
	    /**
	     * Writes the specified 64-bit double. The resulting output is the eight
	     * bytes returned by {@link Double#doubleToLongBits(double)}.
	     * 
	     * @param val
	     *            the double to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readDouble()
	     */
	    void writeDouble(double val);
	
	    /**
	     * Writes the specified 32-bit float. The resulting output is the four bytes
	     * returned by {@link Float#floatToIntBits(float)}.
	     * 
	     * @param val
	     *            the float to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readFloat()
	     */
	    void writeFloat(float val);
	
	    /**
	     * Writes the specified 32-bit int. The resulting output is the four bytes,
	     * highest order first, of {@code val}.
	     * 
	     * @param val
	     *            the int to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readInt()
	     */
	    void writeInt(int val);
	
	    /**
	     * Writes the specified 64-bit long. The resulting output is the eight
	     * bytes, highest order first, of {@code val}.
	     * 
	     * @param val
	     *            the long to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readLong()
	     */
	    void writeLong(long val);
	
	    /**
	     * Writes the specified 16-bit short. Only the lower two bytes of {@code
	     * val} are written with the higher one written first.
	     * 
	     * @param val
	     *            the short to write.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readShort()
	     * @see DataInput#readUnsignedShort()
	     */
	    void writeShort(int val);
	
	    /**
	     * Writes the specified string encoded in {@link DataInput modified UTF-8}.
	     * 
	     * @param str
	     *            the string to write encoded in {@link DataInput modified UTF-8}.
	     * @throws IOException
	     *             if an I/O error occurs while writing.
	     * @see DataInput#readUTF()
	     */
	    void writeUTF(String str);
	}
}
