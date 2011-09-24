using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofs.Net.Utils;

namespace Nofs.Net.Common.Interfaces.nofs.Cache
{
    interface IFileCache
    {
        // Exception
        public void Write(ByteBuffer buffer, long offset);
        // Exception
	    public void Read(ByteBuffer buffer, long offset);
        // Exception
	    public void Truncate(long length) ;
        // Exception
	    public void Commit();
        // Exception
	    public int GetFileSize();

	    public IFileObject File();
    }
}
