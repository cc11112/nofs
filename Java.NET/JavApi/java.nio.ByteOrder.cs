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

namespace biz.ritter.javapi.nio
{


    /**
     * Defines byte order constants.
     */
    /// <remarks>Class is ported from Apache Harmony project.</remarks>
    public sealed class ByteOrder
    {

        /**
         * This constant represents big endian.
         */
        public static readonly ByteOrder BIG_ENDIAN = new ByteOrder("BIG_ENDIAN"); //$NON-NLS-1$

        /**
         * This constant represents little endian.
         */
        public static readonly ByteOrder LITTLE_ENDIAN = new ByteOrder("LITTLE_ENDIAN"); //$NON-NLS-1$

        private static readonly ByteOrder NATIVE_ORDER = BitConverter.IsLittleEndian ? LITTLE_ENDIAN : BIG_ENDIAN;

        

        /**
         * Returns the current platform byte order.
         * 
         * @return the byte order object, which is either LITTLE_ENDIAN or
         *         BIG_ENDIAN.
         */
        public static ByteOrder nativeOrder() {
            return NATIVE_ORDER;
        }

        private readonly String name;

        private ByteOrder(String name) : base (){
            this.name = name;
        }

        /**
         * Returns a string that describes this object.
         * 
         * @return "BIG_ENDIAN" for {@link #BIG_ENDIAN ByteOrder.BIG_ENDIAN}
         *         objects, "LITTLE_ENDIAN" for
         *         {@link #LITTLE_ENDIAN ByteOrder.LITTLE_ENDIAN} objects.
         */
        public override String ToString() {
            return name;
        }
    }
}
