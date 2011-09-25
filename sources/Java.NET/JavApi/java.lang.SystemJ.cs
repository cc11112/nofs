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
using System.Collections;
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{
    public sealed class SystemJ
    {

        private static java.util.Properties systemProperties;
        static SystemJ()
        {
            outJ = new dotnet.lang.ConsoleOutputPrintStream();
            inJ = new dotnet.lang.ConsoleInputStream();
            err = new dotnet.lang.ConsoleErrorPrintStream();
        }

        public SystemJ()
        {
        }

        public static int identityHashCode (Object o) {
            if (null == o) return 0;
            return o.GetHashCode();
        }

        public static util.Properties getProperties()
        {
            if (null == systemProperties)
            {
                // Collect the informations
                IDictionary env = Environment.GetEnvironmentVariables();
                java.util.Properties prop = new java.util.Properties(env);
                int osMajor = System.Environment.OSVersion.Version.Major;
                int osMinior = System.Environment.OSVersion.Version.Minor;
                int osVersion = osMajor * 10 + osMinior;
                String osName = "Unknown";
                switch (System.Environment.OSVersion.Platform)
                {
                    case PlatformID.WinCE:
                        osName = "Windows CE";
                        break;
                    case PlatformID.Xbox :
                        osName = "XBox";
                        break;
                    case PlatformID.Unix:
                        osName = "Unix";
                        break;
                    case PlatformID.MacOSX:
                        osName = "MacOS";
                        break;
                    case PlatformID.Win32S:
                        osName = "Windows 32";
                        break;
                    case PlatformID.Win32Windows:
                        osName = "Windows ";
                        switch (osVersion)
                        {
                            case 40 :
                                osName += "95";
                                break;
                            case 50 :
                                osName += "98";
                                break;
                            case 130:
                                osName += "ME";
                                break;
                        }
                        break;
                    case PlatformID.Win32NT:
                        osName = "Windows ";
                        switch (osVersion)
                        {
                            case 40:
                                osName += "NT 4.0";
                                break;
                            case 50:
                                osName += "2000";
                                break;
                            case 51:
                                osName += "XP";
                                break;
                            case 52:
                                osName += "Server 2003";
                                break;
                            case 60:
                                osName += "Vista";
                                break;
                            case 61:
                                osName += "7";
                                break;
                        }
                        break;
                }
                
                // store some Java system specific environment
                prop.Add("file.encoding", Encoding.Default.WebName);
                prop.Add("file.separator", System.IO.Path.DirectorySeparatorChar);
                prop.Add("line.separator", System.Environment.NewLine);
                prop.Add("os.arch", System.Environment.GetEnvironmentVariable ("PROCESSOR_ARCHITEW6432"));
                prop.Add("os.name", osName); 
                prop.Add("os.version", osMajor+"."+osMinior);
                prop.Add("path.separator", System.IO.Path.PathSeparator);
                prop.Add("user.country", System.Globalization.CultureInfo.CurrentCulture.Name.Substring(3, 2));
                prop.Add("user.dir", System.Environment.CurrentDirectory);
                // Mono 2.6.4 BugFix:
                if (PlatformID.Unix == System.Environment.OSVersion.Platform)
                {
                    prop.Add("user.home", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                }
                else
                { // UserProfile not exist in Mono 2.6.4
                    //prop.Add("user.home", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                    //.net frame 3.4 has not Environment.SpecialFolder.UserProfile

                    prop.Add("user.home", Environment.GetFolderPath(Environment.SpecialFolder.Personal));

                    //need 4.0 support
                    //prop.Add("user.home", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                }
                prop.Add("user.language", System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                prop.Add("user.name", System.Environment.UserName);
                prop.Add("user.timezone", null); // content???
                prop.Add("user.variant", null); // content???
                // Java specific environment
                prop.Add("java.vendor","Sebastian Ritter");
                prop.Add("java.version",System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                prop.Add("java.class.path", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                // own important environment

                // Here it is...
                systemProperties = prop;
            }
            return systemProperties;
        }

        public static String getProperty(String key)
        {
            return getProperties().getProperty(key);
        }

        public static String getProperty(String key, String defaultValue)
        {
            return getProperties().getProperty(key, defaultValue);
        }

        /*
        java.runtime.name=Java(TM) SE Runtime Environment
        java.vm.version=16.0-b13
        java.vm.vendor=Sun Microsystems Inc.
        java.vendor.url=http://java.sun.com/
        sun.boot.library.path=C:\Program Files\Java\jdk1.6.0_18\jre...
        path.separator=;
        java.vm.name=Java HotSpot(TM) 64-Bit Server VM
        file.encoding.pkg=sun.io
        user.country=DE
        sun.java.launcher=SUN_STANDARD
        sun.os.patch.level=
        java.vm.specification.name=Java Virtual Machine Specification
        user.dir=C:\Develop\Projekte\CODE\JavApiSamples
        java.runtime.version=1.6.0_18-b07
        java.awt.graphicsenv=sun.awt.Win32GraphicsEnvironment
        java.endorsed.dirs=C:\Program Files\Java\jdk1.6.0_18\jre...
        os.arch=amd64
        java.io.tmpdir=C:\Users\Ritter\AppData\Local\Temp\
        java.vm.specification.vendor=Sun Microsystems Inc.
        user.variant=
        os.name=Windows 7
        sun.jnu.encoding=Cp1252
        java.library.path=C:\Program Files\Java\jdk1.6.0_18\bin...
        java.specification.name=Java Platform API Specification
        java.class.version=50.0
        sun.management.compiler=HotSpot 64-Bit Server Compiler
        os.version=6.1
        user.home=C:\Users\Ritter
        user.timezone=
        java.awt.printerjob=sun.awt.windows.WPrinterJob
        file.encoding=Cp1252
        java.specification.version=1.6
        java.class.path=C:\Develop\Projekte\CODE\JavApiSample...
        java.vm.specification.version=1.0
        sun.arch.data.model=64
        java.home=C:\Program Files\Java\jdk1.6.0_18\jre
        java.specification.vendor=Sun Microsystems Inc.
        user.language=de
        awt.toolkit=sun.awt.windows.WToolkit
        java.vm.info=mixed mode
        java.version=1.6.0_18
        java.ext.dirs=C:\Program Files\Java\jdk1.6.0_18\jre...
        sun.boot.class.path=C:\Program Files\Java\jdk1.6.0_18\jre...
        java.vendor=Sun Microsystems Inc.
        file.separator=\
        java.vendor.url.bug=http://java.sun.com/cgi-bin/bugreport...
        sun.cpu.endian=little
        sun.io.unicode.encoding=UnicodeLittle
        sun.desktop=windows
        sun.cpu.isalist=amd64

         */


        /// <see cref="java.lang.System.out"/>
        public static PrintStream outJ;
        public static PrintStream err;
        public static java.io.InputStream inJ;

        public void setOut(PrintStream outputPrintStream)
        {
            outJ = outputPrintStream;
        }

        public void setOut(java.io.InputStream inputStream)
        {
            inJ = inputStream;
        }

        public static void arraycopy(Object src, int srcPos, Object dest, int destPos, int length) // throws NullPointerException, ArrayStoreException, NullPointerException
        {
            if (!(src is Array && dest is Array)) throw new ArrayStoreException();
            if (null == src || dest == null) throw new NullPointerException();
            System.Array.Copy((Array)src, srcPos, (Array)dest, destPos, length);
        }

        public static long currentTimeMillis()
        {
            TimeSpan timeDiff = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (long)timeDiff.TotalMilliseconds;
        }

        public static void gc()
        {
            Runtime.getRuntime().gc();
        }

        public static void exit(int rc)
        {
            Runtime.getRuntime().exit(rc);
        }
    }
}
