using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buggy
{
  public class Point
  {
    public int x, y;

    public Point(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public bool IsEqual(Point otherPt)
    {
      return ((this.x == otherPt.x) && (this.y == otherPt.y));
    }
  }

  class Program
  {
    private int r, c;
    private char[,] grid;
    private string instructions;

    static void Main(string[] args)
    {
      new Program().doit();
    }

    public Point GetLocationOfChar(char ch)
    {
      for (int i = 0; i < r; i++)
        for (int j = 0; j < c; j++)
          if (grid[i, j] == ch) return new Point(i, j);

      return null;
    }

    public List<String> GetAllPaths()
    {
      Point start = GetLocationOfChar('R');
      Point end = GetLocationOfChar('E');
      List<String> paths = new List<String>();
      bool[,] beenHere = new bool[r, c];

      BFS(start, end, String.Empty, ref paths, ref beenHere);

      return paths;
    }

    public void BFS(Point start, Point end, String currentInstructions, ref List<String> paths, ref bool[,] beenHere)
    {
      beenHere[start.x, start.y] = true;

      if (start.IsEqual(end))
      {
        paths.Add(currentInstructions);
      }
      else
      {
        Point downMove = new Point(start.x, start.y + 1);
        Point upMove = new Point(start.x, start.y - 1);
        Point rightMove = new Point(start.x + 1, start.y);
        Point leftMove = new Point(start.x - 1, start.y);

        if (CanMove(upMove) && !beenHere[upMove.x, upMove.y]) BFS(upMove, end, currentInstructions + "U", ref paths, ref beenHere);
        if (CanMove(downMove) && !beenHere[downMove.x, downMove.y]) BFS(downMove, end, currentInstructions + "D", ref paths, ref beenHere);
        if (CanMove(rightMove) && !beenHere[rightMove.x, rightMove.y]) BFS(rightMove, end, currentInstructions + "R", ref paths, ref beenHere);
        if (CanMove(leftMove) && !beenHere[leftMove.x, leftMove.y]) BFS(leftMove, end, currentInstructions + "L", ref paths, ref beenHere);
      }

      beenHere[start.x, start.y] = false;
    }

    public bool CanMove(Point curLocation)
    {
      return (InBounds(curLocation) && !grid[curLocation.x, curLocation.y].Equals('#'));
    }

    public bool InBounds(Point curLocation)
    {
      return (0 <= curLocation.x && curLocation.x < r && 0 <= curLocation.y && curLocation.y < c);
    }

    public int GetMin(int num1, int num2, int num3)
    {
      return Math.Min(num1, Math.Min(num2, num3));
    }

    public int getEditDistance(string word1, string word2)
    {
      int cols = word1.Length + 1;
      int rows = word2.Length + 1;
      int[,] matrix = new int[rows, cols];

      for (int i = 0; i < cols; i++) matrix[0, i] = i;
      for (int i = 0; i < rows; i++) matrix[i, 0] = i;

      for (int i = 1; i < rows; i++)
      {
        for (int j = 1; j < cols; j++)
        {
          int value;

          if (word1[j - 1] == word2[i - 1]) value = matrix[i - 1, j - 1];
          else value = GetMin(matrix[i - 1,j - 1], matrix[i - 1, j], matrix[i, j - 1]) + 1;

          matrix[i, j] = value;
        }
      }

      return matrix[rows - 1, cols - 1];
    }

    public void guessWord(string word, List<String> dictionary, ref string bestGuess, ref int bestScore)
    {
      foreach (string s in dictionary)
      {
        int value = getEditDistance(word, s);
        if (value == 0)
        {
          bestGuess = word;
          bestScore = 0;
          return;
        }
        if (value < bestScore)
        {
          bestGuess = s;
          bestScore = value;
        }
      }
    }

    public void doit()
    {
      string[] line = Console.ReadLine().Split(' ');
      r = Int32.Parse(line[0]);
      c = Int32.Parse(line[1]);
      grid = new char[r, c];

      for (int i = 0; i < r; i++)
      {
        char[] chars = Console.ReadLine().ToCharArray();
        for (int j = 0; j < c; j++) grid[i, j] = chars[j];
      }

      string givenGuess = Console.ReadLine();



      List<String> paths = GetAllPaths();

      string bestGuess = null;
      int bestScore = Int32.MaxValue;

      guessWord(givenGuess, paths, ref bestGuess, ref bestScore);
      Console.WriteLine("From your guess: " + givenGuess);
      Console.WriteLine("The closest answer to your guess would be: " + bestGuess);
      Console.WriteLine("The minimum number of edits that would need to be made are: " + bestScore);
      Console.ReadKey();

    }
  }
}
