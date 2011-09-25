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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace biz.ritter.javapi.lang
{
    /**
     * Thrown by <code>String</code> methods to indicate that an index
     * is either negative or greater than the size of the string.  For
     * some methods such as the charAt method, this exception also is
     * thrown when the index is equal to the size of the string.
     *
     * @author  unascribed
     * @version 1.23, 11/17/05
     * @see     java.lang.String#charAt(int)
     * @since   JDK1.0
     */
    public
    class StringIndexOutOfBoundsException : IndexOutOfBoundsException {
        /**
         * Constructs a <code>StringIndexOutOfBoundsException</code> with no 
         * detail message. 
         *
         * @since   JDK1.0.
         */
        public StringIndexOutOfBoundsException() :base(){}

        /**
         * Constructs a <code>StringIndexOutOfBoundsException</code> with 
         * the specified detail message. 
         *
         * @param   s   the detail message.
         */
        public StringIndexOutOfBoundsException(String s) :base (s){}

        /**
         * Constructs a new <code>StringIndexOutOfBoundsException</code> 
         * class with an argument indicating the illegal index. 
         *
         * @param   index   the illegal index.
         */
        public StringIndexOutOfBoundsException(int index) : base ("String index out of range: " + index){}
    }
}
