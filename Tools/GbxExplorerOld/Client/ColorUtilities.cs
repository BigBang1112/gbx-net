using GBX.NET;

namespace GbxExplorerOld.Client;

public static class ColorUtilities
{
    public static byte[] Color2dArrayToByteArray(Color[,] val, out int width, out int height)
    {
        width = val.GetLength(0);
        height = val.GetLength(1);

        var byteArray = new byte[width * height * 4];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                byteArray[(y * width + x) * 4] = (byte)val[x, y].R;
                byteArray[(y * width + x) * 4 + 1] = (byte)val[x, y].G;
                byteArray[(y * width + x) * 4 + 2] = (byte)val[x, y].B;
                byteArray[(y * width + x) * 4 + 3] = (byte)val[x, y].A;
            }
        }

        return byteArray;
    }
}
