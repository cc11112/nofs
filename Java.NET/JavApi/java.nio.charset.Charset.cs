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

namespace biz.ritter.javapi.nio.charset
{
/*
    import java.io.BufferedReader;
    import java.io.IOException;
    import java.io.InputStream;
    import java.io.InputStreamReader;
    import java.net.URL;
    import java.nio.ByteBuffer;
    import java.nio.CharBuffer;
    import java.nio.charset.spi.CharsetProvider;
    import java.security.AccessController;
    import java.security.PrivilegedAction;
    import java.util.Collections;
    import java.util.Comparator;
    import java.util.Enumeration;
    import java.util.HashMap;
    import java.util.HashSet;
    import java.util.Iterator;
    import java.util.Locale;
    import java.util.Set;
    import java.util.SortedMap;
    import java.util.TreeMap;
    import java.util.Vector;

    import org.apache.harmony.niochar.CharsetProviderImpl;
*/
    /**
     * A charset defines a mapping between a Unicode character sequence and a byte
     * sequence. It facilitates the encoding from a Unicode character sequence into
     * a byte sequence, and the decoding from a byte sequence into a Unicode
     * character sequence.
     * <p>
     * A charset has a canonical name, which is usually in uppercase. Typically it
     * also has one or more aliases. The name string can only consist of the
     * following characters: '0' - '9', 'A' - 'Z', 'a' - 'z', '.', ':'. '-' and '_'.
     * The first character of the name must be a digit or a letter.
     * <p>
     * The following charsets should be supported by any java platform: US-ASCII,
     * ISO-8859-1, UTF-8, UTF-16BE, UTF-16LE, UTF-16.
     * <p>
     * Additional charsets can be made available by configuring one or more charset
     * providers through provider configuration files. Such files are always named
     * as "java.nio.charset.spi.CharsetProvider" and located in the
     * "META-INF/services" sub folder of one or more classpaths. The files should be
     * encoded in "UTF-8". Each line of their content specifies the class name of a
     * charset provider which extends
     * <code>java.nio.charset.spi.CharsetProvider</code>. A line should end with
     * '\r', '\n' or '\r\n'. Leading and trailing whitespaces are trimmed. Blank
     * lines, and lines (after trimming) starting with "#" which are regarded as
     * comments, are both ignored. Duplicates of names already found are also
     * ignored. Both the configuration files and the provider classes will be loaded
     * using the thread context class loader.
     * <p>
     * This class is thread-safe.
     *
     * @see java.nio.charset.spi.CharsetProvider
     */
    public abstract class Charset : java.lang.Comparable<Charset> {

        /*
         * The name of configuration files where charset provider class names can be
         * specified.
         */
        private static readonly String PROVIDER_CONFIGURATION_FILE_NAME = "META-INF/services/java.nio.charset.spi.CharsetProvider"; //$NON-NLS-1$

        /*
         * The encoding of configuration files
         */
        private static readonly String PROVIDER_CONFIGURATION_FILE_ENCODING = "UTF-8"; //$NON-NLS-1$

        /*
         * The comment string used in configuration files
         */
        private static readonly String PROVIDER_CONFIGURATION_FILE_COMMENT = "#"; //$NON-NLS-1$

        //private static ClassLoader systemClassLoader;

        // built in provider instance, assuming thread-safe
        private static java.nio.charset.spi.CharsetProvider builtInProvider = null;

        private readonly String canonicalName;

        // the aliases set
        private readonly java.util.HashSet<String> aliasesSet;

        private static bool inForNameInternal = false;

        /**
         * Constructs a <code>Charset</code> object. Duplicated aliases are
         * ignored.
         * 
         * @param canonicalName
         *            the canonical name of the charset.
         * @param aliases
         *            an array containing all aliases of the charset. May be null.
         * @throws IllegalCharsetNameException
         *             on an illegal value being supplied for either
         *             <code>canonicalName</code> or for any element of
         *             <code>aliases</code>.
         */
        protected Charset(String canonicalName, String[] aliases) {
            if (null == canonicalName) {
                throw new java.lang.NullPointerException();
            }
            // check whether the given canonical name is legal
            checkCharsetName(canonicalName);
            this.canonicalName = canonicalName;
            // check each alias and put into a set
            this.aliasesSet = new java.util.HashSet<String>();
            if (null != aliases) {
                for (int i = 0; i < aliases.Length; i++) {
                    checkCharsetName(aliases[i]);
                    this.aliasesSet.add(aliases[i]);
                }
            }
        }

        /*
         * Checks whether a character is a special character that can be used in
         * charset names, other than letters and digits.
         */
        private static bool isSpecial(char c) {
            return ('-' == c || '.' == c || ':' == c || '_' == c);
        }

        /*
         * Checks whether a character is a letter (ascii) which are defined in the
         * spec.
         */
        private static bool isLetter(char c) {
            return ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');
        }

        /*
         * Checks whether a character is a digit (ascii) which are defined in the
         * spec.
         */
        private static bool isDigit(char c) {
            return ('0' <= c && c <= '9');
        }

        /*
         * Checks whether a given string is a legal charset name. The argument name
         * should not be null.
         */
        private static void checkCharsetName(String name) {
            // An empty string is illegal charset name
            if (name.length() == 0) {
                throw new IllegalCharsetNameException(name);
            }
            // The first character must be a letter or a digit
            // This is related to HARMONY-68 (won't fix)
            // char first = name.charAt(0);
            // if (!isLetter(first) && !isDigit(first)) {
            // throw new IllegalCharsetNameException(name);
            // }
            // Check the remaining characters
            int length = name.length();
            for (int i = 0; i < length; i++) {
                char c = name.charAt(i);
                if (!isLetter(c) && !isDigit(c) && !isSpecial(c)) {
                    throw new IllegalCharsetNameException(name);
                }
            }
        }


        /**
         * Gets a map of all available charsets supported by the runtime.
         * <p>
         * The returned map contains mappings from canonical names to corresponding
         * instances of <code>Charset</code>. The canonical names can be considered
         * as case-insensitive.
         *
         * @return an unmodifiable map of all available charsets supported by the
         *         runtime
         */
        public static java.util.SortedMap<String, Charset> availableCharsets() {
            java.util.Iterator<Charset> it = Charset.builtInProvider.charsets();
            java.util.SortedMap<String,Charset> sm = new java.util.TreeMap<String,Charset>();
            while (it.hasNext())
            {
                Charset next = it.next();
                sm.put(next.name(), next);
                java.util.Iterator<String> alias = next.aliases().iterator();
                while (alias.hasNext())
                {
                    sm.put(alias.next(), next);
                }
            } 
            return java.util.Collections.unmodifiableSortedMap(sm);
        }


        /**
         * Gets a <code>Charset</code> instance for the specified charset name.
         * 
         * @param charsetName
         *            the canonical name of the charset or an alias.
         * @return a <code>Charset</code> instance for the specified charset name.
         * @throws IllegalCharsetNameException
         *             if the specified charset name is illegal.
         * @throws UnsupportedCharsetException
         *             if the desired charset is not supported by this runtime.
         */
        public static Charset forName(String charsetName) {
            Charset cs = getCharsetFromBuiltInProvider(charsetName);
            if (null == cs) throw new UnsupportedCharsetException(charsetName);
            return cs;
        }

        /**
         * Determines whether the specified charset is supported by this runtime.
         * 
         * @param charsetName
         *            the name of the charset.
         * @return true if the specified charset is supported, otherwise false.
         * @throws IllegalCharsetNameException
         *             if the specified charset name is illegal.
         */
        public static bool isSupported(String charsetName) {
            lock (builtInProvider) {
                if (getCharsetFromBuiltInProvider(charsetName) != null) {
                    return true;
                } else {
                    throw new java.lang.UnsupportedOperationException ("Not yet implemented");
                    //TODO: META-INF/services/java.nio.charset.spi.CharsetProvider
                }
            }
        }

        /// <summary>
        /// Check charset name for build in implementation
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        protected static Charset getCharsetFromBuiltInProvider (String charsetName) {
            checkCharsetName(charsetName);
            if (null == charsetName) {
                throw new java.lang.IllegalArgumentException();
            }
            if (null == builtInProvider) {
                builtInProvider = new dotnet.nio.charset.CharsetProviderImpl();
            }
            Charset cs = builtInProvider.charsetForName(charsetName);
            return cs;
        }

        /**
         * Determines whether this charset is a super set of the given charset.
         * 
         * @param charset
         *            a given charset.
         * @return true if this charset is a super set of the given charset,
         *         false if it's unknown or this charset is not a superset of
         *         the given charset.
         */
        public abstract bool contains(Charset charset);

        /**
         * Gets a new instance of an encoder for this charset.
         * 
         * @return a new instance of an encoder for this charset.
         */
        public abstract CharsetEncoder newEncoder();

        /**
         * Gets a new instance of a decoder for this charset.
         * 
         * @return a new instance of a decoder for this charset.
         */
        public abstract CharsetDecoder newDecoder();

        /**
         * Gets the canonical name of this charset.
         * 
         * @return this charset's name in canonical form.
         */
        public String name() {
            return this.canonicalName;
        }

        /**
         * Gets the set of this charset's aliases.
         * 
         * @return an unmodifiable set of this charset's aliases.
         */
        public java.util.Set<String> aliases() {
            return java.util.Collections.unmodifiableSet(this.aliasesSet);
        }

        /**
         * Gets the name of this charset for the default locale.
         * 
         * <p>The default implementation returns the canonical name of this charset.
         * Subclasses may return a localized display name.
         *
         * @return the name of this charset for the default locale.
         */
        public virtual String displayName() {
            return this.canonicalName;
        }

        /**
         * Gets the name of this charset for the specified locale.
         *
         * <p>The default implementation returns the canonical name of this charset.
         * Subclasses may return a localized display name.
         *
         * @param l
         *            a certain locale
         * @return the name of this charset for the specified locale
         */
        public virtual String displayName(java.util.Locale l) {
            return this.canonicalName;
        }

        /**
         * Indicates whether this charset is known to be registered in the IANA
         * Charset Registry.
         * 
         * @return true if the charset is known to be registered, otherwise returns
         *         false.
         */
        public bool isRegistered() {
            return !canonicalName.startsWith("x-") //$NON-NLS-1$
                    && !canonicalName.startsWith("X-"); //$NON-NLS-1$
        }

        /**
         * Returns true if this charset supports encoding, false otherwise.
         * 
         * @return true if this charset supports encoding, false otherwise.
         */
        public virtual bool canEncode() {
            return true;
        }

        /// <summary>
        /// <para>
        /// Encodes the content of the give character buffer and outputs to a byte
        /// buffer that is to be returned.
        /// </para>
        /// <para>The default action in case of encoding errors is <c>CodingErrorAction.REPLACE</c>.</para>
        /// </summary>
        /// <param name="buffer">the character buffer containing the content to be encoded.</param>
        /// <returns>the result of the encoding.</returns>
        public java.nio.ByteBuffer encode(java.nio.CharBuffer buffer) {
            try {
                return this.newEncoder()
                        .onMalformedInput(CodingErrorAction.REPLACE)
                        .onUnmappableCharacter(CodingErrorAction.REPLACE).encode(
                                buffer);

            } catch (CharacterCodingException ex) {
                throw new java.lang.Error(ex.getMessage(), ex);
            }
        }

        /// <summary>
        /// <para>Encodes a string and outputs to a byte buffer that is to be returned.</para>
        /// <para>The default action in case of encoding errors is <c>CodingErrorAction.REPLACE</c>.</para>
        /// </summary>
        /// <param name="s">the string to be encoded.</param>
        /// <returns>the result of the encoding.</returns>
        public java.nio.ByteBuffer encode(String s) {
            return encode(java.nio.CharBuffer.wrap(s.getWrapperInstance()));
        }

        /**
         * Decodes the content of the specified byte buffer and writes it to a
         * character buffer that is to be returned.
         * <p>
         * The default action in case of decoding errors is
         * <code>CodingErrorAction.REPLACE</code>.
         * 
         * @param buffer
         *            the byte buffer containing the content to be decoded.
         * @return a character buffer containing the output of the decoding.
         */
        public CharBuffer decode(ByteBuffer buffer) {

            try {
                return this.newDecoder()
                        .onMalformedInput(CodingErrorAction.REPLACE)
                        .onUnmappableCharacter(CodingErrorAction.REPLACE).decode(
                                buffer);

            } catch (CharacterCodingException ex) {
                throw new java.lang.Error(ex.getMessage(), ex);
            }
        }

        /*
         * -------------------------------------------------------------------
         * Methods implementing parent interface Comparable
         * -------------------------------------------------------------------
         */

        /**
         * Compares this charset with the given charset. This comparation is
         * based on the case insensitive canonical names of the charsets.
         * 
         * @param charset
         *            the given object to be compared with.
         * @return a negative integer if less than the given object, a positive
         *         integer if larger than it, or 0 if equal to it.
         */
        public int compareTo(Charset charset) {
            return this.canonicalName.CompareTo(charset.canonicalName);
        }

        /*
         * -------------------------------------------------------------------
         * Methods overriding parent class Object
         * -------------------------------------------------------------------
         */

        /**
         * Determines whether this charset equals to the given object. They are
         * considered to be equal if they have the same canonical name.
         * 
         * @param obj
         *            the given object to be compared with.
         * @return true if they have the same canonical name, otherwise false.
         */
        
        public override bool Equals(Object obj) {
            if (obj is Charset) {
                Charset that = (Charset) obj;
                return this.canonicalName.equals(that.canonicalName);
            }
            return false;
        }

        /**
         * Gets the hash code of this charset.
         * 
         * @return the hash code of this charset.
         */
        
        public override int GetHashCode() {
            return this.canonicalName.GetHashCode();
        }

        /**
         * Gets a string representation of this charset. Usually this contains the
         * canonical name of the charset.
         * 
         * @return a string representation of this charset.
         */
        
        public override String ToString() {
            return "Charset[" + this.canonicalName + "]"; //$NON-NLS-1$//$NON-NLS-2$
        }

        /**
         * Gets the system default charset from the virtual machine.
         * 
         * @return the default charset.
         */
        public static Charset defaultCharset() {
            Charset defaultCharset = null;
            String encoding = java.lang.SystemJ.getProperty("file.encoding"); //$NON-NLS-1$
            try {
                defaultCharset = Charset.forName(encoding);
            } catch (UnsupportedCharsetException e) {
                defaultCharset = Charset.forName("UTF-8"); //$NON-NLS-1$
            }
            return defaultCharset;
        }

    }
#region IgnoreCaseComparator
    /**
        * A comparator that ignores case.
        */
    internal class IgnoreCaseComparator<E> : java.util.Comparator<E> {

        // the singleton
        private static java.util.Comparator<String> c = new IgnoreCaseComparator<String>();

        /*
            * Default constructor.
            */
        private IgnoreCaseComparator() {
            // no action
        }

        /*
         * Gets a single instance.
         */
        public static java.util.Comparator<String> getInstance() {
            return c;
        }

        /*
         * Compares two strings ignoring case.
         */
        public int compare(E s1, E s2) {
            // null testing
            if (s1 == null) return -1;
            else if (s2 == null) return +1;
            // Java types convert
            String str1 = s1.ToString();
            String str2 = s2.ToString();
            return str1.CompareTo(str2);
        }

        public bool equals(Object obj)
        {
            if (obj is IgnoreCaseComparator<String>) return true;
            return false;
        }
    }
#endregion
}
