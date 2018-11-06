using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fantastic7
{
    class Room
    {
        //protected GObject leftDoor = new GObject(new Vector2(0, 0), new NSprite(new Rectangle(0, 0, 10, 100), Color.Aqua));
        public Room left;
        public Room right;
        public Room up;
        public Room down;
        protected List<GObject> _go;
        protected List<GSprite> _gs;
        public Rectangle floor;

        int direction = 1;

        protected GObject[] _doors = { new GObject(new NSprite(new Rectangle(1280 - 110, 720 / 2 - 50, 110, 100), Color.DarkGray)),
            new GObject(new NSprite( new Rectangle(1280/2-50,0,100,110), Color.DarkGray)),
            new GObject(new NSprite( new Rectangle(0,720/2-50,110,100), Color.DarkGray)),
            new GObject(new NSprite( new Rectangle(1280/2-50,720-110,100,110), Color.DarkGray))};

        //Creates a random room
        public Room()
        {
            Random r = new Random();
            _gs = new List<GSprite>();
            _gs.Add(new NSprite(new Rectangle(0, 0, 1280, 720), new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255))));
            floor = new Rectangle(100, 100, 1280 - 200, 720 - 200);
            _gs.Add(new NSprite(floor, new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255))));

            _go = new List<GObject>();
        }

        //Normal way to create a specific room, including gameObjects and the base room sprites.
        //Optionally doors to others rooms can be included
        public Room( GSprite[] roomSprites, GObject[] gameObjects = null, Room r = null, Room u = null, Room l = null, Room d = null)
        {
            right = r;
            up = u;
            left = l;
            down = d;

            _gs = new List<GSprite>();
            _gs.AddRange(roomSprites);
            
            _go = new List<GObject>();
            if (gameObjects != null) _go.AddRange(gameObjects);
        }

        public void addObject(GObject go)
        {
            _go.Add(go);
        }

        public void update(GameTime gt)
        {
            foreach(GObject go in _go)
            {
                
                //go.move(new Vector2(0, -10 * direction) * (float)gt.ElapsedGameTime.TotalSeconds);
                
            }
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            foreach (GSprite gs in _gs)
            {
                gs.draw(sb, scale);
            }

            foreach (GObject go in _go)
            {
                go.draw(sb, scale);
            }

            if (right != null) _doors[0].draw(sb, scale);
            if (up != null) _doors[1].draw(sb, scale);
            if (left != null) _doors[2].draw(sb, scale);
            if (down != null) _doors[3].draw(sb, scale);
        }

        public GObject[] getDoors()
        {
            return _doors;
        }

        public GObject removeObject(GObject go)
        {
            GObject found;
            foreach (GObject g in _go)
            {
                if (g.Equals(go))
                {
                    found = g;
                    _go.Remove(go);
                    return found;
                }
            }
            return null;
        }
    }
}
