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
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.text
{

    /**
     * Holds a string with attributes describing the characters of
     * this string.
     */
    public class AttributedString {

        protected internal System.String text;

        protected internal java.util.Map<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>> attributeMap;



        /**
         * Constructs an {@code AttributedString} from an {@code
         * AttributedCharacterIterator}, which represents attributed text.
         *
         * @param iterator
         *            the {@code AttributedCharacterIterator} that contains the text
         *            for this attributed string.
         */
        public AttributedString(AttributedCharacterIterator iterator) {
            if (iterator.getBeginIndex() > iterator.getEndIndex()) {
                // text.0A=Invalid substring range
                throw new java.lang.IllegalArgumentException("Invalid substring range"); //$NON-NLS-1$
            }
            StringBuilder buffer = new StringBuilder();
            for (int i = iterator.getBeginIndex(); i < iterator.getEndIndex(); i++) {
                buffer.Append(iterator.current());
                iterator.next();
            }
            text = buffer.ToString();
            java.util.Set<AttributedCharacterIteratorNS.Attribute> attributes = iterator
                    .getAllAttributeKeys();
            if (attributes == null) {
                return;
            }
            attributeMap = new java.util.HashMap<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>>();//(attributes.size() * 4 / 3) + 1);

            java.util.Iterator<AttributedCharacterIteratorNS.Attribute> it = attributes.iterator();
            while (it.hasNext()) {
                AttributedCharacterIteratorNS.Attribute attribute = it.next();
                iterator.setIndex(0);
                while (iterator.current() != CharacterIteratorConstants.DONE) {
                    int start = iterator.getRunStart(attribute);
                    int limit = iterator.getRunLimit(attribute);
                    System.Object value = iterator.getAttribute(attribute);
                    if (value != null) {
                        addAttribute(attribute, value, start, limit);
                    }
                    iterator.setIndex(limit);
                }
            }
        }

        private AttributedString(AttributedCharacterIterator iterator, int start,
                int end, java.util.Set<AttributedCharacterIteratorNS.Attribute> attributes) {
            if (start < iterator.getBeginIndex() || end > iterator.getEndIndex()
                    || start > end) {
                throw new java.lang.IllegalArgumentException();
            }

            if (attributes == null) {
                return;
            }

            StringBuilder buffer = new StringBuilder();
            iterator.setIndex(start);
            while (iterator.getIndex() < end) {
                buffer.Append(iterator.current());
                iterator.next();
            }
            text = buffer.ToString();
            attributeMap = new java.util.HashMap<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>>();//(attributes.size() * 4 / 3) + 1);

            java.util.Iterator<AttributedCharacterIteratorNS.Attribute> it = attributes.iterator();
            while (it.hasNext()) {
                AttributedCharacterIteratorNS.Attribute attribute = it.next();
                iterator.setIndex(start);
                while (iterator.getIndex() < end) {
                    System.Object value = iterator.getAttribute(attribute);
                    int runStart = iterator.getRunStart(attribute);
                    int limit = iterator.getRunLimit(attribute);
                    if ((value is java.lang.annotation.Annotation && runStart >= start && limit <= end)
                            || (value != null && !(value is java.lang.annotation.Annotation))) {
                        addAttribute(attribute, value, (runStart < start ? start
                                : runStart)
                                - start, (limit > end ? end : limit) - start);
                    }
                    iterator.setIndex(limit);
                }
            }
        }

        /**
         * Constructs an {@code AttributedString} from a range of the text contained
         * in the specified {@code AttributedCharacterIterator}, starting at {@code
         * start} and ending at {@code end}. All attributes will be copied to this
         * attributed string.
         *
         * @param iterator
         *            the {@code AttributedCharacterIterator} that contains the text
         *            for this attributed string.
         * @param start
         *            the start index of the range of the copied text.
         * @param end
         *            the end index of the range of the copied text.
         * @throws IllegalArgumentException
         *             if {@code start} is less than first index of
         *             {@code iterator}, {@code end} is greater than the last
         *             index + 1 in {@code iterator} or if {@code start > end}.
         */
        public AttributedString(AttributedCharacterIterator iterator, int start,
                int end) :this(iterator, start, end, iterator.getAllAttributeKeys()){
        }

        /**
         * Constructs an {@code AttributedString} from a range of the text contained
         * in the specified {@code AttributedCharacterIterator}, starting at {@code
         * start}, ending at {@code end} and it will copy the attributes defined in
         * the specified set. If the set is {@code null} then all attributes are
         * copied.
         *
         * @param iterator
         *            the {@code AttributedCharacterIterator} that contains the text
         *            for this attributed string.
         * @param start
         *            the start index of the range of the copied text.
         * @param end
         *            the end index of the range of the copied text.
         * @param attributes
         *            the set of attributes that will be copied, or all if it is
         *            {@code null}.
         * @throws IllegalArgumentException
         *             if {@code start} is less than first index of
         *             {@code iterator}, {@code end} is greater than the last index +
         *             1 in {@code iterator} or if {@code start > end}.
         */
        public AttributedString(AttributedCharacterIterator iterator, int start,
                int end, AttributedCharacterIteratorNS.Attribute[] attributes)
            : this(iterator, start, end, new java.util.HashSet<AttributedCharacterIteratorNS.Attribute>(java.util.Arrays<System.Object>.asList(attributes))){
        }

        /**
         * Creates an {@code AttributedString} from the given text.
         *
         * @param value
         *            the text to take as base for this attributed string.
         */
        public AttributedString(System.String value) {
            if (value == null) {
                throw new java.lang.NullPointerException();
            }
            text = value;
            attributeMap = new java.util.HashMap<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>>();//11);
        }

        /**
         * Creates an {@code AttributedString} from the given text and the
         * attributes. The whole text has the given attributes applied.
         *
         * @param value
         *            the text to take as base for this attributed string.
         * @param attributes
         *            the attributes that the text is associated with.
         * @throws IllegalArgumentException
         *             if the length of {@code value} is 0 but the size of {@code
         *             attributes} is greater than 0.
         * @throws NullPointerException
         *             if {@code value} is {@code null}.
         */
        public AttributedString(System.String value,
                java.util.Map<AttributedCharacterIteratorNS.Attribute, System.Object> attributes) {
            if (value == null) {
                throw new java.lang.NullPointerException();
            }
            if (value.Length == 0 && !attributes.isEmpty()) {
                // text.0B=Cannot add attributes to empty string
                throw new java.lang.IllegalArgumentException("Cannot add attributes to empty string"); //$NON-NLS-1$
            }
            text = value;
            attributeMap = new java.util.HashMap<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>>();//(attributes.size() * 4 / 3) + 1);
            java.util.Iterator<java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute,System.Object>> it = attributes.entrySet().iterator();
            while (it.hasNext()) {
                java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute, System.Object> entry = it.next();
                java.util.ArrayList<IAC_Range> ranges = new java.util.ArrayList<IAC_Range>(1);
                ranges.add(new IAC_Range(0, text.Length, entry.getValue()));
                attributeMap.put((AttributedCharacterIteratorNS.Attribute) entry
                        .getKey(), ranges);
            }
        }

        /**
         * Applies a given attribute to this string.
         *
         * @param attribute
         *            the attribute that will be applied to this string.
         * @param value
         *            the value of the attribute that will be applied to this
         *            string.
         * @throws IllegalArgumentException
         *             if the length of this attributed string is 0.
         * @throws NullPointerException
         *             if {@code attribute} is {@code null}.
         */
        public void addAttribute(AttributedCharacterIteratorNS.Attribute attribute,
                System.Object value) {
            if (null == attribute) {
                throw new java.lang.NullPointerException();
            }
            if (text.Length == 0) {
                throw new java.lang.IllegalArgumentException();
            }

            java.util.List<IAC_Range> ranges = attributeMap.get(attribute);
            if (ranges == null) {
                ranges = new java.util.ArrayList<IAC_Range>(1);
                attributeMap.put(attribute, ranges);
            } else {
                ranges.clear();
            }
            ranges.add(new IAC_Range(0, text.Length, value));
        }

        /**
         * Applies a given attribute to the given range of this string.
         *
         * @param attribute
         *            the attribute that will be applied to this string.
         * @param value
         *            the value of the attribute that will be applied to this
         *            string.
         * @param start
         *            the start of the range where the attribute will be applied.
         * @param end
         *            the end of the range where the attribute will be applied.
         * @throws IllegalArgumentException
         *             if {@code start < 0}, {@code end} is greater than the length
         *             of this string, or if {@code start >= end}.
         * @throws NullPointerException
         *             if {@code attribute} is {@code null}.
         */
        public void addAttribute(AttributedCharacterIteratorNS.Attribute attribute,
                System.Object value, int start, int end) {
            if (null == attribute) {
                throw new java.lang.NullPointerException();
            }
            if (start < 0 || end > text.Length || start >= end) {
                throw new java.lang.IllegalArgumentException();
            }

            if (value == null) {
                return;
            }

            java.util.List<IAC_Range> ranges = attributeMap.get(attribute);
            if (ranges == null) {
                ranges = new java.util.ArrayList<IAC_Range>(1);
                ranges.add(new IAC_Range(start, end, value));
                attributeMap.put(attribute, ranges);
                return;
            }
            java.util.ListIterator<IAC_Range> it = ranges.listIterator();
            while (it.hasNext()) {
                IAC_Range range = it.next();
                if (end <= range.start) {
                    it.previous();
                    break;
                } else if (start < range.end
                        || (start == range.end && value.Equals(range.value))) {
                    IAC_Range r1 = null, r3;
                    it.remove();
                    r1 = new IAC_Range(range.start, start, range.value);
                    r3 = new IAC_Range(end, range.end, range.value);

                    while (end > range.end && it.hasNext()) {
                        range = it.next();
                        if (end <= range.end) {
                            if (end > range.start
                                    || (end == range.start && value.Equals(range.value))) {
                                it.remove();
                                r3 = new IAC_Range(end, range.end, range.value);
                                break;
                            }
                        } else {
                            it.remove();
                        }
                    }

                    if (value.Equals(r1.value)) {
                        if (value.Equals(r3.value)) {
                            it.add(new IAC_Range(r1.start < start ? r1.start : start,
                                    r3.end > end ? r3.end : end, r1.value));
                        } else {
                            it.add(new IAC_Range(r1.start < start ? r1.start : start,
                                    end, r1.value));
                            if (r3.start < r3.end) {
                                it.add(r3);
                            }
                        }
                    } else {
                        if (value.Equals(r3.value)) {
                            if (r1.start < r1.end) {
                                it.add(r1);
                            }
                            it.add(new IAC_Range(start, r3.end > end ? r3.end : end,
                                    r3.value));
                        } else {
                            if (r1.start < r1.end) {
                                it.add(r1);
                            }
                            it.add(new IAC_Range(start, end, value));
                            if (r3.start < r3.end) {
                                it.add(r3);
                            }
                        }
                    }
                    return;
                }
            }
            it.add(new IAC_Range(start, end, value));
        }

        /**
         * Applies a given set of attributes to the given range of the string.
         *
         * @param attributes
         *            the set of attributes that will be applied to this string.
         * @param start
         *            the start of the range where the attribute will be applied.
         * @param end
         *            the end of the range where the attribute will be applied.
         * @throws IllegalArgumentException
         *             if {@code start < 0}, {@code end} is greater than the length
         *             of this string, or if {@code start >= end}.
         */
        public void addAttributes(
                java.util.Map<AttributedCharacterIteratorNS.Attribute, System.Object> attributes,
                int start, int end) {
            java.util.Iterator<java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute,System.Object>> it = attributes.entrySet().iterator();
            while (it.hasNext()) {
                java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute, System.Object> entry = it.next();
                addAttribute(entry.getKey(),
                        entry.getValue(), start, end);
            }
        }

        /**
         * Returns an {@code AttributedCharacterIterator} that gives access to the
         * complete content of this attributed string.
         *
         * @return the newly created {@code AttributedCharacterIterator}.
         */
        public AttributedCharacterIterator getIterator() {
            return new AttributedIterator(this);
        }

        /**
         * Returns an {@code AttributedCharacterIterator} that gives access to the
         * complete content of this attributed string. Only attributes contained in
         * {@code attributes} are available from this iterator if they are defined
         * for this text.
         *
         * @param attributes
         *            the array containing attributes that will be in the new
         *            iterator if they are defined for this text.
         * @return the newly created {@code AttributedCharacterIterator}.
         */
        public AttributedCharacterIterator getIterator(
                AttributedCharacterIteratorNS.Attribute[] attributes) {
            return new AttributedIterator(this, attributes, 0, text.Length);
        }

        /**
         * Returns an {@code AttributedCharacterIterator} that gives access to the
         * contents of this attributed string starting at index {@code start} up to
         * index {@code end}. Only attributes contained in {@code attributes} are
         * available from this iterator if they are defined for this text.
         *
         * @param attributes
         *            the array containing attributes that will be in the new
         *            iterator if they are defined for this text.
         * @param start
         *            the start index of the iterator on the underlying text.
         * @param end
         *            the end index of the iterator on the underlying text.
         * @return the newly created {@code AttributedCharacterIterator}.
         */
        public AttributedCharacterIterator getIterator(
                AttributedCharacterIteratorNS.Attribute[] attributes, int start,
                int end) {
            return new AttributedIterator(this, attributes, start, end);
        }
    }

    public class IAC_Range {
        protected internal int start;

        protected internal int end;

        protected internal System.Object value;

        internal IAC_Range(int s, int e, System.Object v) {
            start = s;
            end = e;
            value = v;
        }
    }

    internal class AttributedIterator : AttributedCharacterIterator {

        private int begin, end, offset;

        private AttributedString attrString;

        private java.util.HashSet<AttributedCharacterIteratorNS.Attribute> attributesAllowed;

        internal AttributedIterator(AttributedString attrString) {
            this.attrString = attrString;
            begin = 0;
            end = attrString.text.Length;
            offset = 0;
        }

        internal AttributedIterator(AttributedString attrString,
                AttributedCharacterIteratorNS.Attribute[] attributes, int begin,
                int end) {
            if (begin < 0 || end > attrString.text.Length || begin > end) {
                throw new java.lang.IllegalArgumentException();
            }
            this.begin = begin;
            this.end = end;
            offset = begin;
            this.attrString = attrString;
            if (attributes != null) {
                java.util.HashSet<AttributedCharacterIteratorNS.Attribute> set = new java.util.HashSet<AttributedCharacterIteratorNS.Attribute>(
                        (attributes.Length * 4 / 3) + 1);
                for (int i = attributes.Length; --i >= 0;) {
                    set.add(attributes[i]);
                }
                attributesAllowed = set;
            }
        }

        /**
            * Returns a new {@code AttributedIterator} with the same source string,
            * begin, end, and current index as this attributed iterator.
            * 
            * @return a shallow copy of this attributed iterator.
            * @see java.lang.Cloneable
            */
        public System.Object clone() {
            try {
                AttributedIterator clone = (AttributedIterator) MemberwiseClone();
                if (attributesAllowed != null) {
                    clone.attributesAllowed = (java.util.HashSet<AttributedCharacterIteratorNS.Attribute>) attributesAllowed.clone();
                }
                return clone;
            } catch (java.lang.CloneNotSupportedException e) {
                return null;
            }
        }

        public char current() {
            if (offset == end) {
                return CharacterIteratorConstants.DONE;
            }
            return attrString.text[offset];
        }

        public char first() {
            if (begin == end) {
                return CharacterIteratorConstants.DONE;
            }
            offset = begin;
            return attrString.text[offset];
        }

        /**
            * Returns the begin index in the source string.
            * 
            * @return the index of the first character to iterate.
            */
        public int getBeginIndex() {
            return begin;
        }

        /**
            * Returns the end index in the source String.
            * 
            * @return the index one past the last character to iterate.
            */
        public int getEndIndex() {
            return end;
        }

        /**
            * Returns the current index in the source String.
            * 
            * @return the current index.
            */
        public int getIndex() {
            return offset;
        }

        private bool inRange(IAC_Range range) {
            if (!(range.value is java.lang.annotation.Annotation)) {
                return true;
            }
            return range.start >= begin && range.start < end
                    && range.end > begin && range.end <= end;
        }

        private bool inRange(java.util.List<IAC_Range> ranges) {
            java.util.Iterator<IAC_Range> it = ranges.iterator();
            while (it.hasNext()) {
                IAC_Range range = it.next();
                if (range.start >= begin && range.start < end) {
                    return !(range.value is java.lang.annotation.Annotation)
                            || (range.end > begin && range.end <= end);
                } else if (range.end > begin && range.end <= end) {
                    return !(range.value is java.lang.annotation.Annotation)
                            || (range.start >= begin && range.start < end);
                }
            }
            return false;
        }

        /**
            * Returns a set of attributes present in the {@code AttributedString}.
            * An empty set returned indicates that no attributes where defined.
            *
            * @return a set of attribute keys that may be empty.
            */
        public java.util.Set<AttributedCharacterIteratorNS.Attribute> getAllAttributeKeys() {
            if (begin == 0 && end == attrString.text.Length
                    && attributesAllowed == null) {
                return attrString.attributeMap.keySet();
            }

            java.util.Set<AttributedCharacterIteratorNS.Attribute> result = new java.util.HashSet<AttributedCharacterIteratorNS.Attribute>();//(attrString.attributeMap.size() * 4 / 3) + 1);
            java.util.Iterator<java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>>> it = attrString.attributeMap
                    .entrySet().iterator();
            while (it.hasNext()) {
                java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>> entry = it.next();
                if (attributesAllowed == null
                        || attributesAllowed.contains(entry.getKey())) {
                    java.util.List<IAC_Range> ranges = entry.getValue();
                    if (inRange(ranges)) {
                        result.add(entry.getKey());
                    }
                }
            }
            return result;
        }

        private System.Object currentValue(java.util.List<IAC_Range> ranges) {
            java.util.Iterator<IAC_Range> it = ranges.iterator();
            while (it.hasNext()) {
                IAC_Range range = it.next();
                if (offset >= range.start && offset < range.end) {
                    return inRange(range) ? range.value : null;
                }
            }
            return null;
        }

        public System.Object getAttribute(
                AttributedCharacterIteratorNS.Attribute attribute) {
            if (attributesAllowed != null
                    && !attributesAllowed.contains(attribute)) {
                return null;
            }
            java.util.ArrayList<IAC_Range> ranges = (java.util.ArrayList<IAC_Range>) attrString.attributeMap
                    .get(attribute);
            if (ranges == null) {
                return null;
            }
            return currentValue(ranges);
        }

        public java.util.Map<AttributedCharacterIteratorNS.Attribute, System.Object> getAttributes() {
            java.util.Map<AttributedCharacterIteratorNS.Attribute, System.Object> result = new java.util.HashMap<AttributedCharacterIteratorNS.Attribute, System.Object>();//(attrString.attributeMap.size() * 4 / 3) + 1);
            java.util.Iterator<java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>>> it = attrString.attributeMap
                    .entrySet().iterator();
            while (it.hasNext()) {
                java.util.MapNS.Entry<AttributedCharacterIteratorNS.Attribute, java.util.List<IAC_Range>> entry = it.next();
                if (attributesAllowed == null
                        || attributesAllowed.contains(entry.getKey())) {
                    System.Object value = currentValue(entry.getValue());
                    if (value != null) {
                        result.put(entry.getKey(), value);
                    }
                }
            }
            return result;
        }

        public int getRunLimit() {
            return getRunLimit(getAllAttributeKeys());
        }

        private int runLimit(java.util.List<IAC_Range> ranges) {
            int result = end;
            java.util.ListIterator<IAC_Range> it = ranges.listIterator(ranges.size());
            while (it.hasPrevious()) {
                IAC_Range range = it.previous();
                if (range.end <= begin) {
                    break;
                }
                if (offset >= range.start && offset < range.end) {
                    return inRange(range) ? range.end : result;
                } else if (offset >= range.end) {
                    break;
                }
                result = range.start;
            }
            return result;
        }

        public int getRunLimit(AttributedCharacterIteratorNS.Attribute attribute) {
            if (attributesAllowed != null
                    && !attributesAllowed.contains(attribute)) {
                return end;
            }
            java.util.ArrayList<IAC_Range> ranges = (java.util.ArrayList<IAC_Range>) attrString.attributeMap
                    .get(attribute);
            if (ranges == null) {
                return end;
            }
            return runLimit(ranges);
        }

        public int getRunLimit(java.util.Set<AttributedCharacterIteratorNS.Attribute> attributes) {
            int limit = end;
            java.util.Iterator<AttributedCharacterIteratorNS.Attribute> it = attributes.iterator();
            while (it.hasNext()) {
                AttributedCharacterIteratorNS.Attribute attribute = it.next();
                int newLimit = getRunLimit(attribute);
                if (newLimit < limit) {
                    limit = newLimit;
                }
            }
            return limit;
        }

        public int getRunStart() {
            return getRunStart(getAllAttributeKeys());
        }

        private int runStart(java.util.List<IAC_Range> ranges) {
            int result = begin;
            java.util.Iterator<IAC_Range> it = ranges.iterator();
            while (it.hasNext()) {
                IAC_Range range = it.next();
                if (range.start >= end) {
                    break;
                }
                if (offset >= range.start && offset < range.end) {
                    return inRange(range) ? range.start : result;
                } else if (offset < range.start) {
                    break;
                }
                result = range.end;
            }
            return result;
        }

        public int getRunStart(AttributedCharacterIteratorNS.Attribute attribute) {
            if (attributesAllowed != null
                    && !attributesAllowed.contains(attribute)) {
                return begin;
            }
            java.util.ArrayList<IAC_Range> ranges = (java.util.ArrayList<IAC_Range>) attrString.attributeMap
                    .get(attribute);
            if (ranges == null) {
                return begin;
            }
            return runStart(ranges);
        }

        public int getRunStart(java.util.Set<AttributedCharacterIteratorNS.Attribute> attributes) {
            int start = begin;
            java.util.Iterator<AttributedCharacterIteratorNS.Attribute> it = attributes.iterator();
            while (it.hasNext()) {
                AttributedCharacterIteratorNS.Attribute attribute = it.next();
                int newStart = getRunStart(attribute);
                if (newStart > start) {
                    start = newStart;
                }
            }
            return start;
        }

        public char last() {
            if (begin == end) {
                return CharacterIteratorConstants.DONE;
            }
            offset = end - 1;
            return attrString.text[offset];
        }

        public char next() {
            if (offset >= (end - 1)) {
                offset = end;
                return CharacterIteratorConstants.DONE;
            }
            return attrString.text[++offset];
        }

        public char previous() {
            if (offset == begin) {
                return CharacterIteratorConstants.DONE;
            }
            return attrString.text[--offset];
        }

        public char setIndex(int location) {
            if (location < begin || location > end) {
                throw new java.lang.IllegalArgumentException();
            }
            offset = location;
            if (offset == end) {
                return CharacterIteratorConstants.DONE;
            }
            return attrString.text[offset];
        }
    }
}
