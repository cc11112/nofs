using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Fuse.Util;
using biz.ritter.javapi.nio;


namespace Nofs.Fuse
{
    public interface Filesystem3
    {
        int getattr(String path, FuseGetattrSetter getattrSetter);//throws FuseException;

        int readlink(String path, CharBuffer link);//throws FuseException;

        int getdir(String path, FuseDirFiller dirFiller);//throws FuseException;

        int mknod(String path, int mode, int rdev);//throws FuseException;

        int mkdir(String path, int mode);//throws FuseException;

        int unlink(String path);//throws FuseException;

        int rmdir(String path);//throws FuseException;

        int symlink(String from, String to);//throws FuseException;

        int rename(String from, String to);//throws FuseException;

        int link(String from, String to);//throws FuseException;

        int chmod(String path, int mode);//throws FuseException;

        int chown(String path, int uid, int gid);//throws FuseException;

        int truncate(String path, long size);//throws FuseException;

        int utime(String path, int atime, int mtime);//throws FuseException;

        int statfs(FuseStatfsSetter statfsSetter);//throws FuseException;

        // if open returns a filehandle by calling FuseOpenSetter.setFh() method, it will be passed to every method that supports 'fh' argument
        int open(String path, int flags, FuseOpenSetter openSetter);//throws FuseException;

        // fh is filehandle passed from open
        int read(String path, Object fh, ByteBuffer buf, long offset);//throws FuseException;

        // fh is filehandle passed from open,
        // isWritepage indicates that write was caused by a writepage
        int write(String path, Object fh, Boolean isWritepage, ByteBuffer buf, long offset);//throws FuseException;

        // called on every filehandle close, fh is filehandle passed from open
        int flush(String path, Object fh);//throws FuseException;

        // called when last filehandle is closed, fh is filehandle passed from open
        int release(String path, Object fh, int flags);//throws FuseException;

        // Synchronize file contents, fh is filehandle passed from open,
        // isDatasync indicates that only the user data should be flushed, not the meta data
        int fsync(String path, Object fh, Boolean isDatasync);//throws FuseException;
    }
}
