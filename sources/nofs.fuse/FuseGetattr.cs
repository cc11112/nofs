using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse
{

    public class FuseGetattr : FuseFtype, FuseGetattrSetter
    {
        public long inode;
        // public int mode; in superclass
        public int nlink;
        public int uid;
        public int gid;
        public int rdev;
        public long size;
        public long blocks;
        public int atime;
        public int mtime;
        public int ctime;

        //
        // FuseGetattrSetter implementation

        public void set(long inode, int mode, int nlink, int uid, int gid, int rdev, long size, long blocks, int atime, int mtime, int ctime)
        {
            this.inode = inode;
            this.mode = mode;
            this.nlink = nlink;
            this.uid = uid;
            this.gid = gid;
            this.rdev = rdev;
            this.size = size;
            this.blocks = blocks;
            this.atime = atime;
            this.mtime = mtime;
            this.ctime = ctime;
        }

        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff
               .Append("inode=").Append(inode)
               .Append(", nlink=").Append(nlink)
               .Append(", uid=").Append(uid)
               .Append(", gid=").Append(gid)
               .Append(", rdev=").Append(rdev)
               .Append(", size=").Append(size)
               .Append(", blocks=").Append(blocks)
               .Append(", atime=").Append(atime)
               .Append(", mtime=").Append(mtime)
               .Append(", ctime=").Append(ctime);

            return true;
        }
    }

}
