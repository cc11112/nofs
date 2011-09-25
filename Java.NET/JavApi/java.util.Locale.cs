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
using System.Text;
using System.Globalization;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{

    /// <summary>
    /// <code>Locale</code> represents a language/countrycombination. It is an identifier
    /// which dictates particular conventions for the presentation of information.
    /// The language codes are two letter lowercase codes as defined by ISO-639. The
    /// country codes are three letter uppercase codes as defined by ISO-3166.
    /// </summary>
    [Serializable]
    public sealed class Locale : java.lang.Cloneable, java.io.Serializable {

        private static readonly long serialVersionUID = 9149081749638150636L;

        internal CultureInfo delegateInstance = CultureInfo.CurrentUICulture;

        private static Locale defaultLocale = new Locale();

        /**
         * Locale instance for en_CA.
         */
        public static readonly Locale CANADA = new Locale("en", "CA");

        /**
         * Locale instance for fr_CA.
         */
        public static readonly Locale CANADA_FRENCH = new Locale("fr", "CA");

        /**
         * Locale instance for zh_CN.
         */
        public static readonly Locale CHINA = new Locale("zh", "CN");

        /**
         * Locale instance for zh.
         */
        public static readonly Locale CHINESE = new Locale("zh");

        /**
         * Locale instance for en.
         */
        public static readonly Locale ENGLISH = new Locale("en");

        /**
         * Locale instance for fr_FR.
         */
        public static readonly Locale FRANCE = new Locale("fr", "FR");

        /**
         * Locale instance for fr.
         */
        public static readonly Locale FRENCH = new Locale("fr");

        /**
         * Locale instance for de.
         */
        public static readonly Locale GERMAN = new Locale("de");

        /**
         * Locale instance for de_DE.
         */
        public static readonly Locale GERMANY = new Locale("de", "DE");

        /**
         * Locale instance for it.
         */
        public static readonly Locale ITALIAN = new Locale("it");

        /**
         * Locale instance for it_IT.
         */
        public static readonly Locale ITALY = new Locale("it", "IT");

        /**
         * Locale instance for ja_JP.
         */
        public static readonly Locale JAPAN = new Locale("ja", "JP");

        /**
         * Locale instance for ja.
         */
        public static readonly Locale JAPANESE = new Locale("ja");

        /**
         * Locale instance for ko_KR.
         */
        public static readonly Locale KOREA = new Locale("ko", "KR");

        /**
         * Locale instance for ko.
         */
        public static readonly Locale KOREAN = new Locale("ko");

        /**
         * Locale instance for zh_CN.
         */
        public static readonly Locale PRC = new Locale("zh", "CN");

        /**
         * Locale instance for zh_CN.
         */
        public static readonly Locale SIMPLIFIED_CHINESE = new Locale("zh", "CN");

        /**
         * Locale instance for zh_TW.
         */
        public static readonly Locale TAIWAN = new Locale("zh", "TW");

        /**
         * Locale instance for zh_TW.
         */
        public static readonly Locale TRADITIONAL_CHINESE = new Locale("zh", "TW");

        /**
         * Locale instance for en_GB.
         */
        public static readonly Locale UK = new Locale("en", "GB");

        /**
         * Locale instance for en_US.
         */
        public static readonly Locale US = new Locale("en", "US");

        public static readonly Locale ROOT = new Locale("");

        /**
         * Constructs a default.
         */
        internal Locale() {
            this.delegateInstance = CultureInfo.CurrentCulture; 
        }

        /**
         * Constructs a new {@code Locale} using the specified language.
         * 
         * @param language
         *            the language this {@code Locale} represents.
         */
        public Locale(String language) {
            this.newDelegateInstance(language, null);
        }

        ///<summary>
        /// Create a new Locale instance with language and country
        ///</summary>
        ///<param name="country">ISO 3166 two char country code</param>
        ///<param name="language">ISO 639 two char language code</param>
        public Locale(String language, String country) {
            this.newDelegateInstance (language, country);
        }

        private void newDelegateInstance (String language, String country) {
            if (language == null || language.trim().length() == 0)
            {
                this.delegateInstance = CultureInfo.InvariantCulture;
            }
            else
            {
                this.delegateInstance = new CultureInfo(language + (null == country ? "" : "-" + country));
            }
        }

        public Locale(String language, String country, String variant) {
            //FIXME : Variant not supported - create Loginformations
            this.newDelegateInstance (language, country);
        }

        public override bool Equals(Object o) {
            return this.delegateInstance.Equals (o);
        }

        /**
         * Gets all list of installed {@code Locale}. At least a {@code Locale} that is equal to
         * {@code Locale.US} must be contained in this array.
         *
         * @return an array of {@code Locale}s.
         */
        public static Locale[] getAvailableLocales() {
            CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures);
            Locale[] retValue = new Locale[ci.Length];
            Locale[] retValueWithoutUS = new Locale[ci.Length+1];
            bool isUSAdded = false;
            for (int i = 0; i < ci.Length; i++)
            {
                retValue[i] = new Locale();
                retValue[i].delegateInstance = ci[i];
                retValueWithoutUS[i] = retValue [i];
                if ("US".Equals(retValue[i].getCountry())) isUSAdded = true;
            }
            if (!isUSAdded)
            {
                retValueWithoutUS[retValueWithoutUS.Length - 1] = Locale.US;
                retValue = retValueWithoutUS;
            }
            return retValue;
        }

        /**
         * Gets the country code for this {@code Locale} or an empty string of no country
         * was set.
         * 
         * @return a country code.
         */
        public String getCountry() {
            return this.delegateInstance.ToString().Substring(3);
        }

        /**
         * Gets the default {@code Locale}.
         * 
         * @return the default {@code Locale}.
         */
        public static Locale getDefault() {
            return defaultLocale;
        }

        /**
         * Gets the full country name in the default {@code Locale} for the country code of
         * this {@code Locale}. If there is no matching country name, the country code is
         * returned.
         * 
         * @return a country name.
         */
        public String getDisplayCountry() {
            return this.delegateInstance.DisplayName.Split (new char[]{' '})[1];
        }

        /**
         * Gets the full country name in the specified {@code Locale} for the country code
         * of this {@code Locale}. If there is no matching country name, the country code is
         * returned.
         *
         * @param locale
         *            the {@code Locale} for which the display name is retrieved.
         * @return a country name.
         */
        public String getDisplayCountry(Locale locale) {
            return locale.getDisplayCountry();
        }

        /**
         * Gets the full language name in the default {@code Locale} for the language code
         * of this {@code Locale}. If there is no matching language name, the language code
         * is returned.
         * 
         * @return a language name.
         */
        public String getDisplayLanguage() {
            return this.delegateInstance.DisplayName.Split(new char[]{' '})[0];
        }

        /**
         * Gets the full language name in the specified {@code Locale} for the language code
         * of this {@code Locale}. If there is no matching language name, the language code
         * is returned.
         *
         * @param locale
         *            the {@code Locale} for which the display name is retrieved.
         * @return a language name.
         */
        public String getDisplayLanguage(Locale locale) {
            return locale.getDisplayLanguage();
        }

        /**
         * Gets the full language, country, and variant names in the default {@code Locale}
         * for the codes of this {@code Locale}.
         * 
         * @return a {@code Locale} name.
         */
        public String getDisplayName() {
            return this.delegateInstance.DisplayName;
        }

        /**
         * Gets the full language, country, and variant names in the specified
         * Locale for the codes of this {@code Locale}.
         * 
         * @param locale
         *            the {@code Locale} for which the display name is retrieved.
         * @return a {@code Locale} name.
         */
        public String getDisplayName(Locale locale) {
            return locale.getDisplayName();
        }

        /**
         * Gets the three letter ISO language code which corresponds to the language
         * code for this {@code Locale}.
         *
         * @return a three letter ISO language code.
         * @throws MissingResourceException
         *                if there is no matching three letter ISO language code.
         */
        public String getISO3Language() {// throws MissingResourceException {
            return this.delegateInstance.ThreeLetterISOLanguageName;
        }

        /**
         * Gets the language code for this {@code Locale} or the empty string of no language
         * was set.
         * 
         * @return a language code.
         */
        public String getLanguage() {
            return this.delegateInstance.TwoLetterISOLanguageName;
        }

        /**
         * Returns an integer hash code for the receiver. Objects which are equal
         * return the same value for this method.
         * 
         * @return the receiver's hash.
         * @see #equals
         */
        public override int GetHashCode() {
            lock (this.delegateInstance)
            {
                return this.getCountry().GetHashCode() + this.getLanguage().GetHashCode();
            }
        }

        public static void setDefault(Locale locale) {
            if (locale != null) {
                defaultLocale = locale;
                defaultLocale.delegateInstance = CultureInfo.CurrentCulture;
            } else {
                throw new java.lang.NullPointerException();
            }
        }

        /**
         * Returns the string representation of this {@code Locale}. 
         * @return the string representation of this {@code Locale}.
         */
        public override String ToString() {
            StringBuilder result = new StringBuilder();
            result.Append(this.getLanguage());
            if (this.getCountry().length() > 0) {
                result.Append('_');
                result.Append(this.getCountry());
            }
            return result.ToString();
        }

        public Object clone() {
            Locale l = new Locale ();
            l.delegateInstance = (CultureInfo)this.delegateInstance.Clone();
            return l;
        }

        internal System.Globalization.Calendar getCalendar()
        {
            return this.delegateInstance.Calendar;
        }
    }
}
