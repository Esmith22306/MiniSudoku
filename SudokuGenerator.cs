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
