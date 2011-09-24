using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse
{
    public class FuseStatConstants : FuseFtypeConstants
    {
        // additional mode bits

        public const int MODE_MASK = 0007777;   // mode bits mask

        public const int SUID_BIT = 0004000;   // set UID bit
        public const int SGID_BIT = 0002000;   // set GID bit
        public const int STICKY_BIT = 0001000;   // sticky bit

        public const int OWNER_MASK = 0000700;   // mask for file owner permissions
        public const int OWNER_READ = 0000400;   // owner has read permission
        public const int OWNER_WRITE = 0000200;   // owner has write permission
        public const int OWNER_EXECUTE = 0000100;   // owner has execute permission

        public const int GROUP_MASK = 0000070;   // mask for group permissions
        public const int GROUP_READ = 0000040;   // group has read permission
        public const int GROUP_WRITE = 0000020;   // group has write permission
        public const int GROUP_EXECUTE = 0000010;   // group has execute permission

        public const int OTHER_MASK = 0000007;   // mask for permissions for others
        public const int OTHER_READ = 0000004;   // others have read permission
        public const int OTHER_WRITE = 0000002;   // others have write permisson
        public const int OTHER_EXECUTE = 0000001;   // others have execute permission
    }
}
