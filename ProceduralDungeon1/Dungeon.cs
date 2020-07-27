using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ProceduralDungeon1
{
    //public struct Vector2
    //{
    //    public Vector2(int x, int y)
    //    {
    //        X = x;
    //        Y = y;
    //    }

    //    public int X { get; }
    //    public int Y { get; }
    //}

    class Dungeon
    {
        static Random Random = new Random();
        
        bool[,] NoiseMap;
        char[,] DungeonMap; //Characters to draw the dungeon
        int StartX, StartY;

        List<Vector2> EdgeList = new List<Vector2>();

        public void GenerateNoiseMap(int xSize, int ySize, int threshold)
        {
            #region Fill NoiseMap with solid blocks
            NoiseMap = new bool[xSize, ySize];
            DungeonMap = new char[xSize, ySize];

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
            DungeonMap = new char[xSize, ySize];
            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    NoiseMap[x, y] = true;
                }
            }
            #endregion

            //Pick a random starting position
            StartX = Random.Next(0, xSize/2);
            StartY = Random.Next(0, ySize/2);

            //Percentage of cells that should be floor
            int floorCells = ((xSize * ySize) / 100) * threshold;

            //Direction to move randomly
            int xChange = 0;
            int yChange = 0;

            int CurrentX = StartX;
            int CurrentY = StartY;
            float yBias, xBias;
            yBias = 0;
            xBias = 0;

            do
            {
                float currentXPerc = ((float)CurrentX / (float)xSize) * 100f;
                float currentYPerc = ((float)CurrentY / (float)ySize) * 100f;

                if (currentXPerc < 25)
                    xBias += 0.005f;
                else if (currentXPerc > 75)
                    xBias -= 0.005f;

                if (currentYPerc < 25)
                    yBias += 0.005f;
                else if (currentYPerc > 75)
                    yBias -= 0.005f;

                #region X Direction
                if (RandomPercent() > 50 + xBias)
                    xChange = -1;
                else
                    xChange = 1;
                #endregion

                #region Y Direction
                if (RandomPercent() > 50 + yBias)
                    yChange = -1;
                else
                    yChange = 1;
                #endregion

                #region Limit to 4 cardinal directions
                if (Random.NextDouble() < 0.5f)
                    xChange = 0;
                else
                    yChange = 0;
                #endregion

                int NextX = Clamp(CurrentX + xChange, 0, xSize-1);
                int NextY = Clamp(CurrentY + yChange, 0, ySize-1);

                if (NoiseMap[NextX, NextY] == true)
                {                    
                    NoiseMap[NextX, NextY] = false;
                    floorCells--;

                    System.Threading.Thread.Sleep(20);
                    //Console.Clear();
                    DrawNoiseMap();
                }

                CurrentX = NextX;
                CurrentY = NextY;
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
                    if (alive == false && aliveCount >= 5)
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
            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    bool alive = NoiseMap[x, y];
                    int deadCount = 0;

                    //Count dead neighbours
                    for (int yTest = -1; yTest < 2; yTest++)
                    {
                        for (int xTest = -1; xTest < 2; xTest++)
                        {
                            if (!(xTest == 0 && yTest == 0))
                            {
                                int xClamp = Clamp(x + xTest, 0, NoiseMap.GetLength(0) - 1);
                                int yClamp = Clamp(y + yTest, 0, NoiseMap.GetLength(1) - 1);

                                if (NoiseMap[xClamp, yClamp] == false)
                                    deadCount++;
                            }
                        }
                    }

                    if (alive == true)
                    {
                        if (deadCount >= 1)
                        {
                            DungeonMap[x, y] = 'E';
                            EdgeList.Add(new Vector2(x, y));
                        }
                    }
                }
            }
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
                        DungeonMap[x,y] = '▓';
                    }
                    else
                    {
                        yString += ' ';
                        DungeonMap[x, y] = ' ';
                    }
                }

                Console.WriteLine(yString);
            }
        }

        public void DrawDungeonMap()
        {
            for (int y = 0; y < NoiseMap.GetLength(1); y++)
            {
                String yString = "";

                for (int x = 0; x < NoiseMap.GetLength(0); x++)
                {
                    yString += DungeonMap[x, y];
                }

                Console.WriteLine(yString);
            }
        }

        public void DrunkenNoise()
        {
            //for each cell that isn't part of the original walk
            //Find it's percentage distance to the nearest edge of the walk
            //Use that percentage to randomise whether it's alive or not
            //EdgeList.OrderBy(cell => cell.X + cell.Y > RandomPercent());
            while (EdgeList.Count > 5)
            {
                EdgeList.RemoveRange(Random.Next(0, EdgeList.Count), 1);
            }


            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    if (NoiseMap[x, y] == true)
                    {
                        //Find closest edge
                        //foreach (Vector2 edgeCell in EdgeList)

                        

                        if (EdgeList.Count > 0)
                        {
                            Vector2 edgeCell = EdgeList.OrderBy(cell => GetDist((int)cell.X, (int)cell.Y, x, y)).First();
                            {
                                float dist = GetDist(x, y, (int)edgeCell.X, (int)edgeCell.Y);

                                if (dist < 15)
                                {
                                    float distPerc = (dist / 15) * 100;

                                    if (RandomPercent() > distPerc)
                                        NoiseMap[x, y] = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DrunkenWalkToPoint(Vector2 startPos, Vector2 endPos)
        {
            //While currentPos != endPos
            //Pick a random direction and move in that direction
            //If the direction is away from endPos
            //Weight the next move slightly more towards endPos
            //If the direction is towards endPos. Reset the weights
            Vector2 currentPos = startPos;

            List<Vector2> WalkPath = new List<Vector2>();

            int xChange = 0;
            int yChange = 0;
            float yBias, xBias;
            yBias = 0;
            xBias = 0;

            int count = 0;

            do
            {
                #region X Direction
                if (RandomPercent() > 50 + xBias)
                    xChange = -1;
                else
                    xChange = 1;
                #endregion

                #region Y Direction
                if (RandomPercent() > 50 + yBias)
                    yChange = -1;
                else
                    yChange = 1;
                #endregion

                #region Limit to 4 cardinal directions
                if (Random.NextDouble() < 0.5f)
                {
                    xChange = 0;
                }
                else
                {
                    yChange = 0;
                }
                #endregion

                int NextX = Clamp((int)currentPos.X + xChange, 0, NoiseMap.GetLength(0) - 1);
                int NextY = Clamp((int)currentPos.Y + yChange, 0, NoiseMap.GetLength(1) - 1);

                if (NoiseMap[NextX, NextY] == true)
                {
                    //Direction from the current position to the end
                    Vector2 dirToEnd = endPos - currentPos;
                    Vector2 vecDelta = Vector2.Normalize(dirToEnd);
                    dirToEnd = new Vector2(Math.Sign(dirToEnd.X), Math.Sign(dirToEnd.Y));

                    float biasChange;

                    //When the currentPos gets close to the endPos
                    //Start to bias more heavily towards the end point
                    if (Vector2.Distance(endPos, currentPos) < 30)
                        biasChange = 15f;
                    else
                        biasChange = 8f;

                    if (xChange < dirToEnd.X)
                        xBias += biasChange * Math.Abs(vecDelta.X);
                    else if (xChange > dirToEnd.X)
                        xBias -= biasChange * Math.Abs(vecDelta.X);
                    else if (xChange == dirToEnd.X)
                        xBias = 0f;

                    if (yChange < dirToEnd.Y)
                        yBias += biasChange * Math.Abs(vecDelta.Y);
                    else if (yChange > dirToEnd.Y)
                        yBias -= biasChange * Math.Abs(vecDelta.Y);
                    else if (yChange == dirToEnd.Y)
                        yBias = 0f;

                    NoiseMap[NextX, NextY] = false;
                    WalkPath.Add(new Vector2(NextX, NextY));
                    count++;
                }

                currentPos = new Vector2(NextX, NextY);
            }
            while (currentPos != endPos);

            //Widen the path
            foreach (Vector2 cell in WalkPath)
            {
                if (NoiseMap[Clamp((int)cell.X - 1, 0, NoiseMap.GetLength(0) - 1), (int)cell.Y] == true &&
                    NoiseMap[Clamp((int)cell.X + 1, 0, NoiseMap.GetLength(0) - 1), (int)cell.Y])
                {
                    if (RandomPercent() > 50)
                        NoiseMap[(int)cell.X + 1, (int)cell.Y] = false;
                    else
                        NoiseMap[(int)cell.X - 1, (int)cell.Y] = false;
                }

                if (NoiseMap[(int)cell.X, Clamp((int)cell.Y + 1, 0, NoiseMap.GetLength(1)-1)] == true &&
                    NoiseMap[(int)cell.X, Clamp((int)cell.Y - 1, 0, NoiseMap.GetLength(1)-1)])
                {
                    if (RandomPercent() > 50)
                        NoiseMap[(int)cell.X, (int)cell.Y + 1] = false;
                    else
                        NoiseMap[(int)cell.X, (int)cell.Y - 1] = false;
                }
            }
        }

        public void FillMap(int xSize, int ySize)
        {
            #region Fill NoiseMap with solid blocks
            NoiseMap = new bool[xSize, ySize];
            DungeonMap = new char[xSize, ySize];

            for (int x = 0; x < NoiseMap.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseMap.GetLength(1); y++)
                {
                    NoiseMap[x, y] = true;
                }
            }
            #endregion
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public float RandomPercent()
        {
            return (float)Random.NextDouble() * 100;
        }

        public int GetDist(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
