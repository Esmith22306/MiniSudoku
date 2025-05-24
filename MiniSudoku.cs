using System;
// This class will maintan the state of the board and let players place numbers and check for validity

class MiniSudoku
{
    
    private int[,] board = new int[4, 4];
    private int[,] startingBoard = new int[4, 4]; // to keep track of fixed clues
    private Stack<Move> undoStack = new Stack<Move>();


    // The constructor accepts an initial puzzle as input 
    // Makes two copies, starting board and board, to track changes the player makes.  
    public MiniSudoku(int[,] puzzle)
    {
        Array.Copy(puzzle, startingBoard, puzzle.Length);
        Array.Copy(puzzle, board, puzzle.Length);
    }

    //Storing each moves detials and what previous value was
    public struct Move 
    {
        public int Row, Col, OldValue, NewValue;

        public Move(int row, int col, int oldValue, int newValue)
        {
            Row = row;
            Col = col;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    // This will be to make the puzzle readable, and producing a standard sudoku format
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

    // Checks if placing the number in a cell follows the sudoku rules
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

    // We use IsValidMove() to check where placing num is legal, if vale return true otherwise false
    public bool SetCell(int row, int col, int num)
    {
            if (IsValidMove(row, col, num))
            {
                int oldValue = board[row, col];
                board[row, col] = num;
                undoStack.Push(new Move(row, col, oldValue, num));
                return true;
            }
            return false;
    }

    //This method will revert the last move.
        public bool Undo()
        {
            if (undoStack.Count > 0)
            {
                Move lastMove = undoStack.Pop();
                board[lastMove.Row, lastMove.Col] = lastMove.OldValue;
                return true;
            }
            return false;
        }



    //check if all cells are non-zero
    // reuturns true is full filled
    public bool IsComplete()
    {
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (board[r, c] == 0)
                    return false;
        return true;
    }

    //Uses helped IsValidNumberAt() ti check entire board
    public bool CheckSolution()
    {
        //Loops over every cell and calls IsValidNumber(row, col, board[row, col])
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (!IsValidNumberAt(r, c, board[r, c]))
                    return false;
        return true;
    }

    //Checks if a number is alread on the board
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