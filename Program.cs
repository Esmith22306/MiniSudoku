using System;

class SudokuGenerator
{
    private static Random rng = new Random();

    public static int[,] GeneratePuzzle(int blanks = 6)
    {
        int[,] solution = new int[4, 4];
        FillGrid(solution);
        int[,] puzzle = (int[,])solution.Clone();
        RemoveCells(puzzle, blanks);
        return puzzle;
    }

    private static bool FillGrid(int[,] grid)
    {
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (grid[row, col] == 0)
                {
                    List<int> numbers = new List<int> { 1, 2, 3, 4 };
                    Shuffle(numbers);

                    foreach (int num in numbers)
                    {
                        if (IsValid(grid, row, col, num))
                        {
                            grid[row, col] = num;
                            if (FillGrid(grid))
                                return true;
                            grid[row, col] = 0;
                        }
                    }

                    return false; // No valid number, backtrack
                }
            }
        }

        return true; // Fully filled
    }

    private static bool IsValid(int[,] grid, int row, int col, int num)
    {
        for (int i = 0; i < 4; i++)
        {
            if (grid[row, i] == num || grid[i, col] == num)
                return false;
        }

        int startRow = (row / 2) * 2;
        int startCol = (col / 2) * 2;
        for (int r = startRow; r < startRow + 2; r++)
            for (int c = startCol; c < startCol + 2; c++)
                if (grid[r, c] == num)
                    return false;

        return true;
    }

    private static void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private static void RemoveCells(int[,] grid, int blanks)
    {
        int count = 0;
        while (count < blanks)
        {
            int row = rng.Next(4);
            int col = rng.Next(4);
            if (grid[row, col] != 0)
            {
                grid[row, col] = 0;
                count++;
            }
        }
    }
}

class MiniSudoku
{
    private int[,] board = new int[4, 4];
    private int[,] startingBoard = new int[4, 4]; // to keep track of fixed clues

    public MiniSudoku(int[,] puzzle)
    {
        Array.Copy(puzzle, startingBoard, puzzle.Length);
        Array.Copy(puzzle, board, puzzle.Length);
    }

    public void PrintBoard()
    {
        Console.WriteLine("Current Sudoku Board:");
        for (int r = 0; r < 4; r++)
        {
            if (r == 2) Console.WriteLine("------+------");
            for (int c = 0; c < 4; c++)
            {
                if (c == 2) Console.Write("| ");

                if (board[r, c] == 0)
                    Console.Write(". ");
                else
                    Console.Write(board[r, c] + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(int row, int col, int num)
    {
        if (num < 1 || num > 4) return false;
        if (startingBoard[row, col] != 0) return false; // can't change clues

        // Check row and column
        for (int i = 0; i < 4; i++)
        {
            if (board[row, i] == num) return false;
            if (board[i, col] == num) return false;
        }

        // Check 2x2 block
        int startRow = (row / 2) * 2;
        int startCol = (col / 2) * 2;
        for (int r = startRow; r < startRow + 2; r++)
            for (int c = startCol; c < startCol + 2; c++)
                if (board[r, c] == num) return false;

        return true;
    }

    public bool SetCell(int row, int col, int num)
    {
        if (IsValidMove(row, col, num))
        {
            board[row, col] = num;
            return true;
        }
        return false;
    }

    public bool IsComplete()
    {
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (board[r, c] == 0)
                    return false;
        return true;
    }

    public bool CheckSolution()
    {
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (!IsValidNumberAt(r, c, board[r, c]))
                    return false;
        return true;
    }

    private bool IsValidNumberAt(int row, int col, int num)
    {
        if (num < 1 || num > 4) return false;

        // Check row and column excluding current cell
        for (int i = 0; i < 4; i++)
        {
            if (i != col && board[row, i] == num) return false;
            if (i != row && board[i, col] == num) return false;
        }

        // Check block excluding current cell
        int startRow = (row / 2) * 2;
        int startCol = (col / 2) * 2;
        for (int r = startRow; r < startRow + 2; r++)
            for (int c = startCol; c < startCol + 2; c++)
                if ((r != row || c != col) && board[r, c] == num)
                    return false;

        return true;
    }
}
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
