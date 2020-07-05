using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralDungeon1
{
    class Program
    {
        static void Main(string[] args)
        { 
            Dungeon dungeon = new Dungeon();

            //dungeon.GenerateNoiseMap(230, 60, 45);
            dungeon.GenerateDrunkenWalk(230, 60, 45);

            dungeon.DrawNoiseMap();
            Console.WriteLine(" ");

            for (int i = 0; i < 5; i++)
                dungeon.SmoothDungeon();

            dungeon.DrawNoiseMap();

            Console.ReadKey();
        }
    }
}
