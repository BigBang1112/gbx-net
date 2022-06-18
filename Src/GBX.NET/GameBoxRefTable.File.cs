using System.Security.Cryptography;
using System.Text;

namespace GBX.NET;

public partial class GameBoxRefTable
{
    public record File(int Flags,
                       string? FileName,
                       int? ResourceIndex,
                       int NodeIndex,
                       bool? UseFile,
                       int FolderIndex)
    {
        public override string ToString()
        {
            return FileName ?? string.Empty;
        }
    }
}