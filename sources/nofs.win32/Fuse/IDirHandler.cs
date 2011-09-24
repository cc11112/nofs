using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Net.Fuse
{
    interface IDirHandler
    {
        //   FuseException:
        int getdir(String path, FuseDirFiller filler);
        //   FuseException:
	    int mkdir(String path, int mode);
            //   FuseException:
	    int rmdir(String path);
    }
}
