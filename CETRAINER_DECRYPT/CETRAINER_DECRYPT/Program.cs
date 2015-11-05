using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETRAINER_DECRYPT
{
    class Program
    {
        static CE cheatEngine;

        static void Main(string[] args)
        {
            Console.WriteLine("Cheat Engine 6.4 Trainer Decryptor");
            Console.WriteLine("Author:  Cra0");
            Console.WriteLine("Website: dev.cra0kalo.com");
            Console.WriteLine("-----------------------");
            //get the data from the arg1
            if (args.Length == 0 || args.Length < 1)
            {
                Console.WriteLine("Usage: CETRAINER_DECRYPT example.CETRAINER");
                Console.WriteLine("Example: CETRAINER_DECRYPT C:/unpacked_sfx/CET_TRAINER.CETRAINER");
                Console.ReadKey();
                return;
            }

            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine(filePath + " doesn't seem to exist");
                Console.ReadKey();
                return;
            }

            cheatEngine = new CE();
            bool result = cheatEngine.DecryptTrainer(filePath);
            if (result)
            {
                Console.WriteLine("Decrypted " + Path.GetFileName(filePath) + " Successfully!");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Unable to decrypt " + Path.GetFileName(filePath));
                Console.ReadKey();
                return;
            }
        }
    }
}
