using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nofs.Net.Cache.Impl
{
    public class DirectFileCache : IFileCache {

	private IFileObject _file;
	
	public DirectFileCache(IFileObject file) {
		_file = file;
	}
	
	public void Commit() throws Exception {
	}

	public IFileObject File() {
		return _file;
	}

	public int GetFileSize() throws Exception {
		return (int)_file.GetReadWriteInterface().DataSize();
	}

	public void Read(ByteBuffer buffer, long offset) throws Exception {
		int readMax = buffer.limit() - buffer.position();
		_file.GetReadWriteInterface().Read(buffer, offset, readMax);
	}

	public void Truncate(long length) throws Exception {
		_file.GetReadWriteInterface().Truncate(length);
	}

	public void Write(ByteBuffer buffer, long offset) throws Exception {
		int writeMax = buffer.limit() - buffer.position();
		_file.GetReadWriteInterface().Write(buffer, offset, writeMax);
	}
}

}
