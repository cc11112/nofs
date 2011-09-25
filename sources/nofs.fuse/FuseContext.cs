using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Fuse.Util;

namespace Nofs.Fuse
{

    /**
     * Java counterpart of struct fuse_context FUSE C API.
     * Every instance is filled-in with current Thread's active FUSE context which is
     * only relevant for the duration of a filesystem operation
     */
    public class FuseContext : Struct
    {
        public int uid;
        public int gid;
        public int pid;

        private FuseContext()
        {
        }


        public static FuseContext get()
        {
            FuseContext fuseContext = new FuseContext();
            fuseContext.fillInFuseContext();
            return fuseContext;
        }


        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff.Append("uid=").append(uid)
                .Append(", gid=").append(gid)
                .Append(", pid=").append(pid);

            return true;
        }

        private void fillInFuseContext()
        {
        }
    }

}
