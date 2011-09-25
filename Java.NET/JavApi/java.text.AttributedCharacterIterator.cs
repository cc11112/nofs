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
    /**
     * Extends the
     * {@link CharacterIterator} interface, adding support for iterating over
     * attributes and not only characters. An
     * {@code AttributedCharacterIterator} also allows the user to find runs and
     * their limits. Runs are defined as ranges of characters that all have the same
     * attributes with the same values.
     */
    public interface AttributedCharacterIterator : CharacterIterator {

        /**
         * Returns a set of attributes present in the {@code
         * AttributedCharacterIterator}. An empty set is returned if no attributes
         * were defined.
         *
         * @return a set of attribute keys; may be empty.
         */
        java.util.Set<AttributedCharacterIteratorNS.Attribute> getAllAttributeKeys();

        /**
         * Returns the value stored in the attribute for the current character. If
         * the attribute was not defined then {@code null} is returned.
         *
         * @param attribute the attribute for which the value should be returned.
         * @return the value of the requested attribute for the current character or
         *         {@code null} if it was not defined.
         */
        Object getAttribute(AttributedCharacterIteratorNS.Attribute attribute);

        /**
         * Returns a map of all attributes of the current character. If no
         * attributes were defined for the current character then an empty map is
         * returned.
         *
         * @return a map of all attributes for the current character or an empty
         *         map.
         */
        java.util.Map<AttributedCharacterIteratorNS.Attribute, Object> getAttributes();

        /**
         * Returns the index of the last character in the run having the same
         * attributes as the current character.
         *
         * @return the index of the last character of the current run.
         */
        int getRunLimit();

        /**
         * Returns the index of the last character in the run that has the same
         * attribute value for the given attribute as the current character.
         *
         * @param attribute
         *            the attribute which the run is based on.
         * @return the index of the last character of the current run.
         */
        int getRunLimit(AttributedCharacterIteratorNS.Attribute attribute);

        /**
         * Returns the index of the last character in the run that has the same
         * attribute values for the attributes in the set as the current character.
         *
         * @param attributes
         *            the set of attributes which the run is based on.
         * @return the index of the last character of the current run.
         */
        int getRunLimit(java.util.Set<AttributedCharacterIteratorNS.Attribute> attributes);

        /**
         * Returns the index of the first character in the run that has the same
         * attributes as the current character.
         *
         * @return the index of the last character of the current run.
         */
        int getRunStart();

        /**
         * Returns the index of the first character in the run that has the same
         * attribute value for the given attribute as the current character.
         *
         * @param attribute
         *            the attribute which the run is based on.
         * @return the index of the last character of the current run.
         */
        int getRunStart(AttributedCharacterIteratorNS.Attribute attribute);

        /**
         * Returns the index of the first character in the run that has the same
         * attribute values for the attributes in the set as the current character.
         *
         * @param attributes
         *            the set of attributes which the run is based on.
         * @return the index of the last character of the current run.
         */
        int getRunStart(java.util.Set<AttributedCharacterIteratorNS.Attribute> attributes);
    }
    namespace AttributedCharacterIteratorNS {
        /**
         * Defines keys for text attributes.
         */
        [Serializable]
        public class Attribute : java.io.Serializable {

            private static readonly long serialVersionUID = -9142742483513960612L;

            /**
             * This attribute marks segments from an input method. Most input
             * methods create these segments for words.
             *
             * The value objects are of the type {@code Annotation} which contain
             * {@code null}.
             */
            public static readonly Attribute INPUT_METHOD_SEGMENT = new Attribute(
                    "input_method_segment"); //$NON-NLS-1$

            /**
             * The attribute describing the language of a character. The value
             * objects are of type {@code Locale} or a subtype of it.
             */
            public static readonly Attribute LANGUAGE = new Attribute("language"); //$NON-NLS-1$

            /**
             * For languages that have different reading directions of text (like
             * Japanese), this attribute allows to define which reading should be
             * used. The value objects are of type {@code Annotation} which
             * contain a {@code String}.
             */
            public static readonly Attribute READING = new Attribute("reading"); //$NON-NLS-1$

            private String name;

            /**
             * The constructor for an {@code Attribute} with the name passed.
             *
             * @param name
             *            the name of the new {@code Attribute}.
             */
            protected Attribute(String name) {
                this.name = name;
            }

            /**
             * Compares this attribute with the specified object. Checks if both
             * objects are the same instance. It is defined final so all subclasses
             * have the same behavior for this method.
             *
             * @param object
             *            the object to compare against.
             * @return {@code true} if the object passed is equal to this instance;
             *         {@code false} otherwise.
             */
            
            public override bool Equals(Object obj) {
                return this == obj;
            }

            /**
             * Returns the name of this attribute.
             *
             * @return the name of this attribute.
             */
            protected String getName() {
                return name;
            }

            /**
             * Calculates the hash code for objects of type {@code Attribute}. It
             * is defined final so all sub types calculate their hash code
             * identically.
             *
             * @return the hash code for this instance of {@code Attribute}.
             */
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }

            /**
             * Resolves a deserialized instance to the correct constant attribute.
             *
             * @return the {@code Attribute} this instance represents.
             * @throws InvalidObjectException
             *             if this instance is not of type {@code Attribute.class}
             *             or if it is not a known {@code Attribute}.
             */
            protected virtual Object readResolve() {//throws InvalidObjectException {
                if (this.GetType() != typeof(Attribute)) {
                    // text.0C=cannot resolve subclasses
                    throw new java.io.InvalidObjectException("cannot resolve subclasses"); //$NON-NLS-1$
                }
                if (this.getName().equals(INPUT_METHOD_SEGMENT.getName())) {
                    return INPUT_METHOD_SEGMENT;
                }
                if (this.getName().equals(LANGUAGE.getName())) {
                    return LANGUAGE;
                }
                if (this.getName().equals(READING.getName())) {
                    return READING;
                }
                // text.02=Unknown attribute
                throw new java.io.InvalidObjectException("Unknown attribute"); //$NON-NLS-1$
            }

            /**
             * Returns the name of the class followed by a "(", the name of the
             * attribute, and a ")".
             *
             * @return the string representing this instance.
             */
            public override String ToString() {
                return GetType().Name + '(' + getName() + ')';
            }
        }
    }
}
