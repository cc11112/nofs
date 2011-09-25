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

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    public class FileInputStream : InputStream
    {
        private System.IO.FileStream delegateInstance;

        public FileInputStream (File f) : this (f.ToString()){}
        public FileInputStream(string name)
        {
            try
            {
                delegateInstance = new System.IO.FileStream(name, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            }
            catch (System.IO.FileNotFoundException reason)
            {
                throw new java.io.IOException(reason.Message);
            }
        }
        public override int read()
        {
            return this.delegateInstance.ReadByte();
        }
        /// Optimized reading for files
        public override int read(byte[] buffer, int beginOffset, int length)
        {
            return this.delegateInstance.Read(buffer, beginOffset, length);
        }

        public override void close()
        {
            this.delegateInstance.Close();
        }
    }
}
