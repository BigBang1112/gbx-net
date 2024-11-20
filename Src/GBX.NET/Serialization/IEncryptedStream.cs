using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Serialization;

public interface IEncryptedStream
{
    void Initialize(byte[] data, uint offset, uint count);
}
