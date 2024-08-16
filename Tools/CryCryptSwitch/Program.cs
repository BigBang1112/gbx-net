using GBX.NET;
using GBX.NET.Crypto;
using GBX.NET.LZO;
using System.Text;

Gbx.LZO = new Lzo();

foreach (var arg in args)
{
    if (!File.Exists(arg))
    {
        Console.WriteLine($"File not found: {arg}");
        continue;
    }

    using var fsIn = File.OpenRead(arg);

    if (IsTextFile(fsIn))
    {
        fsIn.Position = 0;

        using var r = new StreamReader(fsIn);
        using var fsOut = File.Create(Path.GetFileNameWithoutExtension(arg) + ".cry");
        Cry.Encrypt(fsOut, r.ReadToEnd());
        Console.WriteLine("Encrypted!");
    }
    else
    {
        fsIn.Position = 0;

        var decrypted = Cry.Decrypt(fsIn);
        Console.WriteLine(decrypted);
        File.WriteAllText(Path.GetFileNameWithoutExtension(arg) + ".txt", decrypted);
    }
}

static bool IsTextFile(Stream stream)
{
    try
    {
        using var r = new StreamReader(stream, Encoding.UTF8, true, 1024, true);

        while (!r.EndOfStream)
        {
            int charValue = r.Read();
            if (charValue == 0)
            {
                // file has null byte, considered binary
                return false;
            }
        }

        // file doesn't contain null bytes or invalid UTF-8 sequences, considered text
        return true;
    }
    catch (DecoderFallbackException)
    {
        // invalid UTF-8 sequence, considered binary
        return false;
    }
}