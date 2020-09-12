using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpikeAnalyzer
{
    public static class FileReader
    {
        private readonly static string FolderPath = "../../../StatFiles";
        public static string ReadFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = sr.ReadToEnd();
                    //Console.WriteLine(line);
                    return line;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return "";
        }
        public static string[] GetStatFiles()
        {
            return Directory.GetFiles(FolderPath);
        }
        
    }
}
