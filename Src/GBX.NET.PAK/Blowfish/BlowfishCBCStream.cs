using Simias.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.PAK;

public class BlowfishCBCStream : Stream
{
    private readonly Stream stream;

    private readonly Blowfish _blowfish;
    private ulong _iv;
    private ulong _ivXor;
    private byte[] _buffer;
    private uint _bufferIndex;
    private uint _totalIndex;

    public BlowfishCBCStream(Stream stream, byte[] key, ulong iv)
    {
        this.stream = stream;

        _iv = iv;
        _blowfish = new Blowfish(key);
        _buffer = new byte[8];
        _bufferIndex = 0;
        _totalIndex = 0;
    }

    public ulong IV
    {
        get { return _iv; }
        set { _iv = value; }
    }

    public override long Position
    {
        get { return _totalIndex; }
        set { throw new NotSupportedException(); }
    }

    public override long Length
    {
        get { return stream.Length; }
    }

    public override bool CanSeek
    {
        get { return false; }
    }

    public override bool CanRead => stream.CanRead;

    public override bool CanWrite => stream.CanWrite;

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public void Initialize(byte[] data, uint offset, uint count)
    {
        for (int i = 0; i < count; i++)
        {
            uint lopart = (uint)(_ivXor & 0xFFFFFFFF);
            uint hipart = (uint)(_ivXor >> 32);
            lopart = (uint)(data[offset + i] | 0xAA) ^ (uint)((lopart << 13) | (hipart >> 19));
            hipart = (uint)((_ivXor << 13) >> 32);
            _ivXor = ((ulong)hipart << 32) | lopart;
        }
    }

    public void FinishWriting()
    {
        byte[] buf = new byte[] { 0 };
        while (_bufferIndex % 8 != 0)
        {
            Write(buf, 0, 1);
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_totalIndex == 0)
        {
            _iv ^= _ivXor;
            _ivXor = 0;
        }

        for (int i = 0; i < count; i++)
        {
            if (_bufferIndex % 8 == 0)
            {
                if (_bufferIndex == 0x100)
                {
                    _iv ^= _ivXor;
                    _ivXor = 0;
                    _bufferIndex = 0;
                }

                stream.Read(_buffer, 0, 8);
                ulong nextIV = BitConverter.ToUInt64(_buffer, 0);
                _blowfish.Decipher(_buffer, 8);
                ulong block = BitConverter.ToUInt64(_buffer, 0);
                block ^= _iv;
                _buffer = BitConverter.GetBytes(block);
                _iv = nextIV;
            }
            buffer[offset + i] = _buffer[_bufferIndex % 8];
            _bufferIndex++;
            _totalIndex++;
        }
        return count;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (_totalIndex == 0)
        {
            _iv ^= _ivXor;
            _ivXor = 0;
        }

        for (int i = 0; i < count; i++)
        {
            _buffer[_bufferIndex % 8] = buffer[offset + i];
            _bufferIndex++;
            _totalIndex++;

            if (_bufferIndex % 8 == 0)
            {
                ulong block = BitConverter.ToUInt64(_buffer, 0);
                block ^= _iv;
                _buffer = BitConverter.GetBytes(block);

                _blowfish.Encipher(_buffer, 8);
                stream.Write(_buffer, 0, 8);
                _iv = BitConverter.ToUInt64(_buffer, 0);

                if (_bufferIndex == 0x100)
                {
                    _iv ^= _ivXor;
                    _ivXor = 0;
                    _bufferIndex = 0;
                }
            }
        }
    }

    public override void Flush()
    {
        stream.Flush();
    }

    public override void SetLength(long value)
    {
        stream.SetLength(value);
    }
}
