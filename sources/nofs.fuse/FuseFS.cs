using System;
using biz.ritter.javapi.nio;


namespace Nofs.Fuse
{

    /**
     * This is a byte level filesystem API (in contrast to String level filesystem API like fuse.Filesystem[123]).
     * Any paths/names are passed as native ByteBuffer objects or byte[] arrays. This is the interface
     * that is called from JNI bindings. It is not intended that this interface be implemented directly by users
     * but instead a fuse.Filesystem[123] interface should be implemented and encoding of file names and paths should
     * be left to a special adapter class: fuse.Filesystem3ToFuseFSAdapter.
     *
     * Return value from every method is allways 0 for success or errno for error
     */
    public interface FuseFS
    {
        int getattr(ByteBuffer path, FuseGetattrSetter getattrSetter);

        int readlink(ByteBuffer path, ByteBuffer link);

        int getdir(ByteBuffer path, FuseFSDirFiller dirFiller);

        int mknod(ByteBuffer path, int mode, int rdev);

        int mkdir(ByteBuffer path, int mode);

        int unlink(ByteBuffer path);

        int rmdir(ByteBuffer path);

        int symlink(ByteBuffer from, ByteBuffer to);

        int rename(ByteBuffer from, ByteBuffer to);

        int link(ByteBuffer from, ByteBuffer to);

        int chmod(ByteBuffer path, int mode);

        int chown(ByteBuffer path, int uid, int gid);

        int truncate(ByteBuffer path, long size);

        int utime(ByteBuffer path, int atime, int mtime);

        int statfs(FuseStatfsSetter statfsSetter);

        int open(ByteBuffer path, int flags, FuseOpenSetter openSetter);

        int read(ByteBuffer path, Object fh, ByteBuffer buf, long offset);

        int write(ByteBuffer path, Object fh, Boolean isWritepage, ByteBuffer buf, long offset);

        int flush(ByteBuffer path, Object fh);

        int release(ByteBuffer path, Object fh, int flags);

        int fsync(ByteBuffer path, Object fh, Boolean isDatasync);

        //
        // extended attributes support contributed by Steven Pearson <steven_pearson@final-step.com>
        // and then modified by Peter Levart <peter@select-tech.si> to fit the new errno returning scheme

        int setxattr(ByteBuffer path, ByteBuffer name, ByteBuffer value, int flags);

        int getxattrsize(ByteBuffer path, ByteBuffer name, FuseSizeSetter sizeSetter);

        int getxattr(ByteBuffer path, ByteBuffer name, ByteBuffer value);

        int listxattrsize(ByteBuffer path, FuseSizeSetter sizeSetter);

        int listxattr(ByteBuffer path, ByteBuffer list);

        int removexattr(ByteBuffer path, ByteBuffer name);
    }

}
