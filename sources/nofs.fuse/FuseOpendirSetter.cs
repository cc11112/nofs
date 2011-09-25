using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Fuse
{
    /**
  * A callback interface used in <code>fuse.Filesystem3.opendir()</code> method
  */
    public interface FuseOpendirSetter
    {
        /**
         * Callback for filehandle API
         * <p/>
         * @param fh the filehandle to return from <code>opendir()</code> method.
         */
        void setFh(Object fh);
    }

}
