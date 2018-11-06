using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fantastic7
{
    /// <summary>
    /// 
    /// </summary>
    class MonsterRoom : Room
    {
        private List<Entity> _enemyList;
        //private Wall WallTop { get; }
        //private Wall WallLeft { get; }
        //private Wall WallRight { get; }
        //private Wall WallBottom { get; }

        public MonsterRoom()
        {
            _gs = new List<GSprite>();
            _gs.Add(new NSprite(new Rectangle(0, 0, 1280, 720), Color.Black));
            _gs.Add(new NSprite(new Rectangle(100, 100, 1280 - 200, 720 - 200), Color.White));

            _go = new List<GObject>();

            Random r = new Random();
            for(int i = 0; i < 3; i++)
            {
                //Console.Out.WriteLine("Random num: " + r.Next(100));
                //int posX = r.Next(200, 1000);
                //int posY = r.Next(200, 1000);
                _go.Add(new Entity(new NSprite(new Rectangle(r.Next(200, 1000),
                    r.Next(200, 500), 50, 50), Color.Red)));
            }
        }

        public void spawnEnemies()
        {
            //_enemyList.Add(new Entity(new NSprite(new Rectangle(500, 500, 50, 50), Color.Red)));
        }
    }
}
