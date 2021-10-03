using System.IO;

namespace ELIProcessing
{
    /// <summary>
    /// класс для работы с изображением ELI
    /// </summary>
    class ELIPicture
    {
        private ushort[] data;
        /// <summary>
        /// ширина изображения
        /// </summary>
        public int ImageWidth
        {
            get;
            set;
        }
        /// <summary>
        /// высота изображения
        /// </summary>
        public int ImageHeight
        {
            get;
            set;
        }
        /// <summary>
        /// коллекция изображений
        /// </summary>
        public ushort[] Data
        {
            get
            {
                return data ?? new ushort[0];
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// прочитать изображение
        /// </summary>
        /// <param name="path">путь к изображению</param>
        public void LoadFromFile(string path)
        {
            if (File.Exists(path))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    int dataOffset = -1;
                    int offset = 0;
                    while (true)
                    {
                        int t = reader.ReadInt32();

                        if (offset == 8)
                        {
                            dataOffset = t;
                        }
                        else if (offset == 16)
                        {
                            ImageWidth = t;
                        }
                        else if (offset == 20)
                        {
                            ImageHeight = t;
                        }

                        offset += 4;
                        if (offset == dataOffset)
                        {
                            break;
                        }
                    }
                    int countPixels = (int)((new FileInfo(path)).Length - dataOffset) / 2;
                    data = new ushort[countPixels];
                    for (long i = 0; i < countPixels; i++)
                    {
                        data[i] = (reader.ReadUInt16());
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }

        /// <summary>
        /// записать изображение 
        /// </summary>
        /// <param name="path">путь к изображению</param>
        public void WriteToFile(string path)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {
                writer.Write(4803653);
                writer.Write(32);
                writer.Write(512);
                writer.Write(0);
                writer.Write(ImageWidth);
                writer.Write(ImageHeight);
                writer.Write(16);
                writer.Write(1024);
                for (int i = 0; i < 120; i++)
                {
                    writer.Write(0);
                }

                foreach (var pixel in data)
                {
                    writer.Write(pixel);
                }
            }
        }

        /// <summary>
        /// попиксельное деление изображения
        /// </summary>
        /// <param name="eLIPicture">изображение на которое делится</param>
        /// <returns>изображение, результат вычисления</returns>
        public ELIPicture Division(ELIPicture eLIPicture)
        {
            ProcessingPixels processingPixels = new ProcessingPixels() { pixelsFirst = this.Data, pixelsSecond = eLIPicture.Data };
            ELIPicture eLIPictureResult = new ELIPicture() { Data = processingPixels.Division(), ImageHeight = this.ImageHeight, ImageWidth = this.ImageWidth };
            return eLIPictureResult;
        }
    }
}
