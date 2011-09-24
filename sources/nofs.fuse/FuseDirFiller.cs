using System;

namespace Nofs.Fuse
{
    public interface FuseDirFiller
    {
         void add(String name, long inode, int mode);
    }
}
