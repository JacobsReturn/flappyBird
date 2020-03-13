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
    class Game
    {
        public static float speed = 0.01f;
        public static float gravity = 0.04f;
        public static bool started = false;
        public static bool starting = false;
        public static int gap = 50;
        public static float pipeGap = 5f;

        public static Player player = new Player();

        public static List<Obstacle> obstacles = new List<Obstacle>();

        public static Texture2D gameOver;
        public static Texture2D clickStart;

        public Game()
        {
        }

        private static float oldX;
        private static float oldY;

        public static void Generate(Obstacle top, Obstacle bottom)
        {
            oldX += 52 * 7f;
            oldY = GetScreenHeight() * GetRandomValue(2, 6) / 10;

            top.location.x = oldX + (top.sprite.width * top.rotation / 180);
            top.location.y = oldY - gap;

            bottom.location.x = oldX;
            bottom.location.y = oldY + gap;
            oldX = GetScreenWidth() + 52 * 4f;

            top.hit = false;
            bottom.hit = false;
        }

        public static void Generate()
        {
            oldX += 52 * 4f;
            oldY = GetScreenHeight() * GetRandomValue(2, 6)/10;

            Obstacle bottom = new Obstacle(oldX, oldY + gap, "../Images/pipe.png", false, speed + 0.01f, true, 0);
            Obstacle top = new Obstacle(oldX, oldY - gap, "../Images/pipe.png", false, speed + 0.01f, true, 180f);

            bottom.bottom = bottom;
            bottom.top = top;

            top.bottom = bottom;
            top.top = top;
        }

        public static void Start()
        {
            player.toLocation = 0.0f;
            player.toRotation = 0.0f;
            player.toGameOver = 0.0f;
            player.score = 0;

            player.location = new Vector2(GetScreenWidth() / 2 - player.text.width / 2, GetScreenHeight() / 2 - player.text.height / 2);
            player.rotation = 0.0f;

            obstacles = new List<Obstacle>();

            oldX = GetScreenWidth();
            new Obstacle(0, 0, "../Images/background-day.png", true, speed, false, 0);
            new Obstacle(0, GetScreenHeight() - 110, "../Images/base.png", true, speed + 0.01f, true, 0);

            Generate();
            Generate();
            Generate();
            Generate();
            Generate();
            Generate();

            oldX = GetScreenWidth() + 52 * 4f;

            started = true;
        }

        public void Init()
        {
            gameOver = LoadTextureFromImage(LoadImage("../Images/gameover.png"));
            clickStart = LoadTextureFromImage(LoadImage("../Images/message.png"));

            SetWindowIcon(LoadImage("../Images/player.png"));

            player.up = LoadTextureFromImage(LoadImage("../Images/player.png"));
            player.down = LoadTextureFromImage(LoadImage("../Images/player_down.png"));
            player.text = player.up;

            Start();
        }

        public void Shutdown()
        {
        }

        public void Update()
        {
            if (started)
            {
                player.Collide();

                foreach (Obstacle ob in obstacles)
                {
                    ob.Update();
                }
            }
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);

            foreach(Obstacle ob in obstacles)
            {
                ob.Draw();
            }

            player.Draw();

            EndDrawing();
        }

    }
}
