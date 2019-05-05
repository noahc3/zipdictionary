using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace zipdictionary {
    class Program {
        static void Main(string[] args) {
            if (!(args.Length >= 3 && args.Length <= 4)) {
                Console.WriteLine("usage: zipdictionary <zip-file> <output-directory> <dictionary-file> [delimiter]");
                Environment.Exit(1);
            }

            char delimiter = '\n';

            if (args.Length == 4) {
                if (args[3].Length == 1) {
                    delimiter = args[3][0];
                } else {
                    Console.WriteLine("Delimiter too long! (1 char only)");
                    Environment.Exit(2);
                }
            }

            string zip = args[0];
            string outDir = args[1];
            string dict = args[2];

            string[] passwords = File.ReadAllText(dict).Split(delimiter);

            if (!File.Exists(zip)) {
                Console.WriteLine("ZIP file does not exist!");
                Environment.Exit(3);
            }

            Console.WriteLine("== zipdictionary by noah ==");
            Console.WriteLine("Starting...");

            if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);

            FastZip fastzip = new FastZip();

            foreach(string k in passwords) {
                try {
                    fastzip.Password = k;
                    fastzip.ExtractZip(zip, outDir, "");
                } catch (ZipException) {
                    continue;
                }
                Console.WriteLine("Password found: " + k);
                Console.WriteLine("Files extracted to " + Path.GetFullPath(outDir));
                break;
            }
        }
    }
}
