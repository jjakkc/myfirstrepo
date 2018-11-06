using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fantastic7
{
    public class Wall : GSprite
    {
        private Rectangle wallrect;

        /// <summary>
        /// 
        /// </summary>
        public Wall(int x = 0, int y = 0)
        {
            wallrect = new Rectangle(x, y, 100, 100);
        }

        public void Update()
        {
 
        }

        public override void draw(SpriteBatchPlus sb, float scale)
        {
            //sb.Draw()
        }

        public override void jumpTo(Vector2 v)
        {
            throw new NotImplementedException();
        }

        public override Vector2 getPosition()
        {
            throw new NotImplementedException();
        }
    }
}
