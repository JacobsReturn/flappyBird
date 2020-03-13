using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib;
using static Raylib.Raylib;

namespace Project2D
{
    class Player
    {
        public Texture2D text;

        public Texture2D up;
        public Texture2D down;

        public Vector2 location;
        public float rotation = 0.0f;
        public float size = 1f;
        public bool dead = true;

        public float score = 0f;
        public float highScore = 0f;

        public Player()
        {

        }

        public float toLocation = 0.0f;
        public float toRotation = 0.0f;
        public float toGameOver = 0.0f;

        public void Kill()
        {
            dead = true;
            Game.started = false;
        }

        public void Collide()
        {
            foreach (Obstacle ob in Game.obstacles)
            {
                float rotate = 0.0f;
                float rotate2 = 0.0f;
                
                if (!ob.infinite)
                {
                    rotate = ob.sprite.height * ob.rotation / 180;
                    rotate2 = ob.location.x - ob.sprite.width * ob.rotation / 180;
                }

                Rectangle obj = new Rectangle(rotate2, ob.location.y - rotate, ob.texture.width, ob.texture.height);
                Rectangle ply = new Rectangle(this.location.x, this.location.y, this.text.width, this.text.height);

                if (ob.collideable & CheckCollisionRecs(obj, ply))
                {
                    Kill();
                    break;
                }
                else if (!ob.hit & !dead & Game.started)
                {
                    Rectangle wholeObj = new Rectangle(ob.location.x, 0, ob.texture.width, GetScreenHeight());

                    if (CheckCollisionRecs(wholeObj, ply))
                    {
                        score++;
                        if ((score - 1) > highScore)
                        {
                            highScore = score - 1;
                        }

                        ob.hit = true;
                        if (ob.top != null) ob.top.hit = true;
                        if (ob.bottom != null) ob.top.hit = true;
                    }
                }
            }
        }

        public void Draw()
        {
            if (!dead)
            {
                if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    toLocation -= Game.gravity * 2000;
                    toRotation = -90f;

                    text = up;
                }
                else
                {
                    toLocation += Game.gravity;
                    if (-30 == Math.Round(rotation, 0))
                    {
                        toRotation = 60f;

                        text = down;
                    }
                }

                location.y = Lerp(location.y, toLocation, 0.001f);
                rotation = Lerp(rotation, toRotation, 0.0001f);
            }
            
            DrawTextureEx(text, location, rotation, size, Color.WHITE);
            if (score > 0)
            {
                DrawText("Score: " + (score - 1).ToString(), 2, 0, 30, Color.WHITE);
            }
            else
            {
                DrawText("Score: " + score.ToString(), 2, 0, 30, Color.WHITE);
            }

            DrawText("High: " + highScore.ToString(), 2, 42, 30, Color.WHITE);
            
            if (dead)
            {
                toGameOver = Lerp(toGameOver, 1, 0.001f);

                if (toGameOver > 0.9)
                {
                    toLocation += Game.gravity;
                    toRotation = 180f;

                    location.y = Lerp(location.y, toLocation, 0.001f);
                    rotation = Lerp(rotation, toRotation, 0.0001f);
                }

                if (!Game.started)
                {
                    DrawTextureEx(Game.gameOver, new Vector2(GetScreenWidth() / 2 - Game.gameOver.width / 2, (GetScreenHeight() / 2 - Game.gameOver.height / 2) * toGameOver), 0.0f, 1.0f, Color.WHITE);
                }

                if (!Game.started & IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Game.started = true;
                }
                else if (Game.started)
                {            
                    DrawTextureEx(Game.clickStart, new Vector2(GetScreenWidth() / 2 - Game.clickStart.width / 2, GetScreenHeight() / 2 - Game.clickStart.height / 2), 0.0f, 1.0f, Color.WHITE);

                    if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                    {
                        dead = false;

                        Game.Start();
                    }
                    
                }
            }
        }
    }
}
