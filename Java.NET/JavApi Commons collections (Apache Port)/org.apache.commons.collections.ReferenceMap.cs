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

namespace org.apache.commons.collections
{

    /**
     *  Hash-based {@link Map} implementation that allows
     *  mappings to be removed by the garbage collector.<p>
     *
     *  When you construct a <code>ReferenceMap</code>, you can 
     *  specify what kind of references are used to store the
     *  map's keys and values.  If non-hard references are 
     *  used, then the garbage collector can remove mappings
     *  if a key or value becomes unreachable, or if the 
     *  JVM's memory is running low.  For information on how
     *  the different reference types behave, see
     *  {@link Reference}.<p>
     *
     *  Different types of references can be specified for keys
     *  and values.  The keys can be configured to be weak but
     *  the values hard, in which case this class will behave
     *  like a <a href="http://java.sun.com/j2se/1.4/docs/api/java/util/WeakHashMap.html">
     *  <code>WeakHashMap</code></a>.  However, you
     *  can also specify hard keys and weak values, or any other
     *  combination.  The default constructor uses hard keys
     *  and soft values, providing a memory-sensitive cache.<p>
     *
     *  The algorithms used are basically the same as those
     *  in {@link java.util.HashMap}.  In particular, you 
     *  can specify a load factor and capacity to suit your
     *  needs.  All optional {@link Map} operations are 
     *  supported.<p>
     *
     *  However, this {@link Map} implementation does <I>not</I>
     *  allow null elements.  Attempting to add a null key or
     *  or a null value to the map will raise a 
     *  <code>NullPointerException</code>.<p>
     *
     *  As usual, this implementation is not synchronized.  You
     *  can use {@link java.util.Collections#synchronizedMap} to 
     *  provide synchronized access to a <code>ReferenceMap</code>.
     *
     * @see java.lang.ref.Reference
     * 
     * @deprecated Moved to map subpackage. Due to be removed in v4.0.
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     * 
     * @author Paul Jack
     */
    [Obsolete]
    public class ReferenceMap : org.apache.commons.collections.map.ReferenceMap
    {
    }
}