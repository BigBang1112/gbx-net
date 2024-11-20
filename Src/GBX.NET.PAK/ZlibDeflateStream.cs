using System.Runtime.InteropServices;

namespace GBX.NET.PAK;

internal class ZlibDeflateStream : Stream
{
    #region zlib interop

    [StructLayout(LayoutKind.Sequential)]
    private class z_stream
    {
        public IntPtr next_in;
        public int avail_in;
        public int total_in;

        public IntPtr next_out;
        public int avail_out;
        public int total_out;

        public IntPtr msg;
        public IntPtr state;

        public IntPtr zalloc;
        public IntPtr zfree;
        public IntPtr opaque;

        public int data_type;
        public uint adler;
        public uint reserved;
    }

    private enum ZFlushType
    {
        Z_NO_FLUSH = 0,
        Z_PARTIAL_FLUSH = 1,
        Z_SYNC_FLUSH = 2,
        Z_FULL_FLUSH = 3,
        Z_FINISH = 4,
        Z_BLOCK = 5,
        Z_TREES = 6
    }

    private enum ZStatus
    {
        Z_OK = 0,
        Z_STREAM_END = 1,
        Z_NEED_DICT = 2,
        Z_ERRNO = -1,
        Z_STREAM_ERROR = -2,
        Z_DATA_ERROR = -3,
        Z_MEM_ERROR = -4,
        Z_BUF_ERROR = -5,
        Z_VERSION_ERROR = -6
    }

    [DllImport("zlib1.dll")]
    private static extern ZStatus deflateInit_(z_stream stream, int level, string version, int stream_size);

    [DllImport("zlib1.dll")]
    private static extern ZStatus deflate(z_stream stream, ZFlushType flush);

    [DllImport("zlib1.dll")]
    private static extern ZStatus deflateEnd(z_stream stream);

    [DllImport("zlib1.dll")]
    private static extern ZStatus inflateInit_(z_stream stream, string version, int stream_size);

    [DllImport("zlib1.dll")]
    private static extern ZStatus inflate(z_stream stream, ZFlushType flush);

    [DllImport("zlib1.dll")]
    private static extern ZStatus inflateEnd(z_stream stream);

    #endregion

    private const int COMPRESSED_BLOCK_SIZE = 0x100;
    private const int UNCOMPRESSED_BLOCK_SIZE = 0x400;

    private Stream _innerStream;
    private bool _compressing;
    private long _position;

    private z_stream _zlibStream;
    private byte[] _compressedBuffer;
    private byte[] _uncompressedBuffer;
    private IntPtr _zlibCompressedMem;
    private IntPtr _zlibUncompressedMem;
    private int _compressedIndex;
    private int _uncompressedIndex;
    private int _uncompressedSize;

    private bool _disposed;

    public ZlibDeflateStream(Stream innerStream, bool compressing)
    {
        _innerStream = innerStream;
        _compressing = compressing;
        _position = 0;

        _zlibStream = new z_stream();
        _compressedBuffer = new byte[COMPRESSED_BLOCK_SIZE];
        _uncompressedBuffer = new byte[UNCOMPRESSED_BLOCK_SIZE];
        _zlibCompressedMem = Marshal.AllocHGlobal(COMPRESSED_BLOCK_SIZE);
        _zlibUncompressedMem = Marshal.AllocHGlobal(UNCOMPRESSED_BLOCK_SIZE);
        if (compressing)
        {
            VerifyStatus(deflateInit_(_zlibStream, 8, "1.2.5", Marshal.SizeOf(_zlibStream)));
        }
        else
        {
            VerifyStatus(inflateInit_(_zlibStream, "1.2.5", Marshal.SizeOf(_zlibStream)));
        }
    }

    public override bool CanRead
    {
        get { return !_compressing; }
    }

    public override bool CanSeek
    {
        get { return false; }
    }

    public override bool CanWrite
    {
        get { return _compressing; }
    }

    public override void Flush()
    {

    }

    public override long Length
    {
        get { throw new NotSupportedException(); }
    }

    public override long Position
    {
        get
        {
            return _position;
        }
        set
        {
            throw new NotSupportedException();
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_compressing)
            throw new NotSupportedException();

        int left = count;
        while (left > 0)
        {
            if (_uncompressedIndex == _uncompressedSize)
            {
                _uncompressedIndex = 0;
                _uncompressedSize = 0;
                while (_uncompressedSize < UNCOMPRESSED_BLOCK_SIZE)
                {
                    if (_zlibStream.avail_in == 0)
                    {
                        int inChunkSize = _innerStream.Read(_compressedBuffer, 0, COMPRESSED_BLOCK_SIZE);
                        SetInflateInput(_compressedBuffer, 0, inChunkSize);
                    }
                    bool streamEnd;
                    _uncompressedSize += Inflate(_uncompressedBuffer, _uncompressedSize,
                        UNCOMPRESSED_BLOCK_SIZE - _uncompressedSize, out streamEnd);
                    if (streamEnd)
                        break;
                }
            }
            int outChunkSize = Math.Min(_uncompressedSize - _uncompressedIndex, left);
            Array.Copy(_uncompressedBuffer, _uncompressedIndex, buffer, offset, outChunkSize);
            _uncompressedIndex += outChunkSize;
            _position += outChunkSize;
            offset += outChunkSize;
            left -= outChunkSize;
        }

        return count - left;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!_compressing)
            throw new NotSupportedException();

        int left = count;
        while (left > 0)
        {
            int inChunkSize = Math.Min(UNCOMPRESSED_BLOCK_SIZE - _uncompressedIndex, left);
            Array.Copy(buffer, offset, _uncompressedBuffer, _uncompressedIndex, inChunkSize);
            _uncompressedIndex += inChunkSize;
            _position += inChunkSize;
            offset += inChunkSize;
            left -= inChunkSize;

            if (_uncompressedIndex == UNCOMPRESSED_BLOCK_SIZE)
            {
                SetDeflateInput(_uncompressedBuffer, 0, UNCOMPRESSED_BLOCK_SIZE);
                FlushDeflateOutput(false);
                _uncompressedIndex = 0;
            }
        }
    }

    private void FlushDeflateOutput(bool finish)
    {
        bool finished = false;
        while (!NeedInput() || (finish && !finished))
        {
            int deflated = Deflate(_compressedBuffer, 0, COMPRESSED_BLOCK_SIZE, finish, out finished);
            _innerStream.Write(_compressedBuffer, 0, deflated);
        }
    }

    private bool NeedInput()
    {
        return _zlibStream.avail_in == 0;
    }

    private void SetInflateInput(byte[] buffer, int offset, int length)
    {
        _zlibStream.avail_in = length;
        _zlibStream.next_in = _zlibCompressedMem;
        Marshal.Copy(buffer, offset, _zlibCompressedMem, length);
    }

    private int Inflate(byte[] buffer, int offset, int length, out bool streamEnd)
    {
        streamEnd = false;

        if (_zlibStream.avail_in == 0)
            return 0;

        _zlibStream.avail_out = length;
        _zlibStream.next_out = _zlibUncompressedMem;
        ZStatus status = VerifyStatus(inflate(_zlibStream, ZFlushType.Z_SYNC_FLUSH));
        Marshal.Copy(_zlibUncompressedMem, buffer, offset, length);
        streamEnd = (status == ZStatus.Z_STREAM_END);
        return (int)((ulong)_zlibStream.next_out - (ulong)_zlibUncompressedMem);
    }

    private void SetDeflateInput(byte[] buffer, int offset, int length)
    {
        _zlibStream.avail_in = length;
        _zlibStream.next_in = _zlibUncompressedMem;
        Marshal.Copy(buffer, offset, _zlibUncompressedMem, length);
    }

    private int Deflate(byte[] buffer, int offset, int length, bool finish, out bool finished)
    {
        finished = false;
        _zlibStream.avail_out = length;
        _zlibStream.next_out = _zlibCompressedMem;
        if (finish)
        {
            ZStatus status = VerifyStatus(deflate(_zlibStream, ZFlushType.Z_FINISH));
            finished = (status == ZStatus.Z_STREAM_END);
        }
        else
        {
            if (_zlibStream.avail_in == 0)
                return 0;

            VerifyStatus(deflate(_zlibStream, ZFlushType.Z_SYNC_FLUSH));
        }
        int deflated = (int)_zlibStream.next_out - (int)_zlibCompressedMem;
        Marshal.Copy(_zlibCompressedMem, buffer, offset, deflated);
        return deflated;
    }

    private static ZStatus VerifyStatus(ZStatus status)
    {
        switch (status)
        {
            case ZStatus.Z_BUF_ERROR:
                throw new Exception("Buffer error");

            case ZStatus.Z_DATA_ERROR:
                throw new Exception("Data error");

            case ZStatus.Z_ERRNO:
                throw new Exception("Filesystem error");

            case ZStatus.Z_MEM_ERROR:
                throw new Exception("Memory error");

            case ZStatus.Z_STREAM_ERROR:
                throw new Exception("Stream error");

            case ZStatus.Z_VERSION_ERROR:
                throw new Exception("Version error");
        }
        return status;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_compressing)
                {
                    if (_uncompressedIndex > 0)
                    {
                        SetDeflateInput(_uncompressedBuffer, 0, _uncompressedIndex);
                        FlushDeflateOutput(false);
                    }
                    FlushDeflateOutput(true);
                }
                _innerStream.Dispose();
            }

            if (_compressing)
                VerifyStatus(deflateEnd(_zlibStream));
            else
                VerifyStatus(inflateEnd(_zlibStream));

            Marshal.FreeHGlobal(_zlibCompressedMem);
            Marshal.FreeHGlobal(_zlibUncompressedMem);
            _zlibCompressedMem = IntPtr.Zero;
            _zlibUncompressedMem = IntPtr.Zero;

            _disposed = true;
        }
        base.Dispose(disposing);
    }
}