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

namespace biz.ritter.javapi.text
{

    public sealed class CharacterIteratorConstants
    {
        /**
         * A constant which indicates that there is no character at the current
         * index.
         */
        public const char DONE = '\uffff';
    }

    ///<summary>
    /// An interface for the bidirectional iteration over a group of characters. The
    /// iteration starts at the begin index in the group of characters and continues
    /// to one index before the end index.
    /// <strong>Note: DONE field is moved to </strong>
    ///</summary>
    public interface CharacterIterator : java.lang.Cloneable
    {

        /**
         * Returns the character at the current index.
         * 
         * @return the current character, or {@code DONE} if the current index is
         *         past the beginning or end of the sequence.
         */
        char current();

        /**
         * Sets the current position to the begin index and returns the character at
         * the new position.
         * 
         * @return the character at the begin index.
         */
        char first();

        /**
         * Returns the begin index.
         * 
         * @return the index of the first character of the iteration.
         */
        int getBeginIndex();

        /**
         * Returns the end index.
         * 
         * @return the index one past the last character of the iteration.
         */
        int getEndIndex();

        /**
         * Returns the current index.
         * 
         * @return the current index.
         */
        int getIndex();

        /**
         * Sets the current position to the end index - 1 and returns the character
         * at the new position.
         * 
         * @return the character before the end index.
         */
        char last();

        /**
         * Increments the current index and returns the character at the new index.
         * 
         * @return the character at the next index, or {@code DONE} if the next
         *         index would be past the end.
         */
        char next();

        /**
         * Decrements the current index and returns the character at the new index.
         * 
         * @return the character at the previous index, or {@code DONE} if the
         *         previous index would be past the beginning.
         */
        char previous();

        /**
         * Sets the current index to a new position and returns the character at the
         * new index.
         * 
         * @param location
         *            the new index that this character iterator is set to.
         * @return the character at the new index, or {@code DONE} if the index is
         *         past the end.
         * @throws IllegalArgumentException
         *         if {@code location} is less than the begin index or greater than
         *         the end index.
         */
        char setIndex(int location);
    }
}