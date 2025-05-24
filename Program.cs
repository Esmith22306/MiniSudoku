using System;
using System.Diagnostics;
using System.IO;


class Program
{
    // Method for showing past game statistics 
        static void ShowStatsSummary()
    {
        string filePath = "sudoku_stats.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("No game statistics found.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        int totalGames = 0;
        int completedGames = 0;
        int bestMoves = int.MaxValue;
        TimeSpan fastestTime = TimeSpan.MaxValue;

        foreach (string line in lines)
        {
            totalGames++;

            bool completed = line.Contains("Completed: Yes");

            if (completed)
            {
                completedGames++;

                // Extract moves
                int moveStart = line.IndexOf("Moves: ") + 7;
                int moveEnd = line.IndexOf(",", moveStart);
                string moveStr = line.Substring(moveStart, moveEnd - moveStart);
                if (int.TryParse(moveStr, out int moves) && moves < bestMoves)
                {
                    bestMoves = moves;
                }

                // Extract time
                int timeStart = line.IndexOf("Time: ") + 6;
                string timeStr = line.Substring(timeStart).Trim();
                string[] timeParts = timeStr.Split(' ');
                int mins = int.Parse(timeParts[0].TrimEnd('m'));
                int secs = int.Parse(timeParts[1].TrimEnd('s'));
                TimeSpan time = new TimeSpan(0, mins, secs);
                if (time < fastestTime)
                {
                    fastestTime = time;
                }
            }
        }

        Console.WriteLine("===== Game Stats Summary =====");
        Console.WriteLine($"Total games played: {totalGames}");
        Console.WriteLine($"Completed games: {completedGames}");
        if (completedGames > 0)
        {
            Console.WriteLine($"Best move count: {bestMoves}");
            Console.WriteLine($"Fastest time: {fastestTime.Minutes}m {fastestTime.Seconds}s");
        }
        Console.WriteLine("==============================");
    }


    static void Main()
    {
        // variable to capture when the game starts
        Stopwatch stopwatch = Stopwatch.StartNew();
        DateTime startTime = DateTime.Now;
        int moveCount = 0;
        bool gameCompleted = false;

        // Difficulty Selction (Changing number of blanks)
        Console.WriteLine("Select difficulty: Easy, Medium, Hard");
        string difficultyInput = Console.ReadLine()?.ToLower();
        int blanks;
        
        switch (difficultyInput)
        {
            case "easy":
                blanks = 4; //fewer blanks
                break;
            case "medium":
                blanks = 6;
                break;
            default:
                Console.WriteLine("Invalid choice. Defaulting to Medium");
                blanks = 6;
                break;
        }

        Console.WriteLine($"Generating a {difficultyInput?.ToUpper()} puzzle...");

        //Calls sudokuGenerator to create a partially filled 4x4 board 
        int[,] puzzle = SudokuGenerator.GeneratePuzzle(blanks); // generate 6 blanks
        MiniSudoku game = new MiniSudoku(puzzle);

        Console.WriteLine("Welcome to Mini 4x4 Sudoku!");
    
        //Intilizes a playing loop control varaible 
        bool playing = true;

        //Main Game loop
        while (playing)
        {
            // Timer function
            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Time Elapsed: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}");

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
                    TimeSpan totalTime = stopwatch.Elapsed;
                    Console.WriteLine("Congratulations! Your solution is valid!");
                    Console.WriteLine($"Total time: {totalTime.Minutes:D2}:{totalTime.Seconds:D2}");
                    gameCompleted = true;
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
                        {
                            moveCount++;
                            Console.WriteLine("Move accepted.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid move.");
                        }
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

        // Saving file for stats
        TimeSpan timeTaken = DateTime.Now - startTime;
        string timeFormatted = string.Format("{0}m {1}s", timeTaken.Minutes, timeTaken.Seconds);
        string result = gameCompleted ? "Yes" : "No";

        string logEntry = $"Date: {DateTime.Now}, Completed: {result}, Moves: {moveCount}, Time: {timeFormatted}";

        try
        {
            File.AppendAllText("sudoku_stats.txt", logEntry + Environment.NewLine);
            Console.WriteLine("Game statistics saved.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save game stats: " + ex.Message);
        }


        // Game ending 
        Console.WriteLine("Thanks for playing!");
        ShowStatsSummary(); // Show stats
    }
}
