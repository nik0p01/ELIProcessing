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
                //считываем файлы по заданным путям
                eLIPictureFirst.ReadFile(args[0]);
                eLIPictureSecond.ReadFile(args[1]);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found {ex.Message} ");
                return;
            }
            catch (EndOfStreamException ex)
            {
                Console.WriteLine($"Wrong file {ex.Message} ");
                return;
            }
            //делим попиксельно файлы
            var resultELI = eLIPictureFirst.Division(eLIPictureSecond);
            //записываем файл
            resultELI.WriteFile("result.ELT");
            Console.WriteLine($"file with {resultELI.Data.Length} pixels was created");
            Console.ReadKey();
        }
    }
}
