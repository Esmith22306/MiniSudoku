using System;

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