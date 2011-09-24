using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Net.Fuse
{
    interface IFileHandler
    {
        
        //FuseException
        int rename(String from, String to);

        //FuseException
	    int unlink(String path);

        //FuseException
	    int mknod(String path, int mode, int rdev);
    }
}
