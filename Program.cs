using System;

class Program
{
    static void Main()
    {
        //Calls sudokuGenerator to create a partially filled 4x4 board 
        int[,] puzzle = SudokuGenerator.GeneratePuzzle(7); // generate 6 blanks
        MiniSudoku game = new MiniSudoku(puzzle);

        Console.WriteLine("Welcome to Mini 4x4 Sudoku!");
        //Intilizes a playing loop control varaible 
        bool playing = true;

        //Main Game loop
        while (playing)
        {
            //Show current board using method from the MiniSudoku Class
            game.PrintBoard();
            // User Prompt
            Console.WriteLine("Enter your move in format: row col number (e.g. 1 2 3), or 'check' to check solution, 'undo' to undo your last move,  or 'quit' to exit.");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            //Input Handling 
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

            else if (input.ToLower() == "undo")
            {
                if (game.Undo())
                    Console.WriteLine("Undo successful.");
                else
                    Console.WriteLine("Nothing to undo.");
            }

            //This input is split into parts by spaces
            else
            {
                string[] parts = input.Split(' ');
                if (parts.Length == 3 &&
                    // Converts into 3 valid integers
                    int.TryParse(parts[0], out int row) &&
                    int.TryParse(parts[1], out int col) &&
                    int.TryParse(parts[2], out int num))
                {
                    row -= 1; // zero-based index
                    col -= 1;
                    // Check bounds and set cell
                    if (row >= 0 && row < 4 && col >= 0 && col < 4)
                    {
                        // Use setcell() to try placing number
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
        // Game ending 
        Console.WriteLine("Thanks for playing!");
    }
}
