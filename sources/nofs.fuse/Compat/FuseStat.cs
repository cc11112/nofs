using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse.Compat
{
 /**
 *   FUSE-J: Java bindings for FUSE (Filesystem in Userspace by Miklos Szeredi (mszeredi@inf.bme.hu))
 *
 *   Copyright (C) 2003 Peter Levart (peter@select-tech.si)
 *
 *   This program can be distributed under the terms of the GNU LGPL.
 *   See the file COPYING.LIB
 */

    public class FuseStat : FuseFtype
    {
        public int nlink;
        public int uid;
        public int gid;
        public long size;
        public int atime;
        public int mtime;
        public int ctime;
        public int blocks;

        // inode support fix by Edwin Olson <eolson@mit.edu>
        public long inode;

        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff.Append("nlink=").Append(nlink)
                .Append(", uid=").Append(uid)
                .Append(", gid=").Append(gid)
                .Append(", size=").Append(size)
                .Append(", atime=").Append(atime)
                .Append(", mtime=").Append(mtime)
                .Append(", ctime=").Append(ctime)
                .Append(", blocks=").Append(blocks)
                .Append(", inode=").Append(inode);

            return true;
        }
    }
}
