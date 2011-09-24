
namespace Nofs.Fuse
{
    public interface FuseStatfsSetter
    {
        void set(int blockSize, int blocks, int blocksFree, int blocksAvail, int files, int filesFree, int namelen);
    }
}
