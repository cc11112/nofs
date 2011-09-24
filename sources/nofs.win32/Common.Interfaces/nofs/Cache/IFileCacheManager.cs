using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Net.Common.Interfaces.nofs.Cache
{
    interface IFileCacheManager
    {
        void Flush(UUID objectID);
        //throws Exception
	    IFileCache GetFileCache(IFileObject file);
	    void Deallocate(IFileCache cache);
	    void DeallocateIfNotDirty(IFileCache cache);
    }
}
