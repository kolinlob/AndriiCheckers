﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public List<Checker> checkersSet = new List<Checker>();
        private Board board;

        public void Start()
        {
            board = new Board();

            CreateCheckers("whites");
            CreateCheckers("blacks");
            board.Draw(checkersSet);

            while (true)
                MakeMove(board);
        }

        public void CreateCheckers(string color = "whites")
        {
            int start = 0;
            int end = 3;
            bool isWhite = true;

            if (color == "blacks")
            {
                start = 5;
                end = 8;
                isWhite = false;
            }

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                        checkersSet.Add(new Checker(isWhite, false, i, j));
                }
            }
        }

        public void MakeMove(Board board)
        {
            Console.SetCursorPosition(0, 28);
            Console.Write("\r\nВыберите шашку (например, B6): ");
            int[] adressOld = SelectCell();
            int selectedRowOld = adressOld[0];
            int selectedColOld = adressOld[1];

            Console.Write("Целевая клетка (например, С5): ");
            foreach (var checker in checkersSet)
            {
                if (selectedRowOld == checker.HorizontalCoord && selectedColOld == checker.VerticalCoord)
                {
                    int[] adressNew = SelectCell();
                    if (CanMove(adressNew))
                    {
                        checker.HorizontalCoord = adressNew[0];
                        checker.VerticalCoord = adressNew[1];
                        CheckerBecomesQueen(checker);
                    }
                    else
                    {
                        Console.WriteLine("Нельзя ходить в выбранную клетку!");
                        //return;
                    }
                }
            }

            const int delayNpcMoveMiliseconds = 500;
            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);
            board.Draw(checkersSet);

            Console.SetCursorPosition(0, 28);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 45; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public bool CanMove(int[] adressNew)
        {
            return (board.IsEmpty(adressNew[0], adressNew[1]) && board.IsUsable(adressNew[0], adressNew[1]));
        }

        public int[] SelectCell()
        {
            Encoding ascii = Encoding.ASCII;
            string input = Console.ReadLine();
            //int selectedCheckerCol = 0;
            //int selectedCheckerRow = 0;

            while (input == null || !IsOfCorrectLength(input))// || selectedCheckerCol < 0 || selectedCheckerCol > 7 || selectedCheckerRow < 0 || selectedCheckerRow > 7)
            {
                const string incorrectInputError = "Некорректный ввод. Повторите попытку: ";
                Console.Write(incorrectInputError);

                input = Console.ReadLine();
            }

            Byte[] encodedBytes = ascii.GetBytes(input.ToUpper());

            int selectedCheckerCol = Convert.ToInt32(encodedBytes[0]) - 65;
            int selectedCheckerRow = 56 - Convert.ToInt32(encodedBytes[1]);

            int[] adress = { selectedCheckerRow, selectedCheckerCol };
            return adress;
        }

        private bool IsOfCorrectLength(string input)
        {
            return (input.Length == 2);
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((checker.IsWhite && checker.HorizontalCoord == 7) || (!checker.IsWhite && checker.HorizontalCoord == 0))
            {
                checker.IsQueen = true;
                checker.ChageSymbol();
            }
        }
    }
}