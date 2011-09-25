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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    public class HashMap<K,V> : AbstractMap<K,V>
    {

        public HashMap(){}

        public HashMap(int capacity)
            : base()//!FIX Do not use ignore param 
        {

        }
        public HashMap (int capacity, float loadFactor) :base ()//!FIX Do not use ignore param 
        {}

        public HashMap(Map<K, V> map)
            : base()
        {
            this.putAll(map);
        }
    }
    
}
