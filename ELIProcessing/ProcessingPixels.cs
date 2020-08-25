using System;
using System.Collections.Generic;
using System.Threading;

namespace ELIProcessing
{
    /// <summary>
    ///класс для вычислений над двумя коллекциями пикселей  
    /// </summary>
    class ProcessingPixels
    {
        public ushort[] pixelsFirst;
        public ushort[] pixelsSecond;
        //массив, где накапливается результат вычислений из разных потоков. Выбран обычный массив, так как по логике приложения к каждым конкретным элементом массива работает только один поток 
        private ushort[] pixelsOut;
        /// <summary>
        /// деление каждого значения пикселя из pixelsFirst  на соответствующий пиксель из pixelsSecond
        /// </summary>
        /// <returns>результат вычислений</returns>
        public ushort[] Division()
        {
            //число процессоров
            var processorsCount = Environment.ProcessorCount;
            var countPixelsToProcessor = 0;

            //количество пикселей на один поток
            countPixelsToProcessor = pixelsFirst.Length / processorsCount + (pixelsFirst.Length % processorsCount > 0 ? 1 : 0);
            //коллекция потоков по числу процессоров 
            List<Thread> threads = new List<Thread>(processorsCount);
            pixelsOut = new ushort[pixelsFirst.Length];
            //заполнение коллекции потоков потоками метода DivisionMethod
            for (int i = 0; i < processorsCount; i++)
            {
                threads.Add(new Thread(this.DivisionMethod));
            }

            int carentStartIndex = 0;
            //запускаются потоки, в потоки передается начальный идекс в коллекциях пикселей с которого данный поток начнет вычисление и заполннение результирующей коллекции и количество пикселей, требуемое обработать в этом потоке. 
            for (int i = 0; i < processorsCount; i++)
            {
                threads[i].Name = i.ToString();
                threads[i].Start(new int[] { carentStartIndex, (carentStartIndex + countPixelsToProcessor) > pixelsFirst.Length ? pixelsFirst.Length : carentStartIndex + countPixelsToProcessor });
                carentStartIndex += countPixelsToProcessor;
            }
            //ожидается завершения потоков из коллекции threads
            foreach (var thred in threads)
            {
                thred.Join();
            }
            return pixelsOut;
        }

        /// <summary>
        /// выполнение деления пикселей из коллекции pixelsFirst на соответствующие  пиксели pixelsSecond
        /// </summary>
        /// <param name="startEndIndex">int[], где 0-вой элемент - начальный индекс в коллекциях, 1-ый элемент количество пикселей </param>
        private void DivisionMethod(object startEndIndex)
        {
            for (int i = (startEndIndex as int[])[0]; i < (startEndIndex as int[])[1]; i++)
            {
                pixelsOut[i] = (ushort)(pixelsSecond[i] == 0 ? 0 : (pixelsFirst[i] / pixelsSecond[i]));
            }
        }
    }
}
