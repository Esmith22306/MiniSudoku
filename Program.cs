using System;

class Program
{
    static void Main()
    {
        int[,] puzzle = SudokuGenerator.GeneratePuzzle(6); // generate 6 blanks
        MiniSudoku game = new MiniSudoku(puzzle);

        Console.WriteLine("Welcome to Mini 4x4 Sudoku!");
        bool playing = true;

        while (playing)
        {
            game.PrintBoard();
            Console.WriteLine("Enter your move in format: row col number (e.g. 1 2 3), or 'check' to check solution, or 'quit' to exit.");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
            {
                playing = false;
            }
            else if (input.ToLower() == "check")
            {
                if (!game.IsComplete())
                {
                    Console.WriteLine("Board not complete yet!");
                }
                else if (game.CheckSolution())
                {
                    Console.WriteLine("Congratulations! Your solution is valid!");
                    playing = false;
                }
                else
                {
                    Console.WriteLine("There are errors in your solution. Keep trying!");
                }
            }
            else
            {
                string[] parts = input.Split(' ');
                if (parts.Length == 3 &&
                    int.TryParse(parts[0], out int row) &&
                    int.TryParse(parts[1], out int col) &&
                    int.TryParse(parts[2], out int num))
                {
                    row -= 1; // zero-based index
                    col -= 1;

                    if (row >= 0 && row < 4 && col >= 0 && col < 4)
                    {
                        if (game.SetCell(row, col, num))
                            Console.WriteLine("Move accepted.");
                        else
                            Console.WriteLine("Invalid move.");
                    }
                    else
                    {
                        Console.WriteLine("Row and column must be between 1 and 4.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input format.");
                }
            }
        }

        Console.WriteLine("Thanks for playing!");
    }
}
