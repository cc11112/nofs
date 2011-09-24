using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using biz.ritter.javapi.nio;


namespace Nofs.Fuse.Compat
{

    /**
     * The file system operations:
     *
     * Most of these should work very similarly to the well known UNIX
     * file system operations.  Exceptions are:
     *
     *  - All operations should return the error value (errno) by throwing a
     *  fuse.FuseException with a errno field of the exception set to the desired value.
     *
     *  - getdir() is the opendir(), readdir(), ..., closedir() sequence
     *  in one call.
     *
     *  - There is no create() operation, mknod() will be called for
     *  creation of all non directory, non symlink nodes.
     *
     *  - open() No
     *  creation, or trunctation flags (O_CREAT, O_EXCL, O_TRUNC) will be
     *  passed to open().  Open should only check if the operation is
     *  permitted for the given flags.
     *
     *  - read(), write(), release() are are passed a filehandle that is returned from open() in
     *  addition to a pathname.  The offset of the read and write is passed as the last
     *  argument, the number of bytes read/writen is returned through the java.nio.ByteBuffer object
     *
     *  - release() is called when an open file has:
     *       1) all file descriptors closed
     *       2) all memory mappings unmapped
     *    This call need only be implemented if this information is required.
     *
     * New operations in FUSE-J 2.2.1:
     *
     *  - flush() called when a file is closed (can be called multiple times for each dup-ed filehandle)
     *
     *  - fsync() called when file data should be synced (with a flag to sync only data but not metadata)
     *
     */
    public interface Filesystem2
    {
        FuseStat getattr(String path); // throws FuseException

        String readlink(String path); // throws FuseException

        // CHANGE-22: FuseDirEnt.inode added
        FuseDirEnt[] getdir(String path); // throws FuseException

        void mknod(String path, int mode, int rdev); // throws FuseException

        void mkdir(String path, int mode); // throws FuseException

        void unlink(String path); // throws FuseException

        void rmdir(String path); // throws FuseException

        void symlink(String from, String to); // throws FuseException

        void rename(String from, String to); // throws FuseException

        void link(String from, String to); // throws FuseException

        void chmod(String path, int mode); // throws FuseException

        void chown(String path, int uid, int gid); // throws FuseException

        void truncate(String path, long size); // throws FuseException

        void utime(String path, int atime, int mtime); // throws FuseException

        FuseStatfs statfs(); // throws FuseException

        // CHANGE-22: if open returns a filehandle it is passed to every other filesystem call
        long open(String path, int flags); // throws FuseException

        // CHANGE-22: fh is filehandle passed from open
        void read(String path, long fh, ByteBuffer buf, long offset); // throws FuseException

        // CHANGE-22: fh is filehandle passed from open,
        //            isWritepage indicates that write was caused by a writepage
        void write(String path, long fh, Boolean isWritepage, ByteBuffer buf, long offset); // throws FuseException

        // CHANGE-22: new operation (called on every filehandle close), fh is filehandle passed from open
        void flush(String path, long fh); // throws FuseException

        // CHANGE-22: (called when last filehandle is closed), fh is filehandle passed from open
        void release(String path, long fh, int flags); // throws FuseException

        // CHANGE-22: new operation (Synchronize file contents), fh is filehandle passed from open,
        //            isDatasync indicates that only the user data should be flushed, not the meta data
        void fsync(String path, long fh, Boolean isDatasync); // throws FuseException
    }

}
