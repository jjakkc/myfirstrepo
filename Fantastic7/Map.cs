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
    class Map
    {
        private bool muteConsole = true;
        private Room[] _rooms;
        private Room _currRoom;
        private int size = 6;
        private Entity _player;
        private int[] dir = { 0, 1, 2, 3 };
        private const int doorChance = 10;
        private const int popChance = 20;
        private const int directionChance = 50;
        private const int roomRejectionSize = 10;
        private const int roomUpperBound = 30;
        private Random r;

        // Used for delay in key presses
        private int elapsedTime = 0;
        private int millisecDelay = 50;
        private bool showMinimap = false;
        Direction direction;

        enum Direction
        {
            North,
            East,
            South,
            West
        };

        private GGUI miniMap;

        public Map()
        {
            _rooms = new Room[size * size];
        }

        public void GenerateMap()
        {

            r = new Random();
            _player = new Entity(new NSprite(new Rectangle(500, 500, 50, 50), Color.Wheat));

            int count;
            int x, y;
            do
            {

                //Generates first room in a random place on map
                x = r.Next(0, size);
                y = r.Next(0, size);
                _rooms[x + size * y] = new Room();
                _currRoom = _rooms[x + size * y];

                //Creates stack used for recursize alg
                Stack<Room> _stack = new Stack<Room>();
                _stack.Push(_rooms[x + size * y]);
                _roomRec(_stack, x, y);// Generates map layout recursively 

                count = 0;
                for (int i = 0; i < size * size; i++) if (_rooms[i] != null) count++;
                if (count < roomRejectionSize || count > roomUpperBound)
                {
                    if(!muteConsole) Console.Out.WriteLine("Retry Generation /////////////////");
                    for (int i = 0; i < size * size; i++) _rooms[i] = null;
                }
            } while (count < roomRejectionSize || count > roomUpperBound);

            _currRoom.addObject(_player);//Puts player in first room

            //
            //Used for constructing minimap, can be ignorned 
            List < GSprite > gs = new List<GSprite>();

            gs.Add(new NSprite(new Rectangle(0, 0, 10 + 100 * size, 10 + 100 * size), Color.Black));

            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    if (_rooms[i + size * j] != null)
                    {
                        if (i == x && j == y) gs.Add(new NSprite(new Rectangle(10 + (100) * i, 10 + 100 * j, 90, 90), Color.Red));
                        else gs.Add(new NSprite(new Rectangle(10 + (100) * i, 10 + 100 * j, 90, 90), Color.Azure));
                    }
                }
            }

            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    if (_rooms[i + size * j] != null)
                    {
                        if (_rooms[i + size * j].right != null) gs.Add(new NSprite(new Rectangle(100 + (100) * i, 50 + 100 * j, 10, 10), Color.Gray));
                        if (_rooms[i + size * j].down != null) gs.Add(new NSprite(new Rectangle(50 + (100) * i, 100 + 100 * j, 10, 10), Color.Gray));
                    }
                }
            }
            miniMap = new GGUI(gs.ToArray(), null, Color.Beige);
            if (!muteConsole)  Console.Out.WriteLine("Complete");
            //End minimap section
            //


        }


        //The recursive alg to generate the map
        protected void _roomRec(Stack<Room> stack, int x, int y)
        {


            int var = r.Next(100 - (stack.Count - 2) * 5);
            if (!muteConsole) Console.Out.WriteLine("Stack #:" + stack.Count + "\tRNG:" + var);

            //Used to break up long chains of rooms. Will randomly stop a chain an go back a room
            if (stack.Count > 1 && popChance > var)
            {
                stack.Pop();
                return;
            }


            List<int> a = new List<int>();
            a.AddRange(dir);
            int sel;

            //Checks to see if the room in on the edge of the map and prevents it from generating outside the map
            if (x == size - 1) a.Remove(0);
            if (y == 0) a.Remove(1);
            if (x == 0) a.Remove(2);
            if (y == size - 1) a.Remove(3);

            //Checks to see which direction it came from or if it has any doors attached to it. Removes those to prevent doubling up of rooms
            if (stack.Peek().right != null) a.Remove(0);
            if (stack.Peek().up != null) a.Remove(1);
            if (stack.Peek().left != null) a.Remove(2);
            if (stack.Peek().down != null) a.Remove(3);

            //Will loop until all directional options are exhausted 
            while (a.Count > 0)
            {
                if (!muteConsole) Console.Out.WriteLine("Stack #:" + stack.Count);

                //Random direction pop
                if (directionChance > r.Next(100))
                {
                    if (!muteConsole) Console.Out.WriteLine("Direction Pop");
                    a.Remove(a.ElementAt(r.Next(a.Count())));
                }
                
                if(a.Count > 0)
                {
                    sel = r.Next(a.Count); //Picks a random direction that is still unattempted
                    switch (a.ElementAt(sel))
                    {
                        case 0:
                            if (_rooms[x + 1 + size * y] == null)//Checks if the direction does not have a room and creates one
                            {
                                //_rooms[x + 1 + size * y] = new Room();
                                _rooms[x + 1 + size * y] = CreateRandRoom();
                                _rooms[x + 1 + size * y].left = _rooms[x + size * y];
                                _rooms[x + size * y].right = _rooms[x + 1 + size * y];
                                stack.Push(_rooms[x + 1 + size * y]);
                                _roomRec(stack, x + 1, y); //Calls back to itself restarting process
                            }
                            else
                            {
                                if (doorChance > r.Next(100)) //If the position has a room, randomly decides to put a door
                                {
                                    _rooms[x + 1 + size * y].left = _rooms[x + size * y];
                                    _rooms[x + size * y].right = _rooms[x + 1 + size * y];
                                }
                            }
                            a.Remove(0);//Prevents option from coming up again
                            break;

                        case 1:
                            if (_rooms[x + size * (y - 1)] == null)
                            {
                                //_rooms[x + size * (y - 1)] = new Room();
                                _rooms[x + size * (y - 1)] = CreateRandRoom();
                                _rooms[x + size * (y - 1)].down = _rooms[x + size * y];
                                _rooms[x + size * y].up = _rooms[x + size * (y - 1)];
                                stack.Push(_rooms[x + size * (y - 1)]);
                                _roomRec(stack, x, y - 1);
                            }
                            else
                            {
                                if (doorChance > r.Next(100))
                                {
                                    _rooms[x + size * (y - 1)].down = _rooms[x + size * y];
                                    _rooms[x + size * y].up = _rooms[x + size * (y - 1)];
                                }
                            }
                            a.Remove(1);
                            break;

                        case 2:
                            if (_rooms[x - 1 + size * y] == null)
                            {
                                //_rooms[x - 1 + size * y] = new Room();
                                _rooms[x - 1 + size * y] = CreateRandRoom();
                                _rooms[x - 1 + size * y].right = _rooms[x + size * y];
                                _rooms[x + size * y].left = _rooms[x - 1 + size * y];
                                stack.Push(_rooms[x - 1 + size * y]);
                                _roomRec(stack, x - 1, y);
                            }
                            else
                            {
                                if (doorChance > r.Next(100))
                                {
                                    _rooms[x - 1 + size * y].right = _rooms[x + size * y];
                                    _rooms[x + size * y].left = _rooms[x - 1 + size * y];
                                }
                            }
                            a.Remove(2);
                            break;

                        case 3:
                            if (_rooms[x + size * (y + 1)] == null)
                            {
                                //_rooms[x + size * (y + 1)] = new Room();
                                _rooms[x + size * (y + 1)] = CreateRandRoom();
                                _rooms[x + size * (y + 1)].up = _rooms[x + size * y];
                                _rooms[x + size * y].down = _rooms[x + size * (y + 1)];
                                stack.Push(_rooms[x + size * (y + 1)]);
                                _roomRec(stack, x, y + 1);
                            }
                            else
                            {
                                if (doorChance > r.Next(100))
                                {
                                    _rooms[x + size * (y + 1)].up = _rooms[x + size * y];
                                    _rooms[x + size * y].down = _rooms[x + size * (y + 1)];
                                }
                            }
                            a.Remove(3);
                            break;
                        default:
                            break;

                    }
                }
            }

            stack.Pop(); //Base case occures when all four directions are tried

        }

        public void update(GameTime gt)
        {
            _currRoom.update(gt);

            elapsedTime += gt.ElapsedGameTime.Milliseconds;
            if(elapsedTime > millisecDelay)
            {
                elapsedTime = 0;
                //bring up minimap
                if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                {
                    showMinimap = !showMinimap;
                }
            }
            MovePlayer();
        }

        public void changeRoom(int i)
        {
            if (!(i >= size * size || i < 0)) _currRoom = _rooms[i];
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            _currRoom.draw(sb,scale);

            //This draws the map over top off the room
            //Used for testing purposes when checking the map generation
            if (showMinimap)
            {
                miniMap.draw(sb, scale);
            }
        }

        /// <summary>
        /// Uses wasd keys to move player within bounds of room. Detects when player
        /// touches a door object.
        /// </summary>
        public void MovePlayer()
        {
            int speed = 10;
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
                _player.move(new Vector2(0, -speed));
            if (keyboardState.IsKeyDown(Keys.S))
                _player.move(new Vector2(0, speed));
            if (keyboardState.IsKeyDown(Keys.A))
                _player.move(new Vector2(-speed, 0));
            if (keyboardState.IsKeyDown(Keys.D))
                _player.move(new Vector2(speed, 0));

            int playerX = (int)_player.getPosition().X;
            int playerY = (int)_player.getPosition().Y;
            int playerWidth = _player.CollisionRect().Value.Width;
            int playerHeight = _player.CollisionRect().Value.Height;

            // Room collision detection test. Limit movement to match inner rectangle (floor) dimensions
            if (_player.getPosition().X < _currRoom.floor.X)
                _player.jumpTo(new Vector2(_currRoom.floor.X, playerY));
            if (_player.getPosition().X > (_currRoom.floor.X + _currRoom.floor.Width - playerWidth))
                _player.jumpTo(new Vector2((_currRoom.floor.X + _currRoom.floor.Width - playerWidth), playerY));
            if (_player.getPosition().Y < _currRoom.floor.Y)
                _player.jumpTo(new Vector2(playerX, _currRoom.floor.Y));
            if (_player.getPosition().Y > (_currRoom.floor.Y + _currRoom.floor.Height - playerHeight))
                _player.jumpTo(new Vector2(playerX, (_currRoom.floor.Y + _currRoom.floor.Height - playerHeight)));

            CheckDoorCollision();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckDoorCollision()
        {
            GObject[] doors = _currRoom.getDoors();
            foreach (GObject door in doors)
            {
                if (_player.CollisionRect().Value.Intersects(door.CollisionRect().Value))
                {
                    int doorY = door.CollisionRect().Value.Y;
                    int doorX = door.CollisionRect().Value.X;
                    int doorIndex = -1;

                    // Looking for which side door player is touching
                    if (doorY <= 0 && doorX < 1280 / 2)
                    {
                        doorIndex = getRoom(_currRoom.up);
                        direction = Direction.North;
                    }
                    else if (doorX <= 0 && doorY < 720 / 2)
                    {
                        doorIndex = getRoom(_currRoom.left);
                        direction = Direction.West;
                    }
                    else if (doorX > 1000 && doorY < 720 / 2)
                    {
                        doorIndex = getRoom(_currRoom.right);
                        direction = Direction.East;
                    }
                    else
                    {
                        doorIndex = getRoom(_currRoom.down);
                        direction = Direction.South;
                    }

                    // Changing rooms and also moving player into new room while facing correct position
                    if (doorIndex != -1)
                    {
                        _currRoom.removeObject(_player);
                        changeRoom(doorIndex);
                        // move player to new room
                        //_player = (Entity)g;
                        _currRoom.addObject(_player);
                        // Based on door entered, reposition player location
                        if (direction.Equals(Direction.North))
                            _player.jumpTo(new Vector2(1280 / 2, 720 - 190));
                        else if (direction.Equals(Direction.East))
                            _player.jumpTo(new Vector2(190, 720 / 2));
                        else if (direction.Equals(Direction.South))
                            _player.jumpTo(new Vector2(1280 / 2, 190));
                        else if (direction.Equals(Direction.West))
                            _player.jumpTo(new Vector2(1280 - 190, 720 / 2));
                    }

                    Console.Out.WriteLine("Player touched door at X:" +
                        door.CollisionRect().Value.X + " Y: " + door.CollisionRect().Value.Y);
                }
            }
        }

        public Room CreateRandRoom()
        {
            Room room;
            int mobRoomChance = 60;
            int treasureRoomChance = 10;
            int trapRoomChance = 30;

            if (mobRoomChance > r.Next(100))
            {
                room = new MonsterRoom();
            }
            else if (treasureRoomChance > r.Next(100))
            {
                room = new TreasureRoom();
            }
            else if (trapRoomChance > r.Next(100))
            {
                room = new TrapRoom();
            }
            else
            {
                room = new Room();
            }

            return room;

        }

        public int getRoom(Room room)
        {
            if (room == null)
                return -1;

            for (int i = 0; i < _rooms.Length; i++)
            {
                if (_rooms[i] == room)
                    return i;
            }
            return -1;
        }
    }
}
