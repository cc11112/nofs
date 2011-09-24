
using System;

namespace Nofs.Fuse
{
    public interface FuseOpenSetter
    {
        /**
        * Callback for filehandle API
        * <p/>
        * @param fh the filehandle to return from <code>open()<code> method.
        */
        void setFh(object fh);

        /**
         * Sets/gets the direct_io FUSE option for this opened file
         */
        Boolean isDirectIO();

        void setDirectIO(Boolean directIO);

        /**
         * Sets/gets keep_cache FUSE option for this opened file
         */
        Boolean isKeepCache();

        void setKeepCache(Boolean keepCache);
    }
}
