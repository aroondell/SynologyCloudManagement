using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalStorageManager
{
    class VoiceFile
    {
        string OutputFile;

        public string MergeWavFiles(DateTime date, FileInfo[] files)
        {
            string DirectoryPath = files.Select(a => a.DirectoryName).FirstOrDefault();
            CreateOutputFileName(date, DirectoryPath);
            string[] sourceFiles = files.Select(a => a.FullName).ToArray();
            int length = 0;
            int DataLength = 0;
            HeaderInfo info = new HeaderInfo();
            foreach (string file in sourceFiles)
            {
                info = CaptureHeaderInformation(file, ref length, ref DataLength);
            }
            CreateHeaderForNewFile(length, DataLength, info);
            int DataStart = info.ChunkOneEndPoint() + 12;

            foreach (string path in sourceFiles)
            {
                FileStream fs = new FileStream(@path, FileMode.Open, FileAccess.Read);
                byte[] arrfile = new byte[fs.Length - DataStart];
                fs.Position = DataStart;
                fs.Read(arrfile, 0, arrfile.Length);
                fs.Close();

                FileStream fo = new FileStream(@OutputFile, FileMode.Append, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fo);
                bw.Write(arrfile);
                bw.Close();
                fo.Close();
            }
            return OutputFile;
        }

        private HeaderInfo CaptureHeaderInformation(string spath, ref int length, ref int DataLength)
        {
            HeaderInfo hinfo = new HeaderInfo();
            FileStream fs = new FileStream(spath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            fs.Position = 4;
            length = length + (int)fs.Length - 8;
            fs.Position = 16;
            hinfo.SubChunkSize = br.ReadInt32();
            fs.Position = 20;
            hinfo.AudioFormat = br.ReadInt16();
            fs.Position = 22;
            hinfo.Channels = br.ReadInt16();
            fs.Position = 24;
            hinfo.SampleRate = br.ReadInt32();
            fs.Position = 28;
            hinfo.ByteRate = br.ReadInt32();
            fs.Position = 32;
            hinfo.BlockAlign = br.ReadInt16();
            fs.Position = 34;
            hinfo.BitsPerSample = br.ReadInt16();
            fs.Position = 38;
            hinfo.ExtraData = br.ReadInt16();
            fs.Position = hinfo.ChunkOneEndPoint() + 8;
            hinfo.SubChunk3Size = br.ReadInt32();
            DataLength = DataLength + (int)fs.Length - hinfo.ChunkOneEndPoint() - 8;
            fs.Position = hinfo.ChunkOneEndPoint() + 8;
            int dataLength = br.ReadInt32();

            br.Close();
            fs.Close();
            return hinfo;
        }

        private void CreateHeaderForNewFile(int length, int DataLength, HeaderInfo hInfo)
        {
            var channels = hInfo.Channels;
            var samplerate = hInfo.SampleRate;
            var BitsPerSample = hInfo.BitsPerSample;

            FileStream fs = new FileStream(OutputFile, FileMode.Create, FileAccess.Write);

            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(new char[4] { 'R', 'I', 'F', 'F' });

            bw.Write(length);

            bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });

            bw.Write(20);

            bw.Write(hInfo.AudioFormat);

            bw.Write(channels);

            bw.Write(samplerate);

            bw.Write(hInfo.ByteRate);

            bw.Write(hInfo.BlockAlign);

            bw.Write(BitsPerSample);

            bw.Write((short)2);

            bw.Write(hInfo.ExtraData);

            bw.Write(new char[4] { 'd', 'a', 't', 'a' });
            bw.Write(DataLength);
            bw.Close();
            fs.Close();
        }

        private void CreateOutputFileName(DateTime date, string DirectoryPath)
        {
            string dateToString = date.ToString(@"yyyy-MMM-dd");
            OutputFile = DirectoryPath + "/CompleteRecording-" + dateToString + ".WAV";
        }
    }

    public class HeaderInfo
    {
        public short Channels { get; set; }
        public int SampleRate { get; set; }
        public short BitsPerSample { get; set; }
        public int SubChunkSize { get; set; }
        public int SubChunk2Size { get; set; }
        public int NumberOfSamples { get; set; }
        public short AudioFormat { get; set; }
        public short ExtraData { get; set; }
        public int SubChunk3Size { get; set; }
        public short BlockAlign { get; set; }
        public int ByteRate { get; set; }
        public int ChunkOneEndPoint()
        {
            return 16 + SubChunkSize;
        }
    }
}
