using System;
using System.IO;

namespace ELIProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                return;
            }

            ELIPicture eLIPictureFirst = new ELIPicture();
            ELIPicture eLIPictureSecond = new ELIPicture();
            try
            {
                eLIPictureFirst.LoadFromFile(args[0]);
                eLIPictureSecond.LoadFromFile(args[1]);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found. {ex.Message}");
                return;
            }
            catch (EndOfStreamException ex)
            {
                Console.WriteLine($"Wrong file. {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return;
            }
            var resultELI = eLIPictureFirst.Division(eLIPictureSecond);
            resultELI.WriteToFile("result.ELT");
            Console.WriteLine($"file with {resultELI.Data.Length} pixels was created");
            Console.ReadKey();
        }
    }
}
