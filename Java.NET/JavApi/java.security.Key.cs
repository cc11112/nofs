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

namespace biz.ritter.javapi.security
{

    /**
     * {@code Key} is the common interface for all keys.
     * 
     * @see PublicKey
     * @see PrivateKey
     */
    public interface Key : java.io.Serializable {
        /**
         * Returns the name of the algorithm of this key. If the algorithm is
         * unknown, {@code null} is returned.
         * 
         * @return the name of the algorithm of this key or {@code null} if the
         *         algorithm is unknown.
         */
        String getAlgorithm();

        /**
         * Returns the name of the format used to encode this key, or {@code null}
         * if it can not be encoded.
         * 
         * @return the name of the format used to encode this key, or {@code null}
         *         if it can not be encoded.
         */
        String getFormat();

        /**
         * Returns the encoded form of this key, or {@code null} if encoding is not
         * supported by this key.
         * 
         * @return the encoded form of this key, or {@code null} if encoding is not
         *         supported by this key.
         */
        byte[] getEncoded();
    }
}
