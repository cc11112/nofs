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
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util.jar
{
    internal class JarVerifier
    {
        private HashMap<String, byte[]> metaEntries = new HashMap<String, byte[]>();
        private readonly String jarName;


        /**
         * Constructs and returns a new instance of {@code JarVerifier}.
         *
         * @param name
         *            the name of the JAR file being verified.
         */
        internal JarVerifier(String name)
        {
            jarName = name;
        }

        /**
         * Add a new meta entry to the internal collection of data held on each JAR
         * entry in the {@code META-INF} directory including the manifest
         * file itself. Files associated with the signing of a JAR would also be
         * added to this collection.
         *
         * @param name
         *            the name of the file located in the {@code META-INF}
         *            directory.
         * @param buf
         *            the file bytes for the file called {@code name}.
         * @see #removeMetaEntries()
         */
        internal void addMetaEntry(String name, byte[] buf)
        {
            metaEntries.put(org.apache.harmony.luni.util.Util.toASCIIUpperCase(name), buf);
        }
    }
}
