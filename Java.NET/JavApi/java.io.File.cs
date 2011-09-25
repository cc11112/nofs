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
using System.Collections.Generic;

namespace biz.ritter.javapi.io
{
    [Serializable()]
    public class File : Serializable, java.lang.Comparable<File>
    {
        public static readonly String separator = java.lang.SystemJ.getProperty ("file.separator");
        public static readonly char separatorChar = java.lang.SystemJ.getProperty("file.separator")[0];
        public static readonly String pathSeparator = java.lang.SystemJ.getProperty("path.separator");
        public static readonly char pathSeparatorChar = java.lang.SystemJ.getProperty("path.separaor")[0];
        protected System.IO.FileInfo info;
        private String fullQualifiedFile;

        public File(String pathname)
        {
            this.init(pathname);
        }
        public File(File parent, String child)
        {
            this.init(parent.fullQualifiedFile + java.lang.SystemJ.getProperty("file.separator") + child);
        }
        public File(String parent, String child)
        {
            this.init(parent + java.lang.SystemJ.getProperty("file.separator") + child);
        }
        public File(java.net.URI uri)
        {
            throw new NotImplementedException();
        }
        private void init(String newFullQualifiedFile)
        {
            this.fullQualifiedFile = newFullQualifiedFile;
            this.info = new System.IO.FileInfo(this.fullQualifiedFile);
        }

        public virtual int compareTo(File other) {
            throw new NotImplementedException();
        }

        public virtual String toString()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return fullQualifiedFile;
        }

        public String getPath()
        {
            if (isDirectory())
            {
                return this.fullQualifiedFile.Substring(0, fullQualifiedFile.Length - 1);
            }
            return this.fullQualifiedFile;
        }

        public bool isDirectory()
        {
            //return info.Attributes.HasFlag(System.IO.FileAttributes.Directory);
            //3.5
            return (info.Attributes | System.IO.FileAttributes.Directory) != 0;
        }

        public bool isFile()
        {
            //return !info.Attributes.HasFlag(System.IO.FileAttributes.Device) &&
            //       !info.Attributes.HasFlag(System.IO.FileAttributes.Directory);

            return (info.Attributes | System.IO.FileAttributes.Device) == 0  &&
                   (info.Attributes | System.IO.FileAttributes.Directory) == 0;
        }
        public long length()
        {
            return info.Length;
        }
        public long lastModified()
        {
            TimeSpan timeDiff = info.LastWriteTimeUtc - new DateTime(1970, 1, 1);
            return (long)timeDiff.TotalMilliseconds;
        }

        private static IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            //Returns an enumerable collection of file-system entries in a specified path.

#if DOTNET_FRAMEWORK_4
            return System.IO.Directory.EnumerateFileSystemEntries(path);
#else
            List<string> list = new List<string>();

            list.AddRange(System.IO.Directory.GetDirectories(path));
            list.AddRange(System.IO.Directory.GetFiles(path));
            
            return list;
#endif
        }

        public String[] list()
        {
            if (!this.isDirectory()) return null;
            java.util.ArrayList<String> entries = new util.ArrayList<String>();
            //Returns an enumerable collection of file-system entries in a specified path.
            foreach (String next in EnumerateFileSystemEntries(this.fullQualifiedFile))
            {
                entries.add(next);
            }
            String [] content = new String [entries.size()];
            return entries.toArray<String>(content);
        }
        public String getName()
        {
            return this.info.Name;
        }

        public bool delete()
        {
            try
            {
                this.info.Delete();
                return info.Exists;
            }
            catch (System.IO.IOException notDeleted)
            {
                return false;
            }
        }

        public String getAbsolutePath()
        {
            return info.FullName;
        }

        public bool exists () {
            return this.info.Exists;
        }
        public bool canRead()
        {
            lock (this.info)
            {
                System.IO.Stream canReadCheckStream = null;
                try
                {
                    //Die Datei öffnen.
                    canReadCheckStream = info.Open(System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    canReadCheckStream.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
    }
}
