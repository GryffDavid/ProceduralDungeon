using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ProceduralDungeon1
{
    class Program
    {
        static Random Random = new Random();

        static void Main(string[] args)
        { 
            Dungeon dungeon = new Dungeon();

            do
            {
                //dungeon.GenerateNoiseMap(230, 60, 40);
                dungeon.GenerateDrunkenWalk(230, 60, 10);
                dungeon.DrawNoiseMap();
                Console.WriteLine(" ");


                //dungeon.FillMap(230, 60);
                dungeon.DrunkenWalkToPoint(new Vector2(115, 30), new Vector2(Random.Next(0, 229), Random.Next(0, 59)));
                dungeon.DrunkenWalkToPoint(new Vector2(115, 30), new Vector2(Random.Next(0, 229/2), Random.Next(0, 59/2)));

                dungeon.FindEdges();
                dungeon.DrunkenNoise();

                //dungeon.DrunkenNoise();

                dungeon.DrawNoiseMap();
                Console.WriteLine(" ");

                for (int i = 0; i < 5; i++)
                {
                    dungeon.SmoothDungeon();
                    //System.Threading.Thread.Sleep(1000);
                    //dungeon.DrawNoiseMap();
                }

                //for (int i = 0; i < 5; i++)
                //{
                //    dungeon.SmoothDungeon();
                //    System.Threading.Thread.Sleep(1000);
                    dungeon.DrawNoiseMap();
                //}
            }
            while (Console.ReadKey().Key == ConsoleKey.A);
        }
    }
}
