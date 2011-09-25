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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.text
{
    public class NumberFormat
    {

        public static NumberFormat getInstance()
        {
            return new NumberFormat();
        }

        public java.lang.Number parse(String text)
        {
            Decimal bigDecimal;
            Decimal.TryParse(text, out bigDecimal);
            if (Decimal.Compare(bigDecimal, new Decimal(java.lang.Double.MAX_VALUE)) > 0 ||
                Decimal.Compare(bigDecimal, new Decimal(java.lang.Double.MIN_VALUE)) < 0)
                throw new ParseException("Value outside java.lang.Double range - sorry.", -1);
            else
            {
                return new java.lang.Double(Decimal.ToDouble(bigDecimal));
            }
        }
    }
}
