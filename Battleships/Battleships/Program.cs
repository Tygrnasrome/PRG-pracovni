﻿using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleships 
{
    internal class Program
    {
		static int BattlefieldSize = 12;
        static Battlefield PlayerBf = new Battlefield(BattlefieldSize, [0,12]);
        static Battlefield AIBf = new Battlefield(BattlefieldSize, [30, 12]);

		static AI Svatka = new AI(BattlefieldSize);

        static int[] SelectedTile = {0, 0};
        static bool Valid_placement = true;
        static int[] playerBfLoc = PlayerBf.Location;
        static void Main(string[] args)
        {
			Console.CursorVisible = false;
			while (true)
			{
				Init();
				// placing ships
				AIBf.PlaceAllShips();
				while (!PlayerBf.AllShipsPlaced())
				{
					Valid_placement = true;
					Console.SetCursorPosition(0, 0); // Reset cursor for redraw, Console.Clear() causes too many rendering - slow
					PlaceRender();
					PlayerInput();
				}
				Console.Clear();
                PlayerBf.DrawBattlefield("Hrac", false);
                Console.SetCursorPosition(0, 0);

				while (Svatka.Difficulty == 0)
				{
					WriteBattleships();
					Console.WriteLine("Zadej obtiznost AI protivnika (1-2)");
					Console.WriteLine("1 - very easy");
					Console.WriteLine("2 - normal");

                    switch (Console.ReadKey(intercept: true).Key)
					{
						case ConsoleKey.D1:
							Svatka.Difficulty = 1;
							break;
						case ConsoleKey.D2:
							Svatka.Difficulty = 2;
							break;
						default:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Nespravny vstup");
							Console.ResetColor();
							break;
					}
					Console.SetCursorPosition(0, 0);
				}
				Console.Clear();

				// game loop
				while (!PlayerBf.AllShipsDestroyed() && !AIBf.AllShipsDestroyed())
				{
					Console.SetCursorPosition(0, 0);
					GameRender();
					PlayerGameInput();
				}
                Console.SetCursorPosition(0, 0);
                GameRender();
                if (AIBf.AllShipsDestroyed())
				{
                    // ChatGPT generated
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("░       ░  ░░░  ░   ░");
                    Console.WriteLine("░       ░   ░   ░░  ░");
                    Console.WriteLine("░   ░   ░   ░   ░ ░ ░");
                    Console.WriteLine(" ░ ░ ░ ░    ░   ░  ░░");
                    Console.WriteLine("  ░   ░    ░░░  ░   ░");
                    Console.ResetColor();
                }
                else
				{
                    // ChatGPT generated
					Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("░░░░   ░░░░░  ░░░░░  ░░░░░  ░░░░░  ░░░░░");
                    Console.WriteLine("░   ░  ░      ░      ░      ░   ░    ░  ");
                    Console.WriteLine("░   ░  ░░░░   ░░░░   ░░░░   ░░░░░    ░  ");
                    Console.WriteLine("░   ░  ░      ░      ░      ░   ░    ░  ");
                    Console.WriteLine("░░░░   ░░░░░  ░      ░░░░░  ░   ░    ░  ");
					Console.ResetColor();
                }
				Console.WriteLine("Pro pokracovani stisknete libovolne tlacitko");
				Console.ReadKey(intercept: true);
			}
        }
		static void WriteBattleships()
		{
            // ChatGTP generated
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("░░░░░   ░░░░░  ░░░░░  ░░░░░  ░      ░░░░░   ░░░░   ░   ░  ░░░   ░░░░    ░░░░  ");
            Console.WriteLine("░   ░   ░   ░    ░      ░    ░      ░      ░       ░   ░   ░    ░   ░  ░      ");
            Console.WriteLine("░░░░░   ░░░░░    ░      ░    ░      ░░░░    ░░░░   ░░░░░   ░    ░░░░    ░░░░  ");
            Console.WriteLine("░   ░   ░   ░    ░      ░    ░      ░           ░  ░   ░   ░    ░           ░ ");
            Console.WriteLine("░░░░░   ░   ░    ░      ░    ░░░░░  ░░░░░   ░░░░   ░   ░  ░░░   ░       ░░░░  \n");
            Console.ResetColor();
        }
        static void Init()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PlayerBf.Active = true;

            PlayerBf = new Battlefield(12, [0, 12]);
            AIBf = new Battlefield(12, [30, 12]);

            SelectedTile = [0, 0];
            Valid_placement = true;
            Svatka = new AI(BattlefieldSize);
            Console.Clear();
        }
        static void PlaceRender()
        {
            WriteBattleships();
            Console.WriteLine("Pomoci sipek/WSAD, enter zvolte rozmisteni lodi");
			Console.WriteLine("Muzete pouzit autofill klavesou Tab\n");
			Console.WriteLine("Lode se nesmi navzajem prekryvat ani dotykat stranama");
            
            PlayerBf.DrawBattlefield("Hrac");
        }
		
		static void GameRender()
		{
            WriteBattleships();

            Console.WriteLine("Pomoci sipek, WSAD, enter namirte a strelte");
			PlayerBf.Location = playerBfLoc;
			PlayerBf.DrawBattlefield("Hrac", false);
			AIBf.DrawBattlefield("AI opponent", false, false);
			AIBf.DrawSelectedField(SelectedTile);

			PlayerBf.Location = [60, 12];
			PlayerBf.DrawBattlefield("Pohled AI", false, false);
		}
        
        static void PlayerInput()
        {
            // Read key without displaying it

            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    PlayerBf.SelectedShip.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    PlayerBf.SelectedShip.MoveDown(PlayerBf.Size);
                    break;
                case ConsoleKey.LeftArrow:
					PlayerBf.SelectedShip.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    PlayerBf.SelectedShip.MoveRight(PlayerBf.Size);
                    break;
                case ConsoleKey.Enter:
					if (Valid_placement)
						PlayerBf.PlaceSelectedShip();
                    break;
				case ConsoleKey.A:
				case ConsoleKey.W:
					PlayerBf.SelectedShip.Rotate(PlayerBf.Size);
					break;
				case ConsoleKey.D:
				case ConsoleKey.S:
					PlayerBf.SelectedShip.Rotate(PlayerBf.Size);
					break;
				case ConsoleKey.Tab:
					PlayerBf.PlaceAllShips();
					break;
			}
        }
	
		static void PlayerGameInput()
		{
			switch (Console.ReadKey(intercept: true).Key)
			{
				case ConsoleKey.UpArrow:
					if (SelectedTile[1] != 0)
						SelectedTile[1] -= 1;
					break;
				case ConsoleKey.DownArrow:
					if (SelectedTile[1] != PlayerBf.Size - 1)
						SelectedTile[1] += 1;
					break;
				case ConsoleKey.LeftArrow:
					if (SelectedTile[0] != 0)
						SelectedTile[0] -= 1;
					break;
				case ConsoleKey.RightArrow:
					if (SelectedTile[0] != PlayerBf.Size - 1)
						SelectedTile[0] += 1;
					break;
				case ConsoleKey.Enter:
					AIBf.DestroyField(SelectedTile);
					AIGameInput();
					break;

			}
		}

		static void AIGameInput()
		{
			Svatka.Shoot(ref PlayerBf);
		}
	}
}
