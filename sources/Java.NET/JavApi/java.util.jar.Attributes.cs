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
using org.apache.harmony.archive.util;

namespace biz.ritter.javapi.util.jar
{
    #region Attributes
    /**
     * The {@code Attributes} class is used to store values for manifest entries.
     * Attribute keys are generally instances of {@code Attributes.Name}. Values
     * associated with attribute keys are of type {@code String}.
     */
    public class Attributes : java.lang.Cloneable, java.util.Map<Object, Object> {

        /**
         * The {@code Attributes} as name/value pairs. Maps the attribute names (as
         * {@link Attributes.Name}) of a JAR file manifest to arbitrary values. The
         * attribute names thus are obtained from the {@link Manifest} for
         * convenience.
         */
        protected Map<Object, Object> map;


        /**
         * Constructs an {@code Attributes} instance.
         */
        public Attributes() {
            map = new HashMap<Object, Object>();
        }

        /**
         * Constructs an {@code Attributes} instance obtaining keys and values from
         * the parameter {@code attrib}.
         *
         * @param attrib
         *            The attributes to obtain entries from.
         */
        public Attributes(Attributes attrib) {
            map = (Map<Object, Object>) ((java.util.HashMap<Object,Object>) attrib.map).clone();
        }

        /**
         * Constructs an {@code Attributes} instance with initial capacity of size
         * {@code size}.
         *
         * @param size
         *            Initial size of this {@code Attributes} instance.
         */
        public Attributes(int size) {
            map = new java.util.HashMap<Object, Object>();//size);
        }

        /**
         * Removes all key/value pairs from this {@code Attributes}.
         */
        public void clear() {
            map.clear();
        }

        /**
         * Determines whether this {@code Attributes} contains the specified key.
         *
         * @param key
         *            The key to search for.
         * @return {@code true} if the key is found, {@code false} otherwise.
         */
        public bool containsKey(Object key) {
            return map.containsKey(key);
        }

        /**
         * Determines whether this {@code Attributes} contains the specified value.
         *
         * @param value
         *            the value to search for.
         * @return {@code true} if the value is found, {@code false} otherwise.
         */
        public bool containsValue(Object value) {
            return map.containsValue(value);
        }

        /**
         * Returns a set containing map entries for each of the key/value pair
         * contained in this {@code Attributes}.
         *
         * @return a set of Map.Entry's
         */
        public Set<java.util.MapNS.Entry<Object, Object>> entrySet() {
            return map.entrySet();
        }

        /**
         * Returns the value associated with the parameter key.
         *
         * @param key
         *            the key to search for.
         * @return Object associated with key, or {@code null} if key does not
         *         exist.
         */
        public Object get(Object key) {
            return map.get(key);
        }

        /**
         * Determines whether this {@code Attributes} contains any keys.
         *
         * @return {@code true} if one or more keys exist, {@code false} otherwise.
         */
        public bool isEmpty() {
            return map.isEmpty();
        }

        /**
         * Returns a {@code Set} containing all the keys found in this {@code
         * Attributes}.
         *
         * @return a {@code Set} of all keys.
         */
        public Set<Object> keySet() {
            return map.keySet();
        }

        /**
         * Stores key/value pairs in this {@code Attributes}.
         *
         * @param key
         *            the key to associate with value.
         * @param value
         *            the value to store in this {@code Attributes}.
         * @return the value being stored.
         * @exception ClassCastException
         *                when key is not an {@code Attributes.Name} or value is not
         *                a {@code String}.
         */
        // Require cast to force ClassCastException
        public Object put(Object key, Object value) {
            return map.put((AttributesNS.Name) key, (String) value);
        }

        /**
         * Stores all the key/value pairs in the argument in this {@code
         * Attributes}.
         *
         * @param attrib
         *            the associations to store (must be of type {@code
         *            Attributes}).
         */
        public void putAll(Map<Object, Object> attrib) {
            if (attrib == null || !(attrib is Attributes)) {
                throw new java.lang.ClassCastException();
            }
            this.map.putAll(attrib);
        }

        /**
         * Deletes the key/value pair with key {@code key} from this {@code
         * Attributes}.
         *
         * @param key
         *            the key to remove.
         * @return the values associated with the removed key, {@code null} if not
         *         present.
         */
        public Object remove(Object key) {
            return map.remove(key);
        }

        /**
         * Returns the number of key/value pairs associated with this {@code
         * Attributes}.
         *
         * @return the size of this {@code Attributes}.
         */
        public int size() {
            return map.size();
        }

        /**
         * Returns a collection of all the values present in this {@code
         * Attributes}.
         *
         * @return a collection of all values present.
         */
        public Collection<Object> values() {
            return map.values();
        }

        public Object clone() {
            Attributes clone;
            try {
                clone = (Attributes) this.MemberwiseClone();
            } catch (java.lang.CloneNotSupportedException e) {
                return null;
            }
            clone.map = (java.util.Map<Object, Object>) ((java.util.HashMap<Object,Object>) map).clone();
            return clone;
        }

        /**
         * Returns the hash code of this {@code Attributes}.
         *
         * @return the hash code of this object.
         */
        public override int GetHashCode() {
            return map.GetHashCode();
        }

        /**
         * Determines if this {@code Attributes} and the parameter {@code
         * Attributes} are equal. Two {@code Attributes} instances are equal if they
         * contain the same keys and values.
         *
         * @param obj
         *            the object with which this {@code Attributes} is compared.
         * @return {@code true} if the {@code Attributes} are equal, {@code false}
         *         otherwise.
         */
        public override bool Equals(Object obj) {
            if (this == obj) {
                return true;
            }
            if (obj is Attributes) {
                return map.equals(((Attributes) obj).map);
            }
            return false;
        }

        /**
         * Returns the value associated with the parameter {@code Attributes.Name}
         * key.
         *
         * @param name
         *            the key to obtain the value for.
         * @return the {@code String} associated with name, or {@code null} if name
         *         is not a valid key.
         */
        public String getValue(AttributesNS.Name name) {
            return (String) map.get(name);
        }

        /**
         * Returns the string associated with the parameter name.
         *
         * @param name
         *            the key to obtain the value for.
         * @return the string associated with name, or {@code null} if name is not a
         *         valid key.
         */
        public String getValue(String name) {
            return (String) map.get(new AttributesNS.Name(name));
        }

        /**
         * Stores the value {@code val} associated with the key {@code name} in this
         * {@code Attributes}.
         *
         * @param name
         *            the key to store.
         * @param val
         *            the value to store in this {@code Attributes}.
         * @return the value being stored.
         */
        public String putValue(String name, String val) {
            return (String) map.put(new AttributesNS.Name(name), val);
        }
    }
    #endregion

    namespace AttributesNS {
        /**
            * The name part of the name/value pairs constituting an attribute as
            * defined by the specification of the JAR manifest. May be composed of the
            * following ASCII signs as defined in the EBNF below:
            *
            * <pre>
            * name       = alphanum *headerchar
            * headerchar = alphanum | - | _
            * alphanum   = {A-Z} | {a-z} | {0-9}
            * </pre>
            */
        public class Name {
            private readonly byte[] name;

            private int hashCode;

            /**
                * The class path (a main attribute).
                */
            public static readonly Name CLASS_PATH = new Name("Class-Path"); //$NON-NLS-1$

            /**
                * The version of the manifest file (a main attribute).
                */
            public static readonly Name MANIFEST_VERSION = new Name("Manifest-Version"); //$NON-NLS-1$

            /**
                * The main class's name (for stand-alone applications).
                */
            public static readonly Name MAIN_CLASS = new Name("Main-Class"); //$NON-NLS-1$

            /**
                * Defines the signature version of the JAR file.
                */
            public static readonly Name SIGNATURE_VERSION = new Name(
                    "Signature-Version"); //$NON-NLS-1$

            /**
                * The {@code Content-Type} manifest attribute.
                */
            public static readonly Name CONTENT_TYPE = new Name("Content-Type"); //$NON-NLS-1$

            /**
                * The {@code Sealed} manifest attribute which may have the value
                * {@code true} for sealed archives.
                */
            public static readonly Name SEALED = new Name("Sealed"); //$NON-NLS-1$

            /**
                * The {@code Implementation-Title} attribute whose value is a string
                * that defines the title of the extension implementation.
                */
            public static readonly Name IMPLEMENTATION_TITLE = new Name(
                    "Implementation-Title"); //$NON-NLS-1$

            /**
                * The {@code Implementation-Version} attribute defining the version of
                * the extension implementation.
                */
            public static readonly Name IMPLEMENTATION_VERSION = new Name(
                    "Implementation-Version"); //$NON-NLS-1$

            /**
                * The {@code Implementation-Vendor} attribute defining the organization
                * that maintains the extension implementation.
                */
            public static readonly Name IMPLEMENTATION_VENDOR = new Name(
                    "Implementation-Vendor"); //$NON-NLS-1$

            /**
                * The {@code Specification-Title} attribute defining the title of the
                * extension specification.
                */
            public static readonly Name SPECIFICATION_TITLE = new Name(
                    "Specification-Title"); //$NON-NLS-1$

            /**
                * The {@code Specification-Version} attribute defining the version of
                * the extension specification.
                */
            public static readonly Name SPECIFICATION_VERSION = new Name(
                    "Specification-Version"); //$NON-NLS-1$

            /**
                * The {@code Specification-Vendor} attribute defining the organization
                * that maintains the extension specification.
                */
            public static readonly Name SPECIFICATION_VENDOR = new Name(
                    "Specification-Vendor"); //$NON-NLS-1$

            /**
                * The {@code Extension-List} attribute defining the extensions that are
                * needed by the applet.
                */
            public static readonly Name EXTENSION_LIST = new Name("Extension-List"); //$NON-NLS-1$

            /**
                * The {@code Extension-Name} attribute which defines the unique name of
                * the extension.
                */
            public static readonly Name EXTENSION_NAME = new Name("Extension-Name"); //$NON-NLS-1$

            /**
                * The {@code Extension-Installation} attribute.
                */
            public static readonly Name EXTENSION_INSTALLATION = new Name(
                    "Extension-Installation"); //$NON-NLS-1$

            /**
                * The {@code Implementation-Vendor-Id} attribute specifies the vendor
                * of an extension implementation if the applet requires an
                * implementation from a specific vendor.
                */
            public static readonly Name IMPLEMENTATION_VENDOR_ID = new Name(
                    "Implementation-Vendor-Id"); //$NON-NLS-1$

            /**
                * The {@code Implementation-URL} attribute specifying a URL that can be
                * used to obtain the most recent version of the extension if the
                * required version is not already installed.
                */
            public static readonly Name IMPLEMENTATION_URL = new Name(
                    "Implementation-URL"); //$NON-NLS-1$

            protected internal static readonly Name NAME = new Name("Name"); //$NON-NLS-1$

            /**
                * A String which must satisfy the following EBNF grammar to specify an
                * additional attribute:
                *
                * <pre>
                * name       = alphanum *headerchar
                * headerchar = alphanum | - | _
                * alphanum   = {A-Z} | {a-z} | {0-9}
                * </pre>
                *
                * @param s
                *            The Attribute string.
                * @exception IllegalArgumentException
                *                if the string does not satisfy the EBNF grammar.
                */
            public Name(String s) {
                int i = s.Length;
                if (i == 0 || i > Manifest.LINE_LENGTH_LIMIT - 2) {
                    throw new java.lang.IllegalArgumentException();
                }

                name = new byte[i];

                for (; --i >= 0;) {
                    char ch = s.charAt(i);
                    if (!((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                            || ch == '_' || ch == '-' || (ch >= '0' && ch <= '9'))) {
                        throw new java.lang.IllegalArgumentException(s);
                    }
                    name[i] = (byte) ch;
                }
            }

            /**
                * A private constructor for a trusted attribute name.
                */
            protected internal Name(byte[] buf) {
                name = buf;
            }

            protected internal byte[] getBytes() {
                return name;
            }

            /**
                * Returns this attribute name.
                *
                * @return the attribute name.
                */
        
            public override String ToString() {
                try {
                    return new java.lang.StringJ(name, "ISO-8859-1").ToString(); //$NON-NLS-1$
                } catch (java.io.UnsupportedEncodingException iee) {
                    throw new java.lang.InternalError(iee.getLocalizedMessage());
                }
            }

            /**
                * Returns whether the argument provided is the same as the attribute
                * name.
                *
                * @return if the attribute names correspond.
                * @param object
                *            An attribute name to be compared with this name.
                */
        
            public override bool Equals(Object obj) {
                if (obj == null || obj.GetType() != this.GetType()
                        || obj.GetHashCode() != this.GetHashCode()) {
                    return false;
                }

                return Util.asciiEqualsIgnoreCase(name, ((Name) obj).name);
            }
            /**
             * Computes a hash code of the name.
             *
             * @return the hash value computed from the name.
             */
            
            public override int GetHashCode() {
                if (hashCode == 0) {
                    int hash = 0, multiplier = 1;
                    for (int i = name.Length - 1; i >= 0; i--) {
                        // 'A' & 0xDF == 'a' & 0xDF, ..., 'Z' & 0xDF == 'z' & 0xDF
                        hash += (name[i] & 0xDF) * multiplier;
                        int shifted = multiplier << 5;
                        multiplier = shifted - multiplier;
                    }
                    hashCode = hash;
                }
                return hashCode;
            }

        }
    }
}
