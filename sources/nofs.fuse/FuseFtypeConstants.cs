
namespace Nofs.Fuse
{
    public class FuseFtypeConstants
    {
        // file type 'mode' bits

        public const int TYPE_MASK = 0170000;   // bitmask for the file type bitfields

        public const int TYPE_SOCKET = 0140000;   // socket
        public const int TYPE_SYMLINK = 0120000;   // symbolic link
        public const int TYPE_FILE = 0100000;   // regular file
        public const int TYPE_BLOCKDEV = 0060000;   // block device
        public const int TYPE_DIR = 0040000;   // directory
        public const int TYPE_CHARDEV = 0020000;   // character device
        public const int TYPE_FIFO = 0010000;   // fifo
    }
}
