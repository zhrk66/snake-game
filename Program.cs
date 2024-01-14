using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace Snake
{
    class Program
    {
        static void Main()
        {
            WindowHeight = 20;
            WindowWidth = 32;

            Random rnd = new Random();

            var score = 5;

            var head = new Pixel(WindowWidth / 2, (WindowHeight / 2) - 3, ConsoleColor.DarkRed);

            var food = new Pixel(rnd.Next(1, WindowWidth - 2) - rnd.Next((WindowWidth / 2 - 1), (WindowWidth / 2) + 1), rnd.Next(1, WindowHeight - 2) - rnd.Next((WindowHeight / 2)), ConsoleColor.Red);

            var body = new List<Pixel>();

            var movement = Direction.Right;

            var gameover = false;


            while (true)
            {
                Clear();

                gameover |= (head.XPos == WindowWidth - 1 || head.XPos == 0 || head.YPos == WindowHeight - 1 || head.YPos == 0 || ((head.XPos == (WindowWidth / 2 - 1) || head.XPos == (WindowWidth / 2) || head.XPos == (WindowWidth / 2) + 1) && head.YPos == WindowHeight / 2));

                DrawBorder();
                DrawMiddleWall();

                if (food.XPos == head.XPos && food.YPos == head.YPos)
                {
                    score++;
                    food = new Pixel(rnd.Next(1, WindowWidth - 2), rnd.Next(1, WindowHeight - 2), ConsoleColor.Red);
                }

                for (int i = 0; i < body.Count; i++)
                {
                    DrawBody(body[i]);
                    gameover |= (body[i].XPos == head.XPos && body[i].YPos == head.YPos);
                }

                if (gameover)
                {
                    break;
                }

                DrawBody(head);

                DrawFood(food);

                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds <= 500)
                {

                    movement = ReadMovement(movement);
                }

                body.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));

                switch (movement)
                {
                    case Direction.Up:
                        head.YPos--;
                        break;
                    case Direction.Down:
                        head.YPos++;
                        break;
                    case Direction.Left:
                        head.XPos--;
                        break;
                    case Direction.Right:
                        head.XPos++;
                        break;
                }

                if (body.Count > score)
                {
                    body.RemoveAt(0);
                }
            }
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            WriteLine($"Game over, Score:{ score - 5}");
            ReadKey();
        }

        static Direction ReadMovement(Direction movement)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && movement != Direction.Down)
                {
                    movement = Direction.Up;
                }
                else if (key == ConsoleKey.DownArrow && movement != Direction.Up)
                {
                    movement = Direction.Down;
                }
                else if (key == ConsoleKey.LeftArrow && movement != Direction.Right)
                {
                    movement = Direction.Left;
                }
                else if (key == ConsoleKey.RightArrow && movement != Direction.Left)
                {
                    movement = Direction.Right;
                }
            }

            return movement;
        }

        static void DrawBody(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Write("@");
            SetCursorPosition(0, 0);
        }
        static void DrawFood(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            if ((pixel.XPos == (WindowWidth / 2 - 1) || pixel.XPos == (WindowWidth / 2) || pixel.XPos == (WindowWidth / 2) + 1) && pixel.YPos == WindowHeight / 2)
            {
                pixel.YPos--;
            }
            Write("@");
            SetCursorPosition(0, 0);
        }

        static void DrawBorder()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                ForegroundColor = ConsoleColor.Yellow;
                SetCursorPosition(i, 0);
                Write("#");

                SetCursorPosition(i, WindowHeight - 1);
                Write("#");
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("#");

                SetCursorPosition(WindowWidth - 1, i);
                Write("#");
            }
        }

        static void DrawMiddleWall()
        {
            for (int i = (WindowWidth / 2) - 1; i < (WindowWidth / 2) + 2; i++)
            {
                ForegroundColor = ConsoleColor.Yellow;
                SetCursorPosition(i, (WindowHeight / 2));
                Write("#");
            }
        }

        class Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                ScreenColor = color;
            }
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ScreenColor { get; set; }
        }

        enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}

