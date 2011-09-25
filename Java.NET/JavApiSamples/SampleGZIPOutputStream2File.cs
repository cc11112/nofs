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

namespace javapi.sample.util.zip
{
    class SampleGZIPOutputStream2File
    {
        static void Main()
        {
            String outFilename = "c:/temp/outfile.gzip";
            java.util.zip.GZIPOutputStream outJ = new java.util.zip.GZIPOutputStream(new java.io.FileOutputStream(outFilename));

            String inFilename = "c:/README.txt";
            java.io.FileInputStream inJ = new java.io.FileInputStream(inFilename);

            byte[] buf = new byte[1024];
            int len;
            while ((len = inJ.read(buf)) > 0)
            {
                outJ.write(buf, 0, len);
            }
            inJ.close();

            outJ.finish();
            outJ.close();
        }
    }
}

