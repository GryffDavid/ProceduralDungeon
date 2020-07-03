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
            //dungeon.GenerateNoiseMap(48 * 2, 27 * 2, 40);

            dungeon.GenerateDrunkenWalk(235, 70, 30);

            for (int i = 0; i < 5; i++)
                dungeon.SmoothDungeon();

            //dungeon.FindEdges();
            dungeon.DrawDungeon();
            
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
