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

namespace biz.ritter.javapi.util.jar
{
/**
 * Represents a single file in a JAR archive together with the manifest
 * attributes and digital signatures associated with it.
 *
 * @see JarFile
 * @see JarInputStream
 */
    public class JarEntry : java.util.zip.ZipEntry
    {
        private Attributes attributes;

        protected internal JarFile parentJar;
        /**
         * Creates a new {@code JarEntry} named name.
         *
         * @param name
         *            The name of the new {@code JarEntry}.
         */
        public JarEntry(String name)
            : base(name)
        {
        }

        /**
         * Creates a new {@code JarEntry} using the values obtained from entry.
         *
         * @param entry
         *            The ZipEntry to obtain values from.
         */
        public JarEntry(java.util.zip.ZipEntry entry)
            : base(entry)
        {
        }

        /**
         * Returns the {@code Attributes} object associated with this entry or
         * {@code null} if none exists.
         *
         * @return the {@code Attributes} for this entry.
         * @exception IOException
         *                If an error occurs obtaining the {@code Attributes}.
         * @see Attributes
         */
        public Attributes getAttributes()
        {//throws IOException {
            if (attributes != null || parentJar == null)
            {
                return attributes;
            }
            Manifest manifest = parentJar.getManifest();
            if (manifest == null)
            {
                return null;
            }
            return attributes = manifest.getAttributes(getName());
        }
    }

}
