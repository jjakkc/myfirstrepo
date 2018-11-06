using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fantastic7
{
    class TreasureRoom : Room
    {

        public TreasureRoom()
        {
            _gs = new List<GSprite>();
            _gs.Add(new NSprite(new Rectangle(0, 0, 1280, 720), Color.Silver));
            _gs.Add(new NSprite(new Rectangle(100, 100, 1280 - 200, 720 - 200), Color.White));

            _go = new List<GObject>();
            _go.Add(new Entity(new NSprite(new Rectangle(), Color.Gold)));
        }
    }
}
