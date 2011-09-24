using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Net.Cache.Impl
{
    public class FileCache : IFileCache {
	private final IFileObject _file;
	private ITranslatorStrategy _translator = null;
	private IMethodFilter _methodFilter;
	private MemoryBuffer _cache;
	private final int _blockSize;
	private FileCacheManager _manager;
	private boolean _dirty;
	private LogManager _log;

	public FileCache(LogManager log, IMethodFilter methodFilter, FileCacheManager manager, IFileObject file, int blockSize) {
		_log = log;
		_dirty = false;
		_manager = manager;
		_file = file;
		_blockSize = blockSize;
		_methodFilter = methodFilter;
	}

	public boolean IsDirty() {
		return _dirty;
	}

	public IFileObject File() {
		return _file;
	}

	private ITranslatorStrategy Translator() throws Exception {
		if(_translator == null) {
			_translator = new TranslatorFactory(_methodFilter).CreateTranslator(_file);
		}
		return _translator;
	}

	private MemoryBuffer Cache() throws Exception {
		if(_cache == null) {
			_cache = new MemoryBuffer(_blockSize);
			String values = Translator().Serialize(_file);
			int readCount = 0;
			for(byte value : values.getBytes()) {
				_cache.put(value);
				readCount+=1;
			}
			_log.LogInfo("read " + readCount + " bytes into cache for file: " + File().GetName());
			_cache.setPosition(0);
		}
		return _cache;
	}

	//@Override
	public int GetFileSize() throws Exception {
		return Cache().getSize();
	}

	//@Override
	public void Commit() throws Exception {
		try {
			if(_cache != null && _dirty) {
				StringBuffer buffer = new StringBuffer();
				Cache().setPosition(0);
				for(byte value : Cache()) {
					buffer.append((char)value);
				}
				String data = buffer.toString();
				_log.LogInfo("commit " + data.length() + " bytes into " + File().GetName());
				Translator().DeserializeInto(data, _file);
				_manager.CommitToDB(_file);
			}
		} finally {
			_cache = null;
		}
	}

	//@Override
	public void Read(ByteBuffer buffer, long offset) throws Exception {
		Cache().setPosition((int)offset);
		int readMax = buffer.limit() - buffer.position();
		int bufferMax = Cache().getSize();
		int readCount = 0;
		for(int i = 0; i < readMax && i < bufferMax; i++) {
			readCount += 1;
			buffer.put(_cache.get());
		}
		_log.LogInfo("read " + readCount + " bytes from cache");
	}

	//@Override
	public void Truncate(long length) throws Exception {
		_dirty = true;
		Cache().Trim((int)length);
	}

	//@Override
	public void Write(ByteBuffer buffer, long offset) throws Exception {
		_dirty = true;
		Cache().setPosition((int)offset);
		int writeMax = buffer.limit() - buffer.position();
		int writeCount = 0;
		for(int i = 0; i < writeMax; i++) {
			writeCount += 1;
			Cache().put(buffer.get());
		}
		_log.LogInfo("wrote " + writeCount + " bytes into cache");
	}

}

}
