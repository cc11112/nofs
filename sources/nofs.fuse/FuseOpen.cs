using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Fuse.Util;

namespace Nofs.Fuse
{
    /**
     * An implementation of <code>FuseOpenSetter</code> interface that is passed as an argument to <code>fuse.Filesystem3.open()</code>
     * callback method to return filehandle and open options from it.
     */
    public class FuseOpen : Struct, FuseOpenSetter
    {
        public Object fh;
        public Boolean directIO;
        public Boolean keepCache;

        /**
         * Callback for filehandle API
         * <p/>
         * @param fh the filehandle to return from <code>open()<code> method.
         */
        public void setFh(Object fh)
        {
            this.fh = fh;
        }

        /**
         * Sets/gets the direct_io FUSE option for this opened file
         */
        public Boolean isDirectIO()
        {
            return directIO;
        }

        public void setDirectIO(Boolean directIO)
        {
            this.directIO = directIO;
        }

        /**
         * Sets/gets keep_cache FUSE option for this opened file
         */
        public Boolean isKeepCache()
        {
            return keepCache;
        }

        public void setKeepCache(Boolean keepCache)
        {
            this.keepCache = keepCache;
        }

        protected override Boolean appendAttributes(StringBuilder buff, Boolean isPrefixed)
        {
            buff.Append(base.appendAttributes(buff, isPrefixed) ? ", " : " ");

            buff.Append("fh=").append(fh)
                .Append(", directIO=").append(directIO)
                .Append(", keepCache=").append(keepCache);

            return true;
        }
    }

}
