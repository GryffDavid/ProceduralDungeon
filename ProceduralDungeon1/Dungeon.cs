using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProceduralDungeon1
{
    class Dungeon
    {
        static Random Random = new Random();

        bool[,] NoiseMap;// = new bool[27, 48];
        char[,] DungeonArray = new char[27, 48];
        
        public void GenerateNoiseMap(int xSize, int ySize, int threshold)
        {
            NoiseMap = new bool[ySize, xSize];

            for (int y = 0; y < NoiseMap.GetLength(0)-0; y++)
            {
                for (int x = 0; x < NoiseMap.GetLength(1)-0; x++)
                {
                    if (Random.Next(0, 100) > threshold)
                        NoiseMap[y, x] = true;
                    else
                        NoiseMap[y, x] = false;
                }
            }

            DrawNoiseMap();
            Console.WriteLine(' ');
        }

        public void GenerateDungeon(int xSize, int ySize)
        {
            for (int y = 0; y < NoiseMap.GetLength(0); y++)
            {
                for (int x = 0; x < NoiseMap.GetLength(1); x++)
                {
                    bool alive = NoiseMap[y, x];
                    int aliveCount = 0;

                    //Count alive neighbours
                    for (int yTest = -1; yTest < 2; yTest++)
                    {
                        for (int xTest = -1; xTest < 2; xTest++)
                        {
                            if (!(xTest == 0 && yTest == 0))
                            {
                                int xClamp = Clamp(x + xTest, 0, NoiseMap.GetLength(1) - 1);
                                int yClamp = Clamp(y + yTest, 0, NoiseMap.GetLength(0) - 1);

                                if (NoiseMap[yClamp, xClamp] == true)
                                    aliveCount++;
                            }
                        }
                    }

                    if (alive == false)
                    {
                        if (aliveCount >= 6)
                        {
                            NoiseMap[y, x] = true;
                        }
                    }

                    if (alive == true)
                    {
                        if (!(aliveCount >= 4))
                        {
                            NoiseMap[y, x] = false;                            
                        }
                    }
                }
            }
            
            System.Threading.Thread.Sleep(500);
            Console.Clear();

            DrawNoiseMap();

            //DrawDungeon();
            Console.WriteLine(' ');
        }

        public void FindEdges()
        {
            for (int y = 0; y < NoiseMap.GetLength(0); y++)
            {
                for (int x = 0; x < NoiseMap.GetLength(1); x++)
                {
                    bool alive = NoiseMap[y, x];
                    int deadCount = 0;

                    //Count dead neighbours
                    for (int yTest = -1; yTest < 2; yTest++)
                    {
                        for (int xTest = -1; xTest < 2; xTest++)
                        {
                            if (!(xTest == 0 && yTest == 0))
                            {
                                int xClamp = Clamp(x + xTest, 0, NoiseMap.GetLength(1) - 1);
                                int yClamp = Clamp(y + yTest, 0, NoiseMap.GetLength(0) - 1);

                                if (NoiseMap[yClamp, xClamp] == false)
                                    deadCount++;
                            }
                        }
                    }

                    if (alive == true)
                    {
                        if (deadCount >= 1)
                        {
                            DungeonArray[y, x] = '█';
                        }
                    }
                }
            }
        }

        public void DrawDungeon()
        {
            for (int y = 0; y < NoiseMap.GetLength(0); y++)
            {
                String yString = "";

                for (int x = 0; x < NoiseMap.GetLength(1); x++)
                {
                    yString += DungeonArray[y, x];
                }

                Console.WriteLine(yString);
            }
        }

        void DrawNoiseMap()
        {
            for (int y = 0; y < NoiseMap.GetLength(0); y++)
            {
                String yString = "";

                for (int x = 0; x < NoiseMap.GetLength(1); x++)
                {
                    if (NoiseMap[y, x] == true)
                    {
                        yString += '▓';
                        DungeonArray[y,x] = '▓';
                    }
                    else
                    {
                        yString += '.';
                        DungeonArray[y, x] = ' ';
                    }
                }

                Console.WriteLine(yString);
            }
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
