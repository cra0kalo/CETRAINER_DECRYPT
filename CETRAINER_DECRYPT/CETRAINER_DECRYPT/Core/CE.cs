using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace CETRAINER_DECRYPT
{
    public partial class CE
    {


        public CE()
        {
            //Don't need to do anything
        }

        public bool DecryptTrainer(string filePath)
        {
            MemoryStream ms;
            BinaryReader br;

            string outFileName = "decrypted_trainer.xml";
            string outFilePath = Path.Combine(Path.GetDirectoryName(filePath), outFileName);

            byte[] raw_data = File.ReadAllBytes(filePath);
            int size = raw_data.Length;
            byte ckey = 0xCE;

            //Quote from original author very true indeed keeps the idiots out :P
            /*
            this is the super mega protector routine for the trainer
            Yeah, it's pathetic, but it keeps the retarded noobs out that don't know how to
            read code and only know how to copy/paste
            */

            for (int i = 2; i < size; i++)
            {
                raw_data[i] = (byte)(raw_data[i] ^ raw_data[i - 2]);
            }

            for (int j = size - 2; j >= 0; j--)
            {
                raw_data[j] = (byte)(raw_data[j] ^ raw_data[j + 1]);
            }

            for (int k = 0; k < size; k++)
            {
                raw_data[k] = (byte)(raw_data[k] ^ ckey);
                ++ckey;
            }

            ms = new MemoryStream(raw_data);
            br = new BinaryReader(ms);


            byte[] CE_MAGIC = br.ReadBytes(5);
            if (Encoding.ASCII.GetString(CE_MAGIC) == "CHEAT")
            {
                //Yes we have valid output
                ms.Seek(5, SeekOrigin.Begin);
                byte[] out_data = DeCompress(br.ReadBytes((int)ms.Length - 5));
                if (out_data.Length == 0)
                {
                    Console.WriteLine("Failed to decompress!");
                    return false;
                }
                byte[] out_dataFinal = new byte[out_data.Length - 4]; //Don't include the length of the file
                Array.Copy(out_data, 4, out_dataFinal, 0, out_dataFinal.Length);
                File.WriteAllBytes(outFilePath, out_dataFinal); //Spit that shit out and save
                return true;
            }
            else
            {
                Console.WriteLine("Either decryption went wrong or the trainer is using the old compression method which I will not support");
                return false;
            }

        }


        private static byte[] DeCompress(byte[] data)
        {
            using (var compressStream = new MemoryStream(data))
            using (var outStream = new MemoryStream())
            using (var compressor = new DeflateStream(compressStream, CompressionMode.Decompress))
            {
                compressor.CopyTo(outStream);
                compressor.Close();
                return outStream.ToArray();
            }
        }


    }
}
