using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GenetskiZaCorewar
{
    public static class FileReadWrite
    {
        public static void WriteToTxtFile<T>(string filePath, T objectToWrite)
        {
            StreamWriter sw = new StreamWriter(filePath, false);
            string pomocni;
            pomocni = objectToWrite + "\r\n";
            sw.Write(pomocni);
            sw.Close();
        }
        public static string ReadFromTxtFile<T>(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string jedinka = "";
            // while(sr.EndOfStream == false)
            // {
            //    jedinka += sr.ReadLine();
            // }
            // return jedinka;
            return sr.ReadLine();
        }

        public static void WriteNajbolji<T>(string filePath, T objectToWrite, int generation, float score)
        {
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.WriteLine(";Pokemon iz generacije: " + generation + ", ima skor: " + score);
            sw.Write(objectToWrite);
            sw.Close();
        }
        public static void WritePodaci<T>(string filePath, T bestScore, T averageScore, int generation)
        {
            //StreamWriter sw = new StreamWriter(filePath, false);
            //sw.WriteLine("BEST \t AVG");
            //sw.Close();
            StreamWriter sw2 = new StreamWriter(filePath, true);
            sw2.WriteLine(generation + "\t" + bestScore + "\t" + averageScore);
            sw2.Close();
        }

    }
}
