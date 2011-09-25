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
using System.Globalization;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    /**
     * This class represents a currency as identified in the ISO 4217 currency
     * codes. 
     */
    public sealed class Currency : java.io.Serializable {

        private static readonly long serialVersionUID = -158308464356906721L;

        private RegionInfo region;

        /**
         * @param currencyCode
         */
        private Currency(RegionInfo region) {
            this.region = region;
        }

        /**
         * Returns the {@code Currency} instance for this currency code.
         * <p>
         * 
         * @param currencyCode
         *            the currency code.
         * @return the {@code Currency} instance for this currency code.
         * 
         * @throws IllegalArgumentException
         *             if the currency code is not a supported ISO 4217 currency
         *             code.
         */
        public static Currency getInstance(String currencyCode) {
            foreach (Locale l in Locale.getAvailableLocales())
            {
                RegionInfo rc = new RegionInfo(l.getCountry());
                if (currencyCode.equals(rc.ISOCurrencySymbol))
                {
                    return new Currency(rc);
                }
            }
            throw new java.lang.IllegalArgumentException("currency code is not a supported ISO 4217 currency code");
        }

        /**
         * Returns the {@code Currency} instance for this {@code Locale}'s country.
         * 
         * @param locale
         *            the {@code Locale} of a country.
         * @return the {@code Currency} used in the country defined by the locale parameter.
         * 
         * @throws IllegalArgumentException
         *             if the locale's country is not a supported ISO 3166 Country.
         */
        public static Currency getInstance(Locale locale) {
            try
            {
                if (null == locale) throw new java.lang.NullPointerException();
                System.Globalization.RegionInfo ri = new System.Globalization.RegionInfo(locale.getCountry());
                String currencyCode = ri.ISOCurrencySymbol;

                return getInstance(currencyCode);
            }
            catch (ArgumentException ae)
            {
                throw new java.lang.IllegalArgumentException(ae.Message);
            }
        }

        /**
         * Returns this {@code Currency}'s ISO 4217 currency code.
         * 
         * @return this {@code Currency}'s ISO 4217 currency code.
         */
        public String getCurrencyCode() {
            return region.ISOCurrencySymbol;
        }
    
        /**
         * Returns the symbol for this currency in the default locale. For instance,
         * if the default locale is the US, the symbol of the US dollar is "$". For
         * other locales it may be "US$". If no symbol can be determined, the ISO
         * 4217 currency code of the US dollar is returned.
         * 
         * @return the symbol for this {@code Currency} in the default {@code Locale}.
         */
        public String getSymbol() {
            return getSymbol(Locale.getDefault());
        }

        /**
         * Returns the symbol for this currency in the given {@code Locale}.
         * <p>
         * If the locale doesn't have any countries (e.g.
         * {@code Locale.JAPANESE, new Locale("en","")}), then the ISO
         * 4217 currency code is returned.
         * <p>
         * First the locale's resource bundle is checked, if the locale has the same currency,
         * the CurrencySymbol in this locale bundle is returned.
         * <p>
         * Then a currency bundle for this locale is searched.
         * <p>
         * If a currency bundle for this locale does not exist, or there is no
         * symbol for this currency in this bundle, then the
         * ISO 4217 currency code is returned.
         * <p>
         * 
         * @param locale
         *            the locale for which the currency symbol should be returned.
         * @return the representation of this {@code Currency}'s symbol in the specified
         *         locale.
         */
        public String getSymbol(Locale locale) {
            if (null == locale) throw new java.lang.NullPointerException();
            if ("".equals(locale.getCountry())) { //$NON-NLS-1$
                return this.region.ISOCurrencySymbol;
            }
            return locale.delegateInstance.NumberFormat.CurrencySymbol;
        }

        /**
         * Returns the default number of fraction digits for this currency. For
         * instance, the default number of fraction digits for the US dollar is 2.
         * For the Japanese Yen the number is 0. In the case of pseudo-currencies,
         * such as IMF Special Drawing Rights, -1 is returned.
         * 
         * @return the default number of fraction digits for this currency.
         */
        public int getDefaultFractionDigits() {
            Locale l = new Locale(this.region.Name);
            return l.delegateInstance.NumberFormat.NumberDecimalDigits;
        }

        /**
         * Returns ISO 4217 currency code.
         * 
         * @return ISO 4217 currency code.
         */
        
        
        public override String ToString() {
            return region.ISOCurrencySymbol;
        }

        private Object readResolve() {
            return getInstance(this.region.ISOCurrencySymbol);
        }
    }
}
