using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Net.Utils;

namespace Nofs.Net.Fuse
{
    interface IFileDataHandler
    {
        //FuseException
        public int open(String path, int flags, FuseOpenSetter openSetter);

        //FuseException
        public int release(String path, Object fh, int flags);

        //FuseException
        public int read(String path, Object fh, ByteBuffer buf, long offset);

        //FuseException
        public int write(String path, Object fh, Boolean isWritePage, ByteBuffer buf, long offset);

        //FuseException
        public int flush(String path, Object fileHandle);

        //FuseException
        public int truncate(String path, long length);

        //Exception
        public void CleanUp();

        //FuseException
        public void Sync();
    }
}
