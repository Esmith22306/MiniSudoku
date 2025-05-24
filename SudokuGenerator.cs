using System;

class SudokuGenerator
{

    // rng is going to be a static random number generator shared acrss all methods in this class. 
    private static Random rng = new Random();
    // Blanks optional argument how many cells are blank
    public static int[,] GeneratePuzzle(int blanks = 6)
    {
        // Create 4x4 array and use the fillgrid method for a valid solution
        int[,] solution = new int[4, 4];
        FillGrid(solution);
        //Clone solution so the orginal solution is intact
        int[,] puzzle = (int[,])solution.Clone();
        // Remove cell method to create the final puzzle
        RemoveCells(puzzle, blanks);
        return puzzle;
    }

    // Recursive backtracking algorthirm 
    // Place number 1 through 4 in random order 
    // Only place if its valid. For dead ends we back track. 

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


                    // Loop over every cell in the 4x4 grid 
                    // If 0 then create shuffled list to try
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

    // This method will check if placing num at position (row, col) is valid

    private static bool IsValid(int[,] grid, int row, int col, int num)
    {
        // Row and column check
        for (int i = 0; i < 4; i++)
        {
            if (grid[row, i] == num || grid[i, col] == num)
                return false;
        }

        // 2x2 subgrid check
        int startRow = (row / 2) * 2;
        int startCol = (col / 2) * 2;
        for (int r = startRow; r < startRow + 2; r++)
            for (int c = startCol; c < startCol + 2; c++)
                if (grid[r, c] == num)
                    return false;

        return true;
    }

    // Randomizes the order of numbers 
    // Fischer yates shuffle algortihm 
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


    // Sets to 0 a number cells specified by blanks. 
    // Keep removing random cells until count == blanks. 

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
