using System;
using System.Threading;
using log4net;
using Nofs.Fuse.Compat;
using Nofs.Fuse.Java;


namespace Nofs.Fuse
{


    public class FuseMount
    {
        private static ILog log = LogManager.GetLogger(typeof(FuseMount));


        //static
        //{
        //   System.loadLibrary("javafs");
        //}

        private FuseMount()
        {
            // no instances
        }


        //
        // compatibility APIs

        public static void mount(String[] args, Filesystem1 filesystem1)// throws FuseException
        {
            mount(args, new Filesystem2ToFilesystem3Adapter
                (
                new Filesystem1ToFilesystem2Adapter(filesystem1)), LogManager.GetLogger(typeof(Filesystem1))
                );
        }

        public static void mount(String[] args, Filesystem2 filesystem2)// throws FuseException
        {
            mount(args, new Filesystem2ToFilesystem3Adapter(filesystem2), LogManager.GetLogger(typeof(Filesystem2)));
        }

        //
        // prefered String level API

        public static void mount(String[] args, Filesystem3 filesystem3, ILog log)// throws FuseException
        {
            mount(args, new Filesystem3ToFuseFSAdapter(filesystem3, log));
        }

        //
        // byte level API

        public static void mount(String[] args, FuseFS fuseFS)// throws FuseException
        {
            ThreadGroup threadGroup = new ThreadGroup(Thread.CurrentThread.ManagedThreadId, "FUSE Threads");
            threadGroup.setDaemon(true);

            log.Info("Mounting filesystem");

            mount(args, fuseFS, threadGroup);

            log.Info("Filesystem is unmounted");

            if (log.IsDebugEnabled)
            {
                int n = threadGroup.activeCount();
                log.Debug("ThreadGroup(\"" + threadGroup.getName() + "\").activeCount() = " + n);

                Thread[] threads = new Thread[n];
                threadGroup.enumerate(threads);
                for (int i = 0; i < threads.Length; i++)
                {
                    log.Debug("thread[" + i + "] = " + threads[i] + ", isDaemon = " /*+ threads[i].isDaemon() */);
                }
            }
        }


        private static void mount(String[] args, FuseFS fuseFS, ThreadGroup threadGroup) // throws FuseException;
        {
        }
    }
}
