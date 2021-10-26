using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public class NadeoPakIncPack
    {
        public byte[] Checksum { get; }
        public string Name { get; }
        public string AuthorLogin { get; }
        public string AuthorNickname { get; }
        public string AuthorZone { get; }
        public string AuthorExtraInfo { get; }
        public string ManialinkUrl { get; }
        public string Name2 { get; }
        public DateTime CreationDate { get; }
        public int IncludeDepth { get; }

        public NadeoPakIncPack(byte[] checksum, string name, string authorLogin, string authorNickname, string authorZone, string authorExtraInfo, string manialinkUrl, string name2, DateTime creationDate, int includeDepth)
        {
            Checksum = checksum;
            Name = name;
            AuthorLogin = authorLogin;
            AuthorNickname = authorNickname;
            AuthorZone = authorZone;
            AuthorExtraInfo = authorExtraInfo;
            ManialinkUrl = manialinkUrl;
            Name2 = name2;
            CreationDate = creationDate;
            IncludeDepth = includeDepth;
    }
    }
}
