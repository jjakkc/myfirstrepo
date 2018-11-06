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
    class GObject
    {
        protected Vector2 _pos;
        protected GSprite _sprite;

        public GObject(GSprite sprite)
        {
            _pos = sprite.getPosition();
            _sprite = sprite;
        }

        //Changes object position to new point.
        public void jumpTo(Vector2 position)
        {
            _pos.X = position.X;
            _pos.Y = position.Y;
            _sprite.jumpTo(position);
        }

        //Changes object position by a vector.
        public void move(Vector2 vector)
        {
            _pos += vector;
            _sprite.jumpTo(_pos);
            //Console.Out.WriteLine("Obj: " + _pos.Y + "\tSpr: " + _sprite.getPosition().Y);
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            _sprite.draw(sb, scale);
        }

        public GSprite getSprite()
        {
            return _sprite;
        }

        public Vector2 getPosition()
        {
            return _pos;
        }

        public Rectangle? CollisionRect()
        {
            if (_sprite is NSprite)
            {
                NSprite n = (NSprite)_sprite;
                int width = n.getRect().Width;
                int height = n.getRect().Height;
                return new Rectangle((int)_pos.X, (int)_pos.Y, width, height);
            }
            return null;
        }
    }

    class Entity : GObject
    {
        protected int _maxHealth;
        protected int _curHealth;
        protected int _intDamage;
        protected bool damage;
        protected bool dead = false; //Used to mark it as ready for garbage collection

        //By default, the entities are set as invulnerable if the max health is not changed
        //Interaction damage is used when entities interact, as long as they both use damage, interaction damage is
        //subtracted from the other entity
        public Entity(GSprite sprite, int maxHealth = -1, int interactionDamage = 0) : base(sprite)
        {
            if (maxHealth < 0) damage = false;
            else damage = true;

            _maxHealth = maxHealth;
            _curHealth = maxHealth;
            _intDamage = interactionDamage;
        }

        //Used to deal damage and gain back health. Unbounded so negative numbers can be passed it
        //Use negative numbers to deal damage, and positive to gain health
        //Damage change will only occure if entity is flagged with using health, rather than being invulnerable
        public void modifyHealth(int delta)
        {
            if (damage)
            {
                _curHealth += delta;
                if (_curHealth < 0)
                {
                    _curHealth = 0;
                    dead = true;
                }
                else if (_curHealth > _maxHealth) _curHealth = _maxHealth;
            }
        }

        //Bounded health change, only positive numbers will used. Will heal entity by current amount
        public void getHealth(int regain)
        {
            if (regain > 0) modifyHealth(regain);
        }

        
    }
}
