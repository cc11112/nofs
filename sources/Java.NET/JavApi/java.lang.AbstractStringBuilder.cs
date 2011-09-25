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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{

    /**
     * A modifiable {@link CharSequence sequence of characters} for use in creating
     * and modifying Strings. This class is intended as a base class for
     * {@link StringBuffer} and {@link StringBuilder}.
     *
     * @see StringBuffer
     * @see StringBuilder
     * @since 1.5
     */
    public abstract class AbstractStringBuilder {

        protected internal static readonly int INITIAL_CAPACITY = 16;

        private char[] value;

        private int count;

        private bool shared;

        /*
         * Returns the character array.
         */
        protected internal char[] getValue() {
            return value;
        }

        /*
         * Returns the underlying buffer and sets the shared flag.
         */
        protected internal char[] shareValue() {
            shared = true;
            return value;
        }

        /*
         * Restores internal state after deserialization.
         */
        protected internal void set(char[] val, int len) {//throws InvalidObjectException {
            if (val == null) {
                val = new char[0];
            }
            if (val.Length < len) {
                throw new java.io.InvalidObjectException("Count out of range"); //$NON-NLS-1$
            }

            shared = false;
            value = val;
            count = len;
        }

        protected internal AbstractStringBuilder() {
            value = new char[INITIAL_CAPACITY];
        }

        protected internal AbstractStringBuilder(int capacity) {
            if (capacity < 0) {
                throw new NegativeArraySizeException();
            }
            value = new char[capacity];
        }

        protected internal AbstractStringBuilder(StringJ s) {
            count = s.length();
            shared = false;
            value = new char[count + INITIAL_CAPACITY];
            s.getChars(0, count, value, 0);
        }
        /// <summary>
        /// Create new AbstractStringBuilder
        /// </summary>
        /// <param name="cs"></param>
        protected internal AbstractStringBuilder(String s) : this (s.getWrapperInstance())
        {
        }

        private void enlargeBuffer(int min) {
            int newSize = ((value.Length >> 1) + value.Length) + 2;
            char[] newData = new char[min > newSize ? min : newSize];
            java.lang.SystemJ.arraycopy(value, 0, newData, 0, count);
            value = newData;
            shared = false;
        }

        protected internal void appendNull() {
            int newSize = count + 4;
            if (newSize > value.Length) {
                enlargeBuffer(newSize);
            }
            value[count++] = 'n';
            value[count++] = 'u';
            value[count++] = 'l';
            value[count++] = 'l';
        }

        protected internal void append0(char[] chars) {
            int newSize = count + chars.Length;
            if (newSize > value.Length) {
                enlargeBuffer(newSize);
            }
            java.lang.SystemJ.arraycopy(chars, 0, value, count, chars.Length);
            count = newSize;
        }

        protected internal void append0(char[] chars, int offset, int length) {
            // Force null check of chars first!
            if (offset > chars.Length || offset < 0) {
                // luni.12=Offset out of bounds \: {0}
                throw new ArrayIndexOutOfBoundsException("Offset out of bounds : "+ offset); //$NON-NLS-1$
            }
            if (length < 0 || chars.Length - offset < length) {
                // luni.18=Length out of bounds \: {0}
                throw new ArrayIndexOutOfBoundsException("Length out of bounds : "+ length); //$NON-NLS-1$
            }

            int newSize = count + length;
            if (newSize > value.Length) {
                enlargeBuffer(newSize);
            }
            java.lang.SystemJ.arraycopy(chars, offset, value, count, length);
            count = newSize;
        }

        protected internal void append0(char ch) {
            if (count == value.Length) {
                enlargeBuffer(count + 1);
            }
            value[count++] = ch;
        }

        protected internal void append0(String s) {
            if (s == null) {
                appendNull();
                return;
            }
            int adding = s.length();
            int newSize = count + adding;
            if (newSize > value.Length) {
                enlargeBuffer(newSize);
            }
            s.getChars(0, adding, value, count);
            count = newSize;
        }

        protected internal void append0(CharSequence s, int start, int end) {
            if (s == null) {
                s = new StringJ("null"); //$NON-NLS-1$
            }
            if (start < 0 || end < 0 || start > end || end > s.length()) {
                throw new IndexOutOfBoundsException();
            }

            append0(s.subSequence(start, end).toString());
        }

        /**
         * Returns the number of characters that can be held without growing.
         * 
         * @return the capacity
         * @see #ensureCapacity
         * @see #length
         */
        public virtual int capacity() {
            return value.Length;
        }

        /**
         * Retrieves the character at the {@code index}.
         * 
         * @param index
         *            the index of the character to retrieve.
         * @return the char value.
         * @throws IndexOutOfBoundsException
         *             if {@code index} is negative or greater than or equal to the
         *             current {@link #length()}.
         */
        public virtual char charAt(int index) {
            if (index < 0 || index >= count) {
                throw new StringIndexOutOfBoundsException(index);
            }
            return value[index];
        }

        protected internal void delete0(int start, int end) {
            if (start >= 0) {
                if (end > count) {
                    end = count;
                }
                if (end == start) {
                    return;
                }
                if (end > start) {
                    int length = count - end;
                    if (length >= 0) {
                        if (!shared) {
                            java.lang.SystemJ.arraycopy(value, end, value, start, length);
                        } else {
                            char[] newData = new char[value.Length];
                            java.lang.SystemJ.arraycopy(value, 0, newData, 0, start);
                            java.lang.SystemJ.arraycopy(value, end, newData, start, length);
                            value = newData;
                            shared = false;
                        }
                    }
                    count -= end - start;
                    return;
                }
            }
            throw new StringIndexOutOfBoundsException();
        }

        protected internal void deleteCharAt0(int location) {
            if (0 > location || location >= count) {
                throw new StringIndexOutOfBoundsException(location);
            }
            int length = count - location - 1;
            if (length > 0) {
                if (!shared) {
                    java.lang.SystemJ.arraycopy(value, location + 1, value, location, length);
                } else {
                    char[] newData = new char[value.Length];
                    java.lang.SystemJ.arraycopy(value, 0, newData, 0, location);
                    java.lang.SystemJ.arraycopy(value, location + 1, newData, location,
                                    length);
                    value = newData;
                    shared = false;
                }
            }
            count--;
        }

        /**
         * Ensures that this object has a minimum capacity available before
         * requiring the internal buffer to be enlarged. The general policy of this
         * method is that if the {@code minimumCapacity} is larger than the current
         * {@link #capacity()}, then the capacity will be increased to the largest
         * value of either the {@code minimumCapacity} or the current capacity
         * multiplied by two plus two. Although this is the general policy, there is
         * no guarantee that the capacity will change.
         * 
         * @param min
         *            the new minimum capacity to set.
         */
        public virtual void ensureCapacity(int min) {
            if (min > value.Length) {
                int twice = (value.Length << 1) + 2;
                enlargeBuffer(twice > min ? twice : min);
            }
        }

        /**
         * Copies the requested sequence of characters to the {@code char[]} passed
         * starting at {@code destStart}.
         *
         * @param start
         *            the inclusive start index of the characters to copy.
         * @param end
         *            the exclusive end index of the characters to copy.
         * @param dest
         *            the {@code char[]} to copy the characters to.
         * @param destStart
         *            the inclusive start index of {@code dest} to begin copying to.
         * @throws IndexOutOfBoundsException
         *             if the {@code start} is negative, the {@code destStart} is
         *             negative, the {@code start} is greater than {@code end}, the
         *             {@code end} is greater than the current {@link #length()} or
         *             {@code destStart + end - begin} is greater than
         *             {@code dest.length}.
         */
        public virtual void getChars(int start, int end, char[] dest, int destStart) {
            if (start > count || end > count || start > end) {
                throw new StringIndexOutOfBoundsException();
            }
            java.lang.SystemJ.arraycopy(value, start, dest, destStart, end - start);
        }

        protected internal void insert0(int index, char[] chars) {
            if (0 > index || index > count) {
                throw new StringIndexOutOfBoundsException(index);
            }
            if (chars.Length != 0) {
                move(chars.Length, index);
                java.lang.SystemJ.arraycopy(chars, 0, value, index, chars.Length);
                count += chars.Length;
            }
        }

        protected internal void insert0(int index, char[] chars, int start, int length) {
            if (0 <= index && index <= count) {
                // start + length could overflow, start/length maybe MaxInt
                if (start >= 0 && 0 <= length && length <= chars.Length - start) {
                    if (length != 0) {
                        move(length, index);
                        java.lang.SystemJ.arraycopy(chars, start, value, index, length);
                        count += length;
                    }
                    return;
                }
                throw new StringIndexOutOfBoundsException("offset " + start //$NON-NLS-1$
                        + ", length " + length //$NON-NLS-1$
                        + ", char[].length " + chars.Length); //$NON-NLS-1$
            }
            throw new StringIndexOutOfBoundsException(index);
        }

        protected internal void insert0(int index, char ch) {
            if (0 > index || index > count) {
                // RI compatible exception type
                throw new ArrayIndexOutOfBoundsException(index);
            }
            move(1, index);
            value[index] = ch;
            count++;
        }

        protected internal void insert0(int index, String s) {
            if (0 <= index && index <= count) {
                if (s == null) {
                    s = "null"; //$NON-NLS-1$
                }
                int min = s.length();
                if (min != 0) {
                    move(min, index);
                    s.getChars(0, min, value, index);
                    count += min;
                }
            } else {
                throw new StringIndexOutOfBoundsException(index);
            }
        }

        protected internal void insert0(int index, CharSequence s, int start, int end) {
            if (s == null) {
                s = new StringJ("null"); //$NON-NLS-1$
            }
            if (index < 0 || index > count || start < 0 || end < 0 || start > end
                    || end > s.length()) {
                throw new IndexOutOfBoundsException();
            }
            insert0(index, s.subSequence(start, end).toString());
        }

        /**
         * The current length.
         * 
         * @return the number of characters contained in this instance.
         */
        public virtual int length() {
            return count;
        }

        private void move(int size, int index) {
            int newSize;
            if (value.Length - count >= size) {
                if (!shared) {
                    java.lang.SystemJ.arraycopy(value, index, value, index + size, count
                            - index); // index == count case is no-op
                    return;
                }
                newSize = value.Length;
            } else {
                int a = count + size, b = (value.Length << 1) + 2;
                newSize = a > b ? a : b;
            }

            char[] newData = new char[newSize];
            java.lang.SystemJ.arraycopy(value, 0, newData, 0, index);
            // index == count case is no-op
            java.lang.SystemJ.arraycopy(value, index, newData, index + size, count - index);
            value = newData;
            shared = false;
        }

        protected internal void replace0(int start, int end, String s) {
            if (start >= 0) {
                if (end > count) {
                    end = count;
                }
                if (end > start) {
                    int stringLength = s.length();
                    int diff = end - start - stringLength;
                    if (diff > 0) { // replacing with fewer characters
                        if (!shared) {
                            // index == count case is no-op
                            java.lang.SystemJ.arraycopy(value, end, value, start
                                    + stringLength, count - end);
                        } else {
                            char[] newData = new char[value.Length];
                            java.lang.SystemJ.arraycopy(value, 0, newData, 0, start);
                            // index == count case is no-op
                            java.lang.SystemJ.arraycopy(value, end, newData, start
                                    + stringLength, count - end);
                            value = newData;
                            shared = false;
                        }
                    } else if (diff < 0) {
                        // replacing with more characters...need some room
                        move(-diff, end);
                    } else if (shared) {
                        char[] clone = new char[value.Length];
                        SystemJ.arraycopy(value, 0, clone, 0, value.Length);
                        value = clone;
                        shared = false;
                    }
                    s.getChars(0, stringLength, value, start);
                    count -= diff;
                    return;
                }
                if (start == end) {
                    if (s == null) {
                        throw new NullPointerException();
                    }
                    insert0(start, s);
                    return;
                }
            }
            throw new StringIndexOutOfBoundsException();
        }

        protected internal void reverse0() {
            if (count < 2) {
                return;
            }
            if (!shared) {
                int end = count - 1;
                char frontHigh = value[0];
                char endLow = value[end];
                bool allowFrontSur = true, allowEndSur = true;
                for (int i = 0, mid = count / 2; i < mid; i++, --end) {
                    char frontLow = value[i + 1];
                    char endHigh = value[end - 1];
                    bool surAtFront = allowFrontSur && frontLow >= 0xdc00
                            && frontLow <= 0xdfff && frontHigh >= 0xd800
                            && frontHigh <= 0xdbff;
                    if (surAtFront && (count < 3)) {
                        return;
                    }
                    bool surAtEnd = allowEndSur && endHigh >= 0xd800
                            && endHigh <= 0xdbff && endLow >= 0xdc00
                            && endLow <= 0xdfff;
                    allowFrontSur = allowEndSur = true;
                    if (surAtFront == surAtEnd) {
                        if (surAtFront) {
                            // both surrogates
                            value[end] = frontLow;
                            value[end - 1] = frontHigh;
                            value[i] = endHigh;
                            value[i + 1] = endLow;
                            frontHigh = value[i + 2];
                            endLow = value[end - 2];
                            i++;
                            end--;
                        } else {
                            // neither surrogates
                            value[end] = frontHigh;
                            value[i] = endLow;
                            frontHigh = frontLow;
                            endLow = endHigh;
                        }
                    } else {
                        if (surAtFront) {
                            // surrogate only at the front
                            value[end] = frontLow;
                            value[i] = endLow;
                            endLow = endHigh;
                            allowFrontSur = false;
                        } else {
                            // surrogate only at the end
                            value[end] = frontHigh;
                            value[i] = endHigh;
                            frontHigh = frontLow;
                            allowEndSur = false;
                        }
                    }
                }
                if ((count & 1) == 1 && (!allowFrontSur || !allowEndSur)) {
                    value[end] = allowFrontSur ? endLow : frontHigh;
                }
            } else {
                char[] newData = new char[value.Length];
                for (int i = 0, end = count; i < count; i++) {
                    char high = value[i];
                    if ((i + 1) < count && high >= 0xd800 && high <= 0xdbff) {
                        char low = value[i + 1];
                        if (low >= 0xdc00 && low <= 0xdfff) {
                            newData[--end] = low;
                            i++;
                        }
                    }
                    newData[--end] = high;
                }
                value = newData;
                shared = false;
            }
        }

        /**
         * Sets the character at the {@code index}.
         * 
         * @param index
         *            the zero-based index of the character to replace.
         * @param ch
         *            the character to set.
         * @throws IndexOutOfBoundsException
         *             if {@code index} is negative or greater than or equal to the
         *             current {@link #length()}.
         */
        public virtual void setCharAt(int index, char ch) {
            if (0 > index || index >= count) {
                throw new StringIndexOutOfBoundsException(index);
            }
            if (shared) {
                char[] clone = new char[value.Length];
                SystemJ.arraycopy(value, 0, clone, 0, value.Length);
                value = clone;
                shared = false;
            }
            value[index] = ch;
        }

        /**
         * Sets the current length to a new value. If the new length is larger than
         * the current length, then the new characters at the end of this object
         * will contain the {@code char} value of {@code \u0000}.
         * 
         * @param length
         *            the new length of this StringBuffer.
         * @exception IndexOutOfBoundsException
         *                if {@code length < 0}.
         * @see #length
         */
        public virtual void setLength(int length) {
            if (length < 0) {
                throw new StringIndexOutOfBoundsException(length);
            }
            if (length > value.Length) {
                enlargeBuffer(length);
            } else {
                if (shared) {
                    char[] newData = new char[value.Length];
                    java.lang.SystemJ.arraycopy(value, 0, newData, 0, count);
                    value = newData;
                    shared = false;
                } else {
                    if (count < length) {
                        java.util.Arrays<Object>.fill(value, count, length, (char) 0);
                    }
                }
            }
            count = length;
        }

        /**
         * Returns the String value of the subsequence from the {@code start} index
         * to the current end.
         * 
         * @param start
         *            the inclusive start index to begin the subsequence.
         * @return a String containing the subsequence.
         * @throws StringIndexOutOfBoundsException
         *             if {@code start} is negative or greater than the current
         *             {@link #length()}.
         */
        public virtual String substring(int start) {
            if (0 <= start && start <= count) {
                if (start == count) {
                    return ""; //$NON-NLS-1$
                }

                // Remove String sharing for more performance
                return new String(value, start, count - start);
            }
            throw new StringIndexOutOfBoundsException(start);
        }

        /**
         * Returns the String value of the subsequence from the {@code start} index
         * to the {@code end} index.
         * 
         * @param start
         *            the inclusive start index to begin the subsequence.
         * @param end
         *            the exclusive end index to end the subsequence.
         * @return a String containing the subsequence.
         * @throws StringIndexOutOfBoundsException
         *             if {@code start} is negative, greater than {@code end} or if
         *             {@code end} is greater than the current {@link #length()}.
         */
        public virtual String substring(int start, int end) {
            if (0 <= start && start <= end && end <= count) {
                if (start == end) {
                    return ""; //$NON-NLS-1$
                }

                // Remove String sharing for more performance
                return new String(value, start, end - start);
            }
            throw new StringIndexOutOfBoundsException();
        }

        /**
         * Returns the current String representation.
         * 
         * @return a String containing the characters in this instance.
         */
        
        public override String ToString() {
            if (count == 0) {
                return ""; //$NON-NLS-1$
            }
            // Optimize String sharing for more performance
            int wasted = value.Length - count;
            if (wasted >= 256
                    || (wasted >= INITIAL_CAPACITY && wasted >= (count >> 1))) {
                return new String(value, 0, count);
            }
            shared = true;
            return new StringJ(0, count, value).ToString();
        }

        /**
         * Returns a {@code CharSequence} of the subsequence from the {@code start}
         * index to the {@code end} index.
         * 
         * @param start
         *            the inclusive start index to begin the subsequence.
         * @param end
         *            the exclusive end index to end the subsequence.
         * @return a CharSequence containing the subsequence.
         * @throws IndexOutOfBoundsException
         *             if {@code start} is negative, greater than {@code end} or if
         *             {@code end} is greater than the current {@link #length()}.
         * @since 1.4
         */
        public virtual CharSequence subSequence(int start, int end) {
            return new StringJ(substring(start, end));
        }

        /**
         * Searches for the first index of the specified character. The search for
         * the character starts at the beginning and moves towards the end.
         * 
         * @param string
         *            the string to find.
         * @return the index of the specified character, -1 if the character isn't
         *         found.
         * @see #lastIndexOf(String)
         * @since 1.4
         */
        public virtual int indexOf(String s) {
            return indexOf(s, 0);
        }

        /**
         * Searches for the index of the specified character. The search for the
         * character starts at the specified offset and moves towards the end.
         * 
         * @param subString
         *            the string to find.
         * @param start
         *            the starting offset.
         * @return the index of the specified character, -1 if the character isn't
         *         found
         * @see #lastIndexOf(String,int)
         * @since 1.4
         */
        public virtual int indexOf(String subString, int start) {
            if (start < 0) {
                start = 0;
            }
            int subCount = subString.length();
            if (subCount > 0) {
                if (subCount + start > count) {
                    return -1;
                }
                // TODO optimize charAt to direct array access
                char firstChar = subString.charAt(0);
                while (true) {
                    int i = start;
                    bool found = false;
                    for (; i < count; i++) {
                        if (value[i] == firstChar) {
                            found = true;
                            break;
                        }
                    }
                    if (!found || subCount + i > count) {
                        return -1; // handles subCount > count || start >= count
                    }
                    int o1 = i, o2 = 0;
                    while (++o2 < subCount && value[++o1] == subString.charAt(o2)) {
                        // Intentionally empty
                    }
                    if (o2 == subCount) {
                        return i;
                    }
                    start = i + 1;
                }
            }
            return (start < count || start == 0) ? start : count;
        }

        /**
         * Searches for the last index of the specified character. The search for
         * the character starts at the end and moves towards the beginning.
         * 
         * @param string
         *            the string to find.
         * @return the index of the specified character, -1 if the character isn't
         *         found.
         * @throws NullPointerException
         *             if {@code string} is {@code null}.
         * @see String#lastIndexOf(java.lang.String)
         * @since 1.4
         */
        public virtual int lastIndexOf(String s) {
            return lastIndexOf(s, count);
        }

        /**
         * Searches for the index of the specified character. The search for the
         * character starts at the specified offset and moves towards the beginning.
         * 
         * @param subString
         *            the string to find.
         * @param start
         *            the starting offset.
         * @return the index of the specified character, -1 if the character isn't
         *         found.
         * @throws NullPointerException
         *             if {@code subString} is {@code null}.
         * @see String#lastIndexOf(String,int)
         * @since 1.4
         */
        public virtual int lastIndexOf(String subString, int start) {
            int subCount = subString.length();
            if (subCount <= count && start >= 0) {
                if (subCount > 0) {
                    if (start > count - subCount) {
                        start = count - subCount; // count and subCount are both
                    }
                    // >= 1
                    // TODO optimize charAt to direct array access
                    char firstChar = subString.charAt(0);
                    while (true) {
                        int i = start;
                        bool found = false;
                        for (; i >= 0; --i) {
                            if (value[i] == firstChar) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            return -1;
                        }
                        int o1 = i, o2 = 0;
                        while (++o2 < subCount
                                && value[++o1] == subString.charAt(o2)) {
                            // Intentionally empty
                        }
                        if (o2 == subCount) {
                            return i;
                        }
                        start = i - 1;
                    }
                }
                return start < count ? start : count;
            }
            return -1;
        }

        /**
         * Trims off any extra capacity beyond the current length. Note, this method
         * is NOT guaranteed to change the capacity of this object.
         * 
         * @since 1.5
         */
        public virtual void trimToSize() {
            if (count < value.Length) {
                char[] newValue = new char[count];
                java.lang.SystemJ.arraycopy(value, 0, newValue, 0, count);
                value = newValue;
                shared = false;
            }
        }

        /**
         * Retrieves the Unicode code point value at the {@code index}.
         * 
         * @param index
         *            the index to the {@code char} code unit.
         * @return the Unicode code point value.
         * @throws IndexOutOfBoundsException
         *             if {@code index} is negative or greater than or equal to
         *             {@link #length()}.
         * @see Character
         * @see Character#codePointAt(char[], int, int)
         * @since 1.5
         */
        public virtual int codePointAt(int index) {
            if (index < 0 || index >= count) {
                throw new StringIndexOutOfBoundsException(index);
            }
            return Character.codePointAt(value, index, count);
        }

        /**
         * Retrieves the Unicode code point value that precedes the {@code index}.
         * 
         * @param index
         *            the index to the {@code char} code unit within this object.
         * @return the Unicode code point value.
         * @throws IndexOutOfBoundsException
         *             if {@code index} is less than 1 or greater than
         *             {@link #length()}.
         * @see Character
         * @see Character#codePointBefore(char[], int, int)
         * @since 1.5
         */
        public virtual int codePointBefore(int index) {
            if (index < 1 || index > count) {
                throw new StringIndexOutOfBoundsException(index);
            }
            return Character.codePointBefore(value, index);
        }

        /**
         * Calculates the number of Unicode code points between {@code beginIndex}
         * and {@code endIndex}.
         * 
         * @param beginIndex
         *            the inclusive beginning index of the subsequence.
         * @param endIndex
         *            the exclusive end index of the subsequence.
         * @return the number of Unicode code points in the subsequence.
         * @throws IndexOutOfBoundsException
         *             if {@code beginIndex} is negative or greater than
         *             {@code endIndex} or {@code endIndex} is greater than
         *             {@link #length()}.
         * @see Character
         * @see Character#codePointCount(char[], int, int)
         * @since 1.5
         */
        public virtual int codePointCount(int beginIndex, int endIndex) {
            if (beginIndex < 0 || endIndex > count || beginIndex > endIndex) {
                throw new StringIndexOutOfBoundsException();
            }
            return Character.codePointCount(value, beginIndex, endIndex
                    - beginIndex);
        }

        /**
         * Returns the index that is offset {@code codePointOffset} code points from
         * {@code index}.
         *
         * @param index
         *            the index to calculate the offset from.
         * @param codePointOffset
         *            the number of code points to count.
         * @return the index that is {@code codePointOffset} code points away from
         *         index.
         * @throws IndexOutOfBoundsException
         *             if {@code index} is negative or greater than
         *             {@link #length()} or if there aren't enough code points
         *             before or after {@code index} to match
         *             {@code codePointOffset}.
         * @see Character
         * @see Character#offsetByCodePoints(char[], int, int, int, int)
         * @since 1.5
         */
        public virtual int offsetByCodePoints(int index, int codePointOffset) {
            return Character.offsetByCodePoints(value, 0, count, index,
                    codePointOffset);
        }
    }
}
