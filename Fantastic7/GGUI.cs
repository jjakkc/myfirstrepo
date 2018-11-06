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
    /// GUI with the capability to have selectable regions 
    /// To create a gui with no selectable inputs pass nothing in for the selections input
    /// </summary>
    class GGUI
    {
        private GSprite[] _sprites;
        private int _index;
        private bool sel;
        private Color _color;
        private MenuOption[] _menuOptions;

        //If no selection is passed, it will disable using selection cursor
        public GGUI(GSprite[] sprites, MenuOption[] menuOptions, Color selectorColor, int defaultIndex = 0)
        {
            if (menuOptions == null)
            {
                defaultIndex = -1;
                sel = false;
            }
            else sel = true;

            _sprites = sprites;
            _menuOptions = menuOptions;
            _index = defaultIndex;
            _color = selectorColor;
        }

        public void nextOption() { _index = (_index + 1) % _menuOptions.Length; } //Scrolls the selection downwards (increments)
        public void previousOption() { _index = ((_index == 0) ? _menuOptions.Length-1 : _index-1); } //Scrolls the selection upwards (decrements)
        public int getIndex() { return _index; }
        public void toggleCursor() { sel = !sel; } //Toggles the cursor that is under the current index selection.

        /// <summary>
        /// Draws the sprites contained in the GUI, if there is a selection grid, it will also be drawn.
        /// The index can be used to determine which selection is currently being pointed at.
        /// </summary>
        /// <param name="sb">Standard Sprite batch</param>
        /// <param name="scale">Used for scaleing, currently unused</param>
        public void draw(SpriteBatchPlus sb, float scale)
        {
            foreach (GSprite s in _sprites)
            {
                s.draw(sb,scale);
            }

            //If no menu options are present, this is skipped
            if(_menuOptions != null) foreach(MenuOption mo in _menuOptions)
            {
                mo.draw(sb, scale);
            }

            //Draws a sector underneath the current index in the gui
            if(sel)
            {
                Rectangle line = new Rectangle(_menuOptions[_index].getRegion().X,
                    _menuOptions[_index].getRegion().Y + _menuOptions[_index].getRegion().Height,
                    _menuOptions[_index].getRegion().Width,
                    5);
                sb.Draw(sb.defaultTexture(), line, _color);
            }

        }
    }
    /// <summary>
    /// Class used to create menu options. It uses a SSprite to draw it on the GUI. 
    /// It creates a rectangle that is the region the text makes up and can be retrieved for collision testing
    /// </summary>
    class MenuOption
    {
        private SSprite _text;
        private Rectangle _region;

        public MenuOption(SSprite text)
        {
            SpriteFont f = text.getFont();
            _text = text;
            _region = new Rectangle((int)text.getPosition().X, (int)text.getPosition().Y, (int)f.MeasureString(text.getText()).X, (int)f.MeasureString(text.getText()).Y);
        }
        public Rectangle getRegion() { return _region; }

        public void draw(SpriteBatchPlus sb, float scale) { _text.draw(sb, scale); }
    }
}
