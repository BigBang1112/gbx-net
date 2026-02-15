#if NET5_0_OR_GREATER
using System.Security.Cryptography;

namespace GBX.NET.Crypto;

public static class RSAExtra
{
    public static byte[] PublicDecrypt(byte[] encryptedData, string publicKeyData)
    {
        using var rsa = RSA.Create();

        rsa.ImportFromPem(publicKeyData);

        // Get the RSA parameters
        var parameters = rsa.ExportParameters(false);

        // Perform manual RSA operation: c^e mod n
        // Convert encrypted data to BigInteger (big-endian, unsigned)
        var c = new System.Numerics.BigInteger(encryptedData.AsEnumerable().Reverse().Concat(new byte[] { 0 }).ToArray());
        var e = new System.Numerics.BigInteger(parameters.Exponent!.AsEnumerable().Reverse().Concat(new byte[] { 0 }).ToArray());
        var n = new System.Numerics.BigInteger(parameters.Modulus!.AsEnumerable().Reverse().Concat(new byte[] { 0 }).ToArray());

        var result = System.Numerics.BigInteger.ModPow(c, e, n);

        // Convert back to bytes (big-endian)
        var resultBytes = result.ToByteArray();
        if (resultBytes[^1] == 0) // Remove sign byte if present
        {
            Array.Resize(ref resultBytes, resultBytes.Length - 1);
        }
        Array.Reverse(resultBytes); // Convert to big-endian

        // Ensure the result is the same length as the modulus
        var modulusSize = parameters.Modulus!.Length;
        if (resultBytes.Length < modulusSize)
        {
            var paddedResult = new byte[modulusSize];
            Array.Copy(resultBytes, 0, paddedResult, modulusSize - resultBytes.Length, resultBytes.Length);
            resultBytes = paddedResult;
        }

        // PKCS#1 v1.5 padding for signatures (0x00 0x01 0xFF...0xFF 0x00 data)
        int paddingEnd = 2;
        while (paddingEnd < resultBytes.Length && resultBytes[paddingEnd] == 0xFF)
            paddingEnd++;

        if (paddingEnd < resultBytes.Length && resultBytes[paddingEnd] == 0x00)
        {
            paddingEnd++; // Skip the 0x00 separator
            return resultBytes[paddingEnd..];
        }

        return resultBytes;
    }

    public static byte[] PrivateEncrypt(byte[] dataToEncrypt, byte[] privateKeyData)
    {
        using var rsa = RSA.Create();

        rsa.ImportPkcs8PrivateKey(privateKeyData, out _);

        // Get the RSA parameters (true to include the private exponent 'D')
        var parameters = rsa.ExportParameters(true);
        var modulusSize = parameters.Modulus!.Length;

        // Ensure data isn't too large for the key 
        // PKCS#1 v1.5 requires at least 11 bytes of padding (0x00 0x01 + 8 bytes of 0xFF + 0x00)
        if (dataToEncrypt.Length > modulusSize - 11)
            throw new ArgumentException($"Data is too long. Max length is {modulusSize - 11} bytes.");

        // Apply PKCS#1 v1.5 padding for signatures (0x00 0x01 0xFF...0xFF 0x00 data)
        var paddedData = new byte[modulusSize];
        paddedData[0] = 0x00;
        paddedData[1] = 0x01;

        int paddingStringLength = modulusSize - dataToEncrypt.Length - 3;
        for (int i = 0; i < paddingStringLength; i++)
        {
            paddedData[2 + i] = 0xFF;
        }

        paddedData[2 + paddingStringLength] = 0x00;
        Array.Copy(dataToEncrypt, 0, paddedData, 2 + paddingStringLength + 1, dataToEncrypt.Length);

        // Perform manual RSA operation: c = m^d mod n
        // Convert padded data to BigInteger (big-endian, unsigned)
        var m = new System.Numerics.BigInteger(paddedData.AsEnumerable().Reverse().Concat(new byte[] { 0 }).ToArray());
        var d = new System.Numerics.BigInteger(parameters.D!.AsEnumerable().Reverse().Concat(new byte[] { 0 }).ToArray());
        var n = new System.Numerics.BigInteger(parameters.Modulus!.AsEnumerable().Reverse().Concat(new byte[] { 0 }).ToArray());

        var result = System.Numerics.BigInteger.ModPow(m, d, n);

        // Convert back to bytes (big-endian)
        var resultBytes = result.ToByteArray();
        if (resultBytes[^1] == 0) // Remove sign byte if present
        {
            Array.Resize(ref resultBytes, resultBytes.Length - 1);
        }
        Array.Reverse(resultBytes); // Convert to big-endian

        // Ensure the result is the same length as the modulus
        if (resultBytes.Length < modulusSize)
        {
            var paddedResult = new byte[modulusSize];
            Array.Copy(resultBytes, 0, paddedResult, modulusSize - resultBytes.Length, resultBytes.Length);
            resultBytes = paddedResult;
        }

        return resultBytes;
    }
}
#endif