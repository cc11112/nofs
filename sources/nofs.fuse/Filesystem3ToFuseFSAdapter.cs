using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using biz.ritter.javapi.nio.charset;
using biz.ritter.javapi.nio;
using biz.ritter.javapi.lang;
using biz.ritter.javapi.util;

namespace Nofs.Fuse
{

    /**
     * This is an adapter that implements fuse.FuseFS byte level API and delegates
     * to the fuse.Filesystem3 String level API. You specify the encoding to be used
     * for file names and paths.
     */
    public class Filesystem3ToFuseFSAdapter : FuseFS
    {
        private Filesystem3 fs3;
        private XattrSupport xattrSupport;
        private Charset cs;
        private ILog log;


        public Filesystem3ToFuseFSAdapter(Filesystem3 fs3, ILog log)
            : this(fs3, SystemJ.getProperty("file.encoding", "UTF-8"), log)
        {
            
        }

        public Filesystem3ToFuseFSAdapter(Filesystem3 fs3, String encoding, ILog log)
            :this(fs3, Charset.forName(encoding), log)
        {
        }

        public Filesystem3ToFuseFSAdapter(Filesystem3 fs3, Charset cs, ILog log)
        {
            this.fs3 = fs3;

            // XattrSupport is optional
            if (fs3 is XattrSupport)
                xattrSupport = (XattrSupport)fs3;

            this.cs = cs;
            this.log = log;
        }


        //
        // FuseFS implementation


        public int getattr(ByteBuffer path, FuseGetattrSetter getattrSetter)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("getattr: path=" + pathStr);

            try
            {
                return handleErrno(fs3.getattr(pathStr, getattrSetter), getattrSetter);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int readlink(ByteBuffer path, ByteBuffer link)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("readlink: path=" + pathStr);

            CharBuffer linkCb = CharBuffer.allocate(link.capacity());

            try
            {
                int errno = fs3.readlink(pathStr, linkCb);

                if (errno == 0)
                {
                    linkCb.flip();

                    CharsetEncoder enc = cs.newEncoder()
                       .onUnmappableCharacter(CodingErrorAction.REPLACE)
                       .onMalformedInput(CodingErrorAction.REPLACE);

                    CoderResult result = enc.encode(linkCb, link, true);
                    if (result.isOverflow())
                        throw new FuseException("Buffer owerflow while encoding result").initErrno(Errno.ENAMETOOLONG);
                }

                return handleErrno(errno, linkCb.rewind());
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int getdir(ByteBuffer path, FuseFSDirFiller dirFiller)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("getdir: path=" + pathStr);

            try
            {
                dirFiller.setCharset(cs);
                return handleErrno(fs3.getdir(pathStr, dirFiller), dirFiller);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int mknod(ByteBuffer path, int mode, int rdev)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("mknod: path=" + pathStr + ", mode=" + Integer.toString(mode, 8) + "(OCT), rdev=" + rdev);

            try
            {
                return handleErrno(fs3.mknod(pathStr, mode, rdev));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int mkdir(ByteBuffer path, int mode)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("mkdir: path=" + pathStr + ", mode=" + Integer.toString(mode, 8) + "(OCT)");

            try
            {
                return handleErrno(fs3.mkdir(pathStr, mode));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int unlink(ByteBuffer path)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("unlink: path=" + pathStr);

            try
            {
                return handleErrno(fs3.unlink(pathStr));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int rmdir(ByteBuffer path)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("rmdir: path=" + pathStr);

            try
            {
                return handleErrno(fs3.rmdir(pathStr));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int symlink(ByteBuffer from, ByteBuffer to)
        {
            String fromStr = cs.decode(from).ToString();
            String toStr = cs.decode(to).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("symlink: from=" + fromStr + " to=" + toStr);

            try
            {
                return handleErrno(fs3.symlink(fromStr, toStr));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int rename(ByteBuffer from, ByteBuffer to)
        {
            String fromStr = cs.decode(from).ToString();
            String toStr = cs.decode(to).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("rename: from=" + fromStr + " to=" + toStr);

            try
            {
                return handleErrno(fs3.rename(fromStr, toStr));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int link(ByteBuffer from, ByteBuffer to)
        {
            String fromStr = cs.decode(from).ToString();
            String toStr = cs.decode(to).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("link: from=" + fromStr + " to=" + toStr);

            try
            {
                return handleErrno(fs3.link(fromStr, toStr));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int chmod(ByteBuffer path, int mode)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("chmod: path=" + pathStr + ", mode=" + Integer.toString(mode, 8) + "(OCT)");

            try
            {
                return handleErrno(fs3.chmod(pathStr, mode));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int chown(ByteBuffer path, int uid, int gid)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("chown: path=" + pathStr + ", uid=" + uid + ", gid=" + gid);

            try
            {
                return handleErrno(fs3.chown(pathStr, uid, gid));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int truncate(ByteBuffer path, long size)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("truncate: path=" + pathStr + ", size=" + size);

            try
            {
                return handleErrno(fs3.truncate(pathStr, size));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int utime(ByteBuffer path, int atime, int mtime)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("utime: path=" + pathStr + ", atime=" + atime + " (" + new Date((long)atime * 1000L) + "), mtime=" + mtime + " (" + new Date((long)mtime * 1000L) + ")");

            try
            {
                return handleErrno(fs3.utime(pathStr, atime, mtime));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int statfs(FuseStatfsSetter statfsSetter)
        {
            if (log != null && log.IsDebugEnabled)
                log.Debug("statfs");

            try
            {
                return handleErrno(fs3.statfs(statfsSetter), statfsSetter);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int open(ByteBuffer path, int flags, FuseOpenSetter openSetter)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("open: path=" + pathStr + ", flags=" + flags);

            try
            {
                return handleErrno(fs3.open(pathStr, flags, openSetter), openSetter);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int read(ByteBuffer path, Object fh, ByteBuffer buf, long offset)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("read: path=" + pathStr + ", fh=" + fh + ", offset=" + offset);

            try
            {
                return handleErrno(fs3.read(pathStr, fh, buf, offset), buf);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int write(ByteBuffer path, Object fh, bool isWritepage, ByteBuffer buf, long offset)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("write: path=" + pathStr + ", fh=" + fh + ", isWritepage=" + isWritepage + ", offset=" + offset);

            try
            {
                return handleErrno(fs3.write(pathStr, fh, isWritepage, buf, offset), buf);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int flush(ByteBuffer path, Object fh)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("flush: path=" + pathStr + ", fh=" + fh);

            try
            {
                return handleErrno(fs3.flush(pathStr, fh));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int release(ByteBuffer path, Object fh, int flags)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("release: path=" + pathStr + ", fh=" + fh + ", flags=" + flags);

            try
            {
                return handleErrno(fs3.release(pathStr, fh, flags));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }


        public int fsync(ByteBuffer path, Object fh, bool isDatasync)
        {
            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("fsync: path=" + pathStr + ", fh=" + fh + ", isDatasync=" + isDatasync);

            try
            {
                return handleErrno(fs3.fsync(cs.decode(path).ToString(), fh, isDatasync));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }

        //
        // extended attribute support is optional

        public int getxattrsize(ByteBuffer path, ByteBuffer name, FuseSizeSetter sizeSetter)
        {
            if (xattrSupport == null)
                return handleErrno(Errno.ENOTSUPP);

            String pathStr = cs.decode(path).ToString();
            String nameStr = cs.decode(name).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("getxattrsize: path=" + pathStr + ", name=" + nameStr);

            try
            {
                return handleErrno(xattrSupport.getxattrsize(pathStr, nameStr, sizeSetter), sizeSetter);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }

        public int getxattr(ByteBuffer path, ByteBuffer name, ByteBuffer value)
        {
            if (xattrSupport == null)
                return handleErrno(Errno.ENOTSUPP);

            String pathStr = cs.decode(path).ToString();
            String nameStr = cs.decode(name).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("getxattr: path=" + pathStr + ", name=" + nameStr);

            try
            {
                return handleErrno(xattrSupport.getxattr(pathStr, nameStr, value), value);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }

        //
        // private implementation of XattrLister that estimates the byte size of the attribute names list
        // using Charset of the enclosing Filesystem3ToFuseFSAdapter class

        private class XattrSizeLister : XattrLister
        {
            public int size = 0;
            public void add(String xattrName)
            {
                CharsetEncoder enc = Charset.forName("UTF-8").newEncoder();
                size += (int)((float)xattrName.length() * enc.averageBytesPerChar()) + 1;
            }
        }

        //
        // estimate the byte size of attribute names list...

        public int listxattrsize(ByteBuffer path, FuseSizeSetter sizeSetter)
        {
            if (xattrSupport == null)
                return handleErrno(Errno.ENOTSUPP);

            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("listxattrsize: path=" + pathStr);

            int errno;
            XattrSizeLister lister = new XattrSizeLister();

            try
            {
                errno = xattrSupport.listxattr(pathStr, lister);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }

            sizeSetter.setSize(lister.size);

            return handleErrno(errno, sizeSetter);
        }

        //
        // private implementation of XattrLister that encodes list of attribute names into given ByteBuffer
        // using Charset of the enclosing Filesystem3ToFuseFSAdapter class

        private class XattrValueLister : XattrLister
        {
            public BufferOverflowException boe;

            CharsetEncoder enc = Charset.forName("UTF-8").newEncoder()
               .onMalformedInput(CodingErrorAction.REPLACE)
               .onUnmappableCharacter(CodingErrorAction.REPLACE);
            ByteBuffer list;

            public XattrValueLister(ByteBuffer list)
            {
                this.list = list;
            }

            public void add(String xattrName)
            {
                if (boe == null) // don't need to bother any more if there was an exception already
                {
                    try
                    {
                        enc.encode(CharBuffer.wrap(xattrName), list, true);
                        list.put((byte)0); // each attribute name is terminated by byte 0
                    }
                    catch (BufferOverflowException e)
                    {
                        boe = e;
                    }
                }
            }

            //
            // for debugging

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("[");
                bool first = true;

                for (int i = 0; i < list.position(); i++)
                {
                    int offset = i;
                    int length = 0;
                    while (offset + length < list.position() && list.get(offset + length) != 0)
                        length++;

                    byte[] nameBytes = new byte[length];
                    for (int j = 0; j < length; j++)
                        nameBytes[j] = list.get(offset + j);

                    if (first)
                        first = false;
                    else
                        sb.Append(", ");

                    sb.Append('"').Append(Charset.forName("UTF-8").decode(ByteBuffer.wrap(nameBytes))).Append('"');

                    i = offset + length;
                }

                sb.Append("]");

                return sb.ToString();
            }
        }

        //
        // list attributes into given ByteBuffer...

        public int listxattr(ByteBuffer path, ByteBuffer list)
        {
            if (xattrSupport == null)
                return handleErrno(Errno.ENOTSUPP);

            String pathStr = cs.decode(path).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("listxattr: path=" + pathStr);

            int errno;
            XattrValueLister lister = new XattrValueLister(list);

            try
            {
                errno = xattrSupport.listxattr(pathStr, lister);
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }

            // was there a BufferOverflowException?
            if (lister.boe != null)
                return handleException(lister.boe);

            return handleErrno(errno, lister);
        }

        public int setxattr(ByteBuffer path, ByteBuffer name, ByteBuffer value, int flags)
        {
            if (xattrSupport == null)
                return handleErrno(Errno.ENOTSUPP);

            String pathStr = cs.decode(path).ToString();
            String nameStr = cs.decode(name).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("setxattr: path=" + pathStr + ", name=" + nameStr + ", value=" + value + ", flags=" + flags);

            try
            {
                return handleErrno(xattrSupport.setxattr(pathStr, nameStr, value, flags));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }

        public int removexattr(ByteBuffer path, ByteBuffer name)
        {
            if (xattrSupport == null)
                return handleErrno(Errno.ENOTSUPP);

            String pathStr = cs.decode(path).ToString();
            String nameStr = cs.decode(name).ToString();

            if (log != null && log.IsDebugEnabled)
                log.Debug("removexattr: path= " + pathStr + ", name=" + nameStr);

            try
            {
                return handleErrno(xattrSupport.removexattr(pathStr, nameStr));
            }
            catch (System.Exception e)
            {
                return handleException(e);
            }
        }

        //
        // private

        private int handleErrno(int errno)
        {
            if (log != null && log.IsDebugEnabled)
                log.Debug((errno == 0) ? "  returning with success" : "  returning errno: " + errno);

            return errno;
        }

        private int handleErrno(int errno, Object v1)
        {
            if (errno != 0)
                return handleErrno(errno);

            if (log != null && log.IsDebugEnabled)
                log.Debug("  returning: " + v1);

            return errno;

        }

        private int handleErrno(int errno, Object v1, Object v2)
        {
            if (errno != 0)
                return handleErrno(errno);

            if (log != null && log.IsDebugEnabled)
                log.Debug("  returning: " + v1 + ", " + v2);

            return errno;

        }


        private int handleException(System.Exception e)
        {
            int errno;

            if (e is FuseException)
            {
                errno = handleErrno(((FuseException)e).getErrno());
                if (log != null && log.IsDebugEnabled)
                    log.Debug(e);
            }
            else if (e is BufferOverflowException)
            {
                errno = handleErrno(Errno.ERANGE);
                if (log != null && log.IsDebugEnabled)
                    log.Debug(e);
            }
            else
            {
                errno = handleErrno(Errno.EFAULT);
                if (log != null)
                    log.Error(e);
            }

            return errno;
        }
    }

}
