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
    /// <summary>
    /// Abstract class to group together sprites holding text and sprites holding pure color
    /// </summary>
    public abstract class GSprite
    {
        abstract public void draw(SpriteBatchPlus sb, float scale);
        abstract public void jumpTo(Vector2 v);
        abstract public Vector2 getPosition();
    }

    /// <summary>
    /// String Sprite holds strings
    /// </summary>
    class SSprite : GSprite
    {
        private String _t;
        private SpriteFont _f;
        private Vector2 _p;
        private Color _c;

        override public void draw(SpriteBatchPlus sb, float scale) { sb.DrawString(_f, _t, _p, _c); }
        public SSprite(String t, SpriteFont f, Vector2 p, Color c){ _t = t; _f = f; _p = p; _c = c; }

        public SpriteFont getFont() { return _f; }
        override public Vector2 getPosition() { return _p; }
        public String getText() { return _t; }

        //Moves the sprite directly to the point V
        override public void jumpTo(Vector2 v)
        {
            _p.X = v.X;
            _p.Y = v.Y;
        }
    }

    class NSprite : GSprite
    {
        private Rectangle _r;
        private Color _c;
        //private Texture2D _t;

        override public void draw(SpriteBatchPlus sb, float scale) { sb.Draw(sb.defaultTexture(), _r, _c); }
        public NSprite(Rectangle r, Color c) { _r = r; _c = c; }

        override public Vector2 getPosition() { return new Vector2(_r.X,_r.Y); }

        public void setColor(Color color)
        {
            _c = color;
        }
        //Moves the sprite directly to the point V
        override public void jumpTo(Vector2 v)
        {
            _r.X = (int)v.X;
            _r.Y = (int)v.Y;
        }

        public Rectangle getRect()
        {
            return _r;
        }
    }
}
