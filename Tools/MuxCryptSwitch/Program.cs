using GBX.NET.Crypto;
using System.Security.Cryptography;
using System.Text;

foreach (var arg in args)
{
    if (!File.Exists(arg))
    {
        Console.WriteLine($"File not found: {arg}");
        continue;
    }

    using var fsIn = File.OpenRead(arg);
    var magic = new byte[9];
    fsIn.ReadExactly(magic);
    fsIn.Position = 0;

    if (Encoding.ASCII.GetString(magic) == "NadeoFile")
    {
        using var fsOut = File.Create(Path.GetFileNameWithoutExtension(arg) + ".ogg");
        using var muxStream = Mux.Decrypt(fsIn);

        muxStream.CopyTo(fsOut);

        Console.WriteLine("Decrypted!");
    }
    else
    {
        var salt = RandomNumberGenerator.GetInt32(int.MaxValue);
        Console.WriteLine($"Salt: {salt}");

        using var fsOut = File.Create(Path.GetFileNameWithoutExtension(arg) + ".mux");
        using var muxStream = Mux.Encrypt(fsOut, salt);

        fsIn.CopyTo(muxStream);

        Console.WriteLine("Encrypted!");
    }
}