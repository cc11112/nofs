using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Globalization;
using Nofs.Fuse.Util;
using System.IO;
using biz.ritter.javapi.nio;
using biz.ritter.javapi.lang;


namespace Nofs.Fuse
{
    public class FakeFilesystem : Filesystem3, XattrSupport
    {
        private static ILog log = LogManager.GetLogger(typeof(FakeFilesystem));

        private static int BLOCK_SIZE = 512;
        private static int NAME_LENGTH = 1024;

        private class N
        {
            public static int nfiles = 0;

            public String name;
            public int mode;
            public IDictionary<String, byte[]> xattrs = new Dictionary<String, byte[]>();

            public N(String name, int mode, params String[] xattrs)
            {
                this.name = name;
                this.mode = mode;

                for (int i = 0; i < xattrs.Length - 1; i += 2)
                {
                    String key = xattrs[i];
                    if (this.xattrs.ContainsKey(key))
                    {
                        this.xattrs[key] = UTF8Encoding.UTF8.GetBytes(xattrs[i + 1]);
                    }
                    else
                    {
                        this.xattrs.Add(xattrs[i], UTF8Encoding.UTF8.GetBytes(xattrs[i + 1]));
                    }
                }

                nfiles++;
            }

            public override String ToString()
            {
                String cn = GetType().Name;

                return cn.Substring(cn.IndexOf("$")) + "[ name=" + name + ", mode=" +
                    Convert.ToString(mode, 8) + "(OCT) ]";
            }
        }

        private class D : N
        {
            public IDictionary<String, N> files = new Dictionary<String, N>();

            public D(String name, int mode, params String[] xattrs)
                : base(name, mode, xattrs)
            {
            }

            public void add(N n)
            {
                if (files.ContainsKey(n.name))
                {
                    files[n.name] = n;
                }
                else
                {
                    files.Add(n.name, n);
                }
            }

            public override string  ToString()
            {

                return base.ToString() + " with " + files.Count.ToString(CultureInfo.InvariantCulture) + " files";
            }
        }

        private class F : N
        {
            public byte[] content;

            public F(String name, int mode, String content, params String[] xattrs)
                :base(name, mode, xattrs)
            {
                this.content = UTF8Encoding.UTF8.GetBytes(content);
            }
        }

        private class L : N
        {
            public String link;

            public L(String name, int mode, String link, params String[] xattrs)
                :base(name, mode, xattrs)
            {

                this.link = link;
            }
        }

        private class FH
        {
            public N n;

            public FH(N n)
            {
                this.n = n;
                log.Debug("  " + this + " created");
            }

            public void release()
            {
                log.Debug("  " + this + " released");
            }

            protected void finalize()
            {
                log.Debug("  " + this + " finalized");
            }

            public override string ToString()
            {

                return "FH[" + n + ", GetHashCode=" + GetHashCode() + "]";
            }
        }


        // a root directory
        private D root;


        // lookup node
        private N lookup(String path)
        {
            if (path.Equals("/"))
                return root;

            FileInfo f = new FileInfo(path);
            N parent = lookup(Path.GetDirectoryName(path));
            N node = null;
            if (parent is D)
            {
                ((D)parent).files.TryGetValue(f.FullName, out node);
            }
            
            if (log.IsDebugEnabled)
                log.Debug("  lookup(\"" + path + "\") returning: " + node);

            return node;
        }


        public FakeFilesystem()
        {
            root = new D("", 0755, "description", "ROOT directory");

            root.add(new F("README", 0644, "Hou have read me\n", "mimetype", "text/plain", "description", "a README file"));
            root.add(new F("execute_me.sh", 0755, "#!/bin/sh\n\necho \"You executed me\"\n", "mimetype", "text/plain", "description", "a BASH script"));

            D subdir = new D("subdir", 0755, "description", "a subdirectory");
            root.add(subdir);
            subdir.add(new L("README.link", 0666, "../README", "description", "a symbolic link"));
            subdir.add(new L("execute_me.link.sh", 0666, "../execute_me.sh", "description", "another symbolic link"));

            log.Info("created");
        }


        // throws FuseException
        public int chmod(String path, int mode)
        {
            N n = lookup(path);

            if (n != null)
            {
                n.mode = (n.mode & FuseStatConstants.TYPE_MASK) | (mode & FuseStatConstants.MODE_MASK);
                return 0;
            }

            return Errno.ENOENT;
        }

        //throws FuseException
        public int chown(String path, int uid, int gid)
        {
            return 0;
        }

        //throws FuseException
        public int getattr(String path, FuseGetattrSetter getattrSetter)
        {
            N n = lookup(path);

            int time = (int)(DateTime.Now.Ticks / 1000L);

            if (n is D)
            {
                D d = (D)n;
                getattrSetter.set(
                   d.GetHashCode(),
                   FuseFtypeConstants.TYPE_DIR | d.mode,
                   1,
                   0, 0,
                   0,
                   d.files.Count * NAME_LENGTH,
                   (d.files.Count * NAME_LENGTH + BLOCK_SIZE - 1) / BLOCK_SIZE,
                   time, time, time
                );

                return 0;
            }
            else if (n is F)
            {
                F f = (F)n;
                getattrSetter.set(
                   f.GetHashCode(),
                   FuseFtypeConstants.TYPE_FILE | f.mode,
                   1,
                   0, 0,
                   0,
                   f.content.Length,
                   (f.content.Length + BLOCK_SIZE - 1) / BLOCK_SIZE,
                   time, time, time
                );

                return 0;
            }
            else if (n is L)
            {
                L l = (L)n;
                getattrSetter.set(
                   l.GetHashCode(),
                   FuseFtypeConstants.TYPE_SYMLINK | l.mode,
                   1,
                   0, 0,
                   0,
                   l.link.Length,
                   (l.link.Length + BLOCK_SIZE - 1) / BLOCK_SIZE,
                   time, time, time
                );

                return 0;
            }

            return Errno.ENOENT;
        }

        //throws FuseException
        public int getdir(String path, FuseDirFiller filler) 
   {
      N n = lookup(path);

      if (n is D)
      {
         foreach (N child in ((D) n).files.Values)
         {
            int ftype = (child is D)
                        ? FuseFtypeConstants.TYPE_DIR
                        : ((child is F)
                           ? FuseFtypeConstants.TYPE_FILE
                           : ((child is L)
                           ? FuseFtypeConstants.TYPE_SYMLINK
                           : 0));
            if (ftype > 0)
               filler.add(
                  child.name,
                  child.GetHashCode(),
                  ftype | child.mode
               );
         }

         return 0;
      }

      return Errno.ENOTDIR;
   }

        // throws FuseException
        public int link(String from, String to)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int mkdir(String path, int mode)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int mknod(String path, int mode, int rdev)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int rename(String from, String to)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int rmdir(String path)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int statfs(FuseStatfsSetter statfsSetter)
        {
            statfsSetter.set(
               BLOCK_SIZE,
               1000,
               200,
               180,
               N.nfiles,
               0,
               NAME_LENGTH
            );

            return 0;
        }

        //throws FuseException
        public int symlink(String from, String to)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int truncate(String path, long size)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int unlink(String path)
        {
            return Errno.EROFS;
        }

        //throws FuseException
        public int utime(String path, int atime, int mtime)
        {
            return 0;
        }

        //throws FuseException
        public int readlink(String path, CharBuffer link)
        {
            N n = lookup(path);

            if (n is L)
            {
                link.append(new StringJ(((L)n).link));
                return 0;
            }

            return Errno.ENOENT;
        }

        // if open returns a filehandle by calling FuseOpenSetter.setFh() method, it will be passed to every method that supports 'fh' argument
        //throws FuseException
        public int open(String path, int flags, FuseOpenSetter openSetter)
        {
            N n = lookup(path);

            if (n != null)
            {
                openSetter.setFh(new FH(n));
                return 0;
            }

            return Errno.ENOENT;
        }

        // fh is filehandle passed from open,
        // isWritepage indicates that write was caused by a writepage
        // throws FuseException
        public int write(String path, Object fh, bool isWritepage, ByteBuffer buf, long offset)
        {
            return Errno.EROFS;
        }

        // fh is filehandle passed from open
        //throws FuseException
        public int read(String path, Object fh, ByteBuffer buf, long offset)
        {
            if (fh is FH)
            {
                F f = (F)((FH)fh).n;
                buf.put(f.content, (int)offset, System.Math.Min(buf.remaining(), f.content.Length - (int)offset));

                return 0;
            }

            return Errno.EBADF;
        }

        // new operation (called on every filehandle close), fh is filehandle passed from open
        //throws FuseException
        public int flush(String path, Object fh)
        {
            if (fh is FH)
                return 0;

            return Errno.EBADF;
        }

        // new operation (Synchronize file contents), fh is filehandle passed from open,
        // isDatasync indicates that only the user data should be flushed, not the meta data
        //   throws FuseException
        public int fsync(String path, Object fh, bool isDatasync)
        {
            if (fh is FH)
                return 0;

            return Errno.EBADF;
        }

        // (called when last filehandle is closed), fh is filehandle passed from open
        //throws FuseException
        public int release(String path, Object fh, int flags)
        {
            if (fh is FH)
            {
                //((FH) fh).release();
                //System.runFinalization();
                return 0;
            }

            return Errno.EBADF;
        }

        //
        // XattrSupport implementation

        /**
         * This method will be called to get the value of the extended attribute
         *
         * @params path the path to file or directory containing extended attribute
         * @params name the name of the extended attribute
         * @params dst  a ByteBuffer that should be filled with the value of the extended attribute
         * @return 0 if Ok or errno when error
         * @throws fuse.FuseException an alternative to returning errno is to throw this exception with errno initialized
         * @throws java.nio.BufferOverflowException
         *                            should be thrown to indicate that the given <code>dst</code> ByteBuffer
         *                            is not large enough to hold the attribute's value. After that <code>getxattr()</code> method will
         *                            be called again with a larger buffer.
         */
        //throws FuseException, BufferOverflowException
        public int getxattr(String path, String name, ByteBuffer dst)
        {
            N n = lookup(path);

            if (n == null)
                return Errno.ENOENT;

            byte[] value = null;

            n.xattrs.TryGetValue(name, out value);

            if (value == null)
                return Errno.ENOATTR;

            dst.put(value);

            return 0;
        }

        /**
         * This method can be called to query for the size of the extended attribute
         *
         * @params path       the path to file or directory containing extended attribute
         * @params name       the name of the extended attribute
         * @params sizeSetter a callback interface that should be used to set the attribute's size
         * @return 0 if Ok or errno when error
         * @throws fuse.FuseException an alternative to returning errno is to throw this exception with errno initialized
         */
        //throws FuseException
        public int getxattrsize(String path, String name, FuseSizeSetter sizeSetter)
        {
            N n = lookup(path);

            if (n == null)
                return Errno.ENOENT;

            byte[] value= null;

            n.xattrs.TryGetValue(name, out value);

            if (value == null)
                return Errno.ENOATTR;

            sizeSetter.setSize(value.Length);

            return 0;
        }

        /**
         * This method will be called to get the list of extended attribute names
         *
         * @params path   the path to file or directory containing extended attributes
         * @params lister a callback interface that should be used to list the attribute names
         * @return 0 if Ok or errno when error
         * @throws fuse.FuseException an alternative to returning errno is to throw this exception with errno initialized
         */
        //throws FuseException
        public int listxattr(String path, XattrLister lister) 
   {
      N n = lookup(path);

      if (n == null)
         return Errno.ENOENT;

      foreach (String xattrName in n.xattrs.Keys)
         lister.add(xattrName);

      return 0;
   }

        /**
         * This method will be called to remove the extended attribute
         *
         * @params path the path to file or directory containing extended attributes
         * @params name the name of the extended attribute
         * @return 0 if Ok or errno when error
         * @throws fuse.FuseException an alternative to returning errno is to throw this exception with errno initialized
         */
        //throws FuseException
        public int removexattr(String path, String name)
        {
            return Errno.EROFS;
        }

        /**
         * This method will be called to set the value of an extended attribute
         *
         * @params path  the path to file or directory containing extended attributes
         * @params name  the name of the extended attribute
         * @params value the value of the extended attribute
         * @params flags parameter can be used to refine the semantics of the operation.<p>
         *              <code>XATTR_CREATE</code> specifies a pure create, which should fail with <code>Errno.EEXIST</code> if the named attribute exists already.<p>
         *              <code>XATTR_REPLACE</code> specifies a pure replace operation, which should fail with <code>Errno.ENOATTR</code> if the named attribute does not already exist.<p>
         *              By default (no flags), the  extended  attribute  will  be created if need be, or will simply replace the value if the attribute exists.
         * @return 0 if Ok or errno when error
         * @throws fuse.FuseException an alternative to returning errno is to throw this exception with errno initialized
         */
        //throws FuseException
        public int setxattr(String path, String name, ByteBuffer value, int flags)
        {
            return Errno.EROFS;
        }


        //
        // Java entry point

        public static void main(String[] args)
        {
            log.Info("entering");

            try
            {
                FuseMount.mount(args, new FakeFilesystem(), log);
            }
            catch (System.Exception e)
            {
                Console.Write(e.StackTrace);
            }
            finally
            {
                log.Info("exiting");
            }
        }
    }
}
