using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Fuse.Util;

namespace Nofs.Fuse
{

    /**
     * An implementation of <code>FuseOpendirSetter</code> interface that is passed as an argument to <code>fuse.Filesystem3.opendir()</code>
     * callback method to return filehandle and open options from it.
     */
    public class FuseOpendir : Struct, FuseOpendirSetter
    {
        public Object fh;

        /**
         * Callback for filehandle API
         * <p/>
         * @param fh the filehandle to return from <code>opendir()<code> method.
         */
        public void setFh(Object fh)
        {
            this.fh = fh;
        }


        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff.Append("fh=").append(fh);

            return true;
        }
    }

}
