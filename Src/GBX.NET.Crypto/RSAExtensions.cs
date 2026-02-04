#if NET5_0_OR_GREATER
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Crypto;

public static class RSAExtensions
{
    // NOTE: This method is AI generated and will need some tweaking in the future
    public static byte[] PublicDecrypt(this RSA rsa, byte[] encryptedData, byte[] publicKeyData)
    {
        try
        {
            // Try to import as PEM first
            var publicKeyString = Encoding.UTF8.GetString(publicKeyData);
            rsa.ImportFromPem(publicKeyString);
        }
        catch
        {
            try
            {
                // If PEM fails, try DER format
                rsa.ImportRSAPublicKey(publicKeyData, out _);
            }
            catch
            {
                // If both fail, try SubjectPublicKeyInfo format
                rsa.ImportSubjectPublicKeyInfo(publicKeyData, out _);
            }
        }

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

        // Remove PKCS#1 padding if present
        if (resultBytes.Length > 1 && resultBytes[0] == 0x00 && resultBytes[1] == 0x02)
        {
            // PKCS#1 v1.5 padding for encryption (0x00 0x02 ... random bytes ... 0x00 data)
            int paddingEnd = 2;
            while (paddingEnd < resultBytes.Length && resultBytes[paddingEnd] != 0x00)
                paddingEnd++;

            if (paddingEnd < resultBytes.Length)
            {
                paddingEnd++; // Skip the 0x00 separator
                return resultBytes[paddingEnd..];
            }
        }
        else if (resultBytes.Length > 1 && resultBytes[0] == 0x00 && resultBytes[1] == 0x01)
        {
            // PKCS#1 v1.5 padding for signatures (0x00 0x01 0xFF...0xFF 0x00 data)
            int paddingEnd = 2;
            while (paddingEnd < resultBytes.Length && resultBytes[paddingEnd] == 0xFF)
                paddingEnd++;

            if (paddingEnd < resultBytes.Length && resultBytes[paddingEnd] == 0x00)
            {
                paddingEnd++; // Skip the 0x00 separator
                return resultBytes[paddingEnd..];
            }
        }

        return resultBytes;
    }
}
#endif