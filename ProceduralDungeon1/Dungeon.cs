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

        bool[,] NoiseMap;

        public void GenerateNoiseMap(int xSize, int ySize, int threshold)
        {
            #region Fill NoiseMap with solid blocks
            NoiseMap = new bool[xSize, ySize];
            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    NoiseMap[x, y] = true;
                }
            }
            #endregion

            //Percentage of cells that should be floor
            int floorCells = ((xSize * ySize) / 100) * threshold;

            do
            {
                int randX = Random.Next(0, NoiseMap.GetLength(0));
                int randY = Random.Next(0, NoiseMap.GetLength(1));

                if (NoiseMap[randX, randY] == true)
                {
                    NoiseMap[randX, randY] = false;
                    floorCells--;
                }
            }
            while (floorCells > 0);
        }

        public void GenerateDrunkenWalk(int xSize, int ySize, int threshold)
        {
            #region Fill NoiseMap with solid blocks
            NoiseMap = new bool[xSize, ySize];
            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    NoiseMap[x, y] = true;
                }
            }
            #endregion

            //Pick a random starting position
            int StartX = Random.Next(0, xSize-1);
            int StartY = Random.Next(0, ySize-1);
            NoiseMap[StartX, StartY] = false;

            //Percentage of cells that should be floor
            int floorCells = ((xSize * ySize) / 100) * threshold;

            //Direction to move randomly
            int xChange = 0;
            int yChange = 0;
            
            do
            {
                xChange = Random.Next(-1, 2);
                yChange = Random.Next(-1, 2);

                #region Limit to 4 cardinal directions
                if (Random.NextDouble() < 0.5f)
                    xChange = 0;
                else
                    yChange = 0; 
                #endregion

                int NextX = Clamp(StartX + xChange, 0, xSize-1);
                int NextY = Clamp(StartY + yChange, 0, ySize-1);

                if (NoiseMap[NextX, NextY] == true)
                {                    
                    NoiseMap[NextX, NextY] = false;
                    floorCells--;
                }

                StartX = NextX;
                StartY = NextY;
            }
            while (floorCells > 0);
        }

        public void SmoothDungeon()
        {
            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    bool alive = NoiseMap[x, y];
                    int aliveCount = 0;

                    //Count alive neighbours
                    for (int xTest = -1; xTest < 2; xTest++)
                    {
                        for (int yTest = -1; yTest < 2; yTest++)
                        {
                            if (!(xTest == 0 && yTest == 0))
                            {
                                int xClamp = Clamp(x + xTest, 0, NoiseMap.GetLength(0) - 1);
                                int yClamp = Clamp(y + yTest, 0, NoiseMap.GetLength(1) - 1);

                                if (NoiseMap[xClamp, yClamp] == true)
                                    aliveCount++;
                            }
                        }
                    }

                    //Cell is dead and 6 or more cells around it are alive. Make this cell alive.
                    if (alive == false && aliveCount >= 6)
                    {
                        NoiseMap[x, y] = true;                        
                    }

                    //Cell is alive and 4 or more cells around it are dead. Make this cell dead.
                    if (alive == true && aliveCount < 4)
                    {
                        NoiseMap[x, y] = false;                        
                    }
                }
            }
        }

        public void FindEdges()
        {
            //for (int y = 0; y < NoiseMap.GetLength(0); y++)
            //{
            //    for (int x = 0; x < NoiseMap.GetLength(1); x++)
            //    {
            //        bool alive = NoiseMap[y, x];
            //        int deadCount = 0;

            //        //Count dead neighbours
            //        for (int yTest = -1; yTest < 2; yTest++)
            //        {
            //            for (int xTest = -1; xTest < 2; xTest++)
            //            {
            //                if (!(xTest == 0 && yTest == 0))
            //                {
            //                    int xClamp = Clamp(x + xTest, 0, NoiseMap.GetLength(1) - 1);
            //                    int yClamp = Clamp(y + yTest, 0, NoiseMap.GetLength(0) - 1);

            //                    if (NoiseMap[yClamp, xClamp] == false)
            //                        deadCount++;
            //                }
            //            }
            //        }

            //        if (alive == true)
            //        {
            //            if (deadCount >= 1)
            //            {
            //                DungeonArray[y, x] = '█';
            //            }
            //        }
            //    }
            //}
        }

        public void DrawNoiseMap()
        {
            for (int y = 0; y < NoiseMap.GetLength(1); y++)
            {
                String yString = "";

                for (int x = 0; x < NoiseMap.GetLength(0); x++)
                {
                    if (NoiseMap[x, y] == true)
                    {
                        yString += '▓';
                    }
                    else
                    {
                        yString += ' ';
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
