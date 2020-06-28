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
            dungeon.GenerateNoiseMap(48, 27, 45);

            for (int i = 0; i < 5; i++)
            dungeon.GenerateDungeon(0, 0);

            dungeon.FindEdges();
            dungeon.DrawDungeon();
            
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
