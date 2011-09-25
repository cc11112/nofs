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
using System.Diagnostics;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{
    public class Runtime
    {
        private Runtime (){}

        private static Runtime runtimeInstance;

        public static Runtime getRuntime()
        {
            if (null == runtimeInstance)
            {
                runtimeInstance = new Runtime();
            }
            return runtimeInstance;
        }

        public int availableProcessors()
        {
            return Environment.ProcessorCount;
        }

        public static Process exec (String command) {
            return exec(command, new String [0], null);
        }
        public static Process exec(String command, String[] env) {
            return exec(command, env, null);
        }
        public static Process exec(String command, String[] env, java.io.File dir) {
            return exec(command.Split(new char []{' ','\t','\n','\r','\f'}),env, dir);
        }
        public static Process exec(String[] cmdArray) {
            return exec(cmdArray, new String[0], null);
        }
        public static Process exec(String[] cmdArray, String[] env) {
            return exec(cmdArray, env, null);
        }
        public static Process exec(String[] cmdArray, String[] env, java.io.File dir) {
            Process p = new Process();
            p.StartInfo.WorkingDirectory = (null!=dir) ? dir.toString () : SystemJ.getProperty("user.dir");
            p.StartInfo.FileName = cmdArray[0];
            for (int i = 0; i < env.Length; i++) {
                String [] keyValue = env [i].Split ('=');
                p.StartInfo.EnvironmentVariables.Add (keyValue[0],keyValue[1]);
            }
            for (int i = 1; i < cmdArray.Length; i++) {
                p.StartInfo.Arguments.Insert(i - 1, cmdArray[i]);
            }
            p.StartInfo.UseShellExecute = true;
            p.Start();
            return p;
        }
        public void gc()
        {
            GC.Collect();
        }
        public long totalMemory()
        {
            return GC.GetTotalMemory(true);
        }
        public long freeMemory()
        {
            PerformanceCounter perCnt = new PerformanceCounter("Memory", "Available Bytes");
            long availableMemory = Convert.ToInt64(perCnt.NextValue());
            return availableMemory;
        }
        /// <summary>
        /// Stop the current process.
        /// </summary>
        /// <see cref="#addShutdownHook"/>
        /// <param name="rc">return code for parent process</param>
        public void exit(int rc)
        {
            Environment.ExitCode = rc;
            // Envirionment.Exit(rc) would NOT do a cooperative shutdown. No finalizers are called!
            // Now using Thread for working addShutdownHook
            System.Threading.Thread t = new System.Threading.Thread(delegate()
            {
                Environment.Exit(1);
            });
            t.Start();
            t.Join();
        }
        /// <summary>
        /// Add an thread to running on exit the CLR.
        /// </summary>
        /// <param name="hook"></param>
        public void addShutdownHook(Thread hook)
        {
            // Check hook for null
            if (hook == null) throw new NullPointerException();

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                if (Thread.State.NEW.Equals (hook.getState()))
                {
                    hook.start();
                }
            };
            AppDomain.CurrentDomain.ProcessExit += delegate(Object sender, EventArgs e)
            {
                if (Thread.State.NEW.Equals(hook.getState()))
                {
                    hook.start();
                }
            };
        }

    }
}
