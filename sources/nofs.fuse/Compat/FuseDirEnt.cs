using System;
using System.Text;

/**
 *   FUSE-J: Java bindings for FUSE (Filesystem in Userspace by Miklos Szeredi (mszeredi@inf.bme.hu))
 *
 *   Copyright (C) 2003 Peter Levart (peter@select-tech.si)
 *
 *   This program can be distributed under the terms of the GNU LGPL.
 *   See the file COPYING.LIB
 */

namespace Nofs.Fuse.Compat
{

    /**
     * This is a String level API directory entry used in fuse.compat.Filesystem1 and fuse.compat.Filesystem2 compatibility APIs
     */
    public class FuseDirEnt : FuseFtype
    {
        public String name;

        // CHANGE-22: inode added
        public int inode;

        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(isPrefixed ? ", " : " ")
                .Append("name='").
                Append(name)
                .Append("'")
                .Append("inode='")
                .Append(inode)
                .Append("'");

            return base.appendAttributes(buff, true);
        }
    }
}
