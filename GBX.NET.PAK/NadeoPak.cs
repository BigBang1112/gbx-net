using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace GBX.NET.PAK
{
    public class NadeoPak : IDisposable
    {
        private BlowfishCBCStream blowfish;
        private NadeoPakFile[] files;

        public byte[] Key { get; }
        public int Version { get; private set; }
        public int DataStart { get; private set; }
        public List<NadeoPakFolder> Folders { get; private set; }
        public List<NadeoPakFile> Files { get; private set; }

        internal Stream Stream { get; private set; }

        public NadeoPak(byte[] key)
        {
            Key = key;
            Folders = new List<NadeoPakFolder>();
            Files = new List<NadeoPakFile>();
        }

        internal void Read(Stream stream)
        {
            Stream = stream;

            using (var r = new GameBoxReader(stream))
                Read(r);
        }

        private void Read(GameBoxReader r)
        {
            if (!r.HasMagic("NadeoPak")) throw new Exception("Not a pak file.");

            Version = r.ReadInt32();
            var headerIV = r.ReadUInt64();

            using (blowfish = new BlowfishCBCStream(r.BaseStream, Key, headerIV))
            using (var rr = new GameBoxReader(blowfish))
                ReadEncrypted(rr);
        }

        private void ReadEncrypted(GameBoxReader r)
        {
            var headerMD5 = r.ReadBytes(16);
            var gbxHeadersStart = r.ReadInt32(); // offset to metadata section
            DataStart = r.ReadInt32();

            if (Version >= 2)
            {
                var gbxHeadersSize = r.ReadInt32();
                var gbxHeadersComprSize = r.ReadInt32();
            }

            if (Version >= 3)
                r.ReadBytes(16); // unused

            var flags = r.ReadInt32();

            var folders = ReadFolders(r);

            ReadFiles(r, folders);
        }

        private NadeoPakFolder[] ReadFolders(GameBoxReader r)
        {
            var numFolders = r.ReadInt32();
            var folders = new NadeoPakFolder[numFolders];

            for (var i = 0; i < numFolders; i++)
            {
                var parentFolderIndex = r.ReadInt32(); // index into folders; -1 if this is a root folder
                var name = r.ReadString();

                var folder = new NadeoPakFolder(name, parentFolderIndex == -1 ? null : folders[parentFolderIndex]);

                if (parentFolderIndex == -1)
                {
                    Folders.Add(folder);
                }
                else
                {
                    folders[parentFolderIndex].Folders.Add(folder);
                }

                folders[i] = folder;
            }

            if (folders.Length > 2 && folders[2].Name.Length > 4)
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(folders[2].Name);
                blowfish.Initialize(nameBytes, 4, 4);
            }

            return folders;
        }

        private void ReadFiles(GameBoxReader r, NadeoPakFolder[] folders)
        {
            var numFiles = r.ReadInt32();
            var files = new NadeoPakFile[numFiles];

            for (var i = 0; i < numFiles; i++)
            {
                var folderIndex = r.ReadInt32(); // index into folders
                var name = r.ReadString();
                var unknown = r.ReadInt32();
                var uncompressedSize = r.ReadInt32();
                var compressedSize = r.ReadInt32();
                var offset = r.ReadInt32();
                var classID = CMwNod.Remap(r.ReadUInt32()); // indicates the type of the file
                var flags = r.ReadUInt64();

                var folder = folders.ElementAtOrDefault(folderIndex);

                var file = new NadeoPakFile(this, folder, name, uncompressedSize, compressedSize, offset, classID, flags)
                {
                    U01 = unknown
                };

                files[i] = file;

                if (folder == null)
                    Files.Add(file);
                else
                    folder.Files.Add(file);
            }

            this.files = files;
        }

        public NadeoPakFile[] GetFiles()
        {
            return files;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">Don't close this Stream if you want to read file contents!</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static NadeoPak Parse(Stream stream, byte[] key)
        {
            var pak = new NadeoPak(key);
            pak.Read(stream);
            return pak;
        }

        public static NadeoPak Parse(string fileName, byte[] key)
        {
            var stream = File.OpenRead(fileName);
            return Parse(stream, key);
        }

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}
