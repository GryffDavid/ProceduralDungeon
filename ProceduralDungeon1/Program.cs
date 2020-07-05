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

            //dungeon.GenerateNoiseMap(230, 60, 45);
            //dungeon.GenerateDrunkenWalk(230, 60, 10);
            do
            {
                //dungeon.GenerateNoiseMap(230, 60, 45);
                //dungeon.GenerateDrunkenWalk(230, 60, 30);
                //dungeon.DrawNoiseMap();
                //Console.WriteLine(" ");

                //dungeon.FindEdges();
                //dungeon.DrunkenNoise();
                dungeon.FillMap(230, 60);
                dungeon.DrunkenWalkToPoint(new Vector2(0, 0), new Vector2(229, 59));
                dungeon.DrunkenWalkToPoint(new Vector2(0, 59), new Vector2(229, 0));
                //dungeon.DrunkenWalkToPoint(new Vector2(0, 50), new Vector2(150, 25));
                //dungeon.DrunkenWalkToPoint(new Vector2(0, 50), new Vector2(150, 25));
                dungeon.DrawNoiseMap();
                Console.WriteLine(" ");

                dungeon.SmoothDungeon(6);

                for (int i = 0; i < 5; i++)
                    dungeon.SmoothDungeon(5);

                dungeon.SmoothDungeon(6);
                dungeon.DrawNoiseMap();                
            }
            while (Console.ReadKey().Key == ConsoleKey.A);
        }
    }
}
