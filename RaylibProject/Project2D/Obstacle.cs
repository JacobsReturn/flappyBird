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
    class Obstacle
    {
        public Vector2 location;
        public Image sprite;
        public Texture2D texture;
        public float speed = 0.0f;
        public float rotation = 0.0f;
        public bool hit = false;

        public bool collideable = false;

        public Obstacle top;
        public Obstacle bottom;

        public bool infinite = false;
        
        public Obstacle(float x, float y, string image, bool inf, float speed, bool collideable, float rotation)
        {
            this.speed = speed;
            sprite = LoadImage(image);

            location.x = x + sprite.width * rotation/180;
            location.y = y;

            this.rotation = rotation;

            this.collideable = collideable;

            if (inf)
            {
                ImageCrop(ref sprite, new Rectangle(0, 0, 640 * 2, sprite.height));
            }

            texture = LoadTextureFromImage(sprite);

            UnloadImage(sprite);

            infinite = inf;

            Game.obstacles.Add(this);
        }

        public void Remove()
        {
            Game.Generate(top, bottom);
        }

        public void Update()
        {
            if (!Game.player.dead)
            {
                if (location.x < -sprite.width & !infinite)
                {
                    Remove();
                }
            }
        }

        public void Draw()
        {
            if (!Game.player.dead)
            {
                location.x -= speed;
                if (location.x < -640)
                {
                    if (infinite)
                    {
                        location.x = 0;
                    }
                }
            }

            if (infinite)
            {
                DrawTextureRec(texture, new Rectangle(0, 0, 640 * 2, sprite.height), location, Color.WHITE);
            }
            else
            {
                DrawTextureEx(texture, location, rotation, 1.0f, Color.WHITE);
            }
        }
    }
}
