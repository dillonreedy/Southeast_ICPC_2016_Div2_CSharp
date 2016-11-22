using System;
using System.Collections.Generic;

namespace Islands
{
  /// <summary>
  /// Just a simple point class that is used to find point locations.
  /// </summary>
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
      return this.x == otherPt.x && this.y == otherPt.y;
    }
  }

  /// <summary>
  ///  A solution to the ICPC 2016 Problem Islands. The solution uses a breadth first search
  ///  to find if certain points can connect. Then from that a undirected matrix is made from
  ///  those values. With the undirected matrix, we make groups of islands that are connected.
  ///  Then finally we count the number of groups that there are.
  /// </summary>
  public class Program
  {
    #region Private Variables

    private char[,] grid;
    private int r, c;
    #endregion

    public static void Main(string[] args)
    {
      new Program().doit();
    }

    public void doit()
    {
      String[] line = Console.ReadLine().Split(' ');
      r = Int32.Parse(line[0]);
      c = Int32.Parse(line[1]);

      grid = new char[r, c];

      for (int i = 0; i < r; i++)
      {
        char[] values = Console.ReadLine().ToCharArray();
        for (int j = 0; j < c; j++) grid[i, j] = values[j];
      }

      if (!GridContainsChar('L')) Console.WriteLine("0");
      else Console.WriteLine(GetNumberOfIslands());

    }

    public int GetNumberOfIslands()
    {
      List<Point> pts = GetAllPointsOfCharFromGrid('L');
      bool[,] IsConnected = new bool[pts.Count, pts.Count];
      List<List<Point>> groups = new List<List<Point>>();
      Dictionary<Point, bool> beenHere = new Dictionary<Point, bool>();

      foreach (Point pt in pts) beenHere.Add(pt, false);

      for (int i = 0; i < pts.Count; i++)
        for (int j = i + 1; j < pts.Count; j++)
          IsConnected[i, j] = CanConnect(pts[i], pts[j]);
      
      for (int i = 0; i < pts.Count; i++)
      {
        List<Point> curGroup = new List<Point>();

        if (!beenHere[pts[i]]) SetCurrentGroupAndBeenHere(ref curGroup, ref beenHere, pts, i, IsConnected);
        if (curGroup.Count != 0) groups.Add(curGroup);

        beenHere[pts[i]] = true;
      }
      return groups.Count;
    }

    public void SetCurrentGroupAndBeenHere(ref List<Point> curGroup, ref Dictionary<Point, bool> beenHere, List<Point> pts, int i, bool[,] IsConnected)
    {
      curGroup.Add(pts[i]);
      for (int j = 0; j < pts.Count; j++)
      {
        if (IsConnected[i, j])
        {
          curGroup.Add(pts[j]);
          beenHere[pts[j]] = true;
        }
      }
    }
    
    public bool CanConnect(Point start, Point end)
    {
      Stack<Point> moves = new Stack<Point>();
      moves.Push(start);
      bool[,] beenHere = new bool[r, c];

      while (moves.Count != 0)
      {
        Point curLoc = moves.Pop();
        if (!beenHere[curLoc.x, curLoc.y]) AddMoves(curLoc, ref moves);
        if (curLoc.IsEqual(end)) return true;
        beenHere[curLoc.x, curLoc.y] = true;
      }
      return false;
    }

    public void AddMoves(Point curLoc, ref Stack<Point> curMoves)
    {
      List<Point> possibleMoves = new List<Point>();
      possibleMoves.Add(new Point(curLoc.x, curLoc.y + 1));
      possibleMoves.Add(new Point(curLoc.x, curLoc.y - 1));
      possibleMoves.Add(new Point(curLoc.x + 1, curLoc.y));
      possibleMoves.Add(new Point(curLoc.x - 1, curLoc.y));

      foreach (Point possibleMove in possibleMoves) if (CanMove(possibleMove)) curMoves.Push(possibleMove);

    }

    public bool CanMove(Point givenPt)
    {
      return (InBorders(givenPt) && !grid[givenPt.x, givenPt.y].Equals('W'));
    }

    public bool InBorders(Point givenPt)
    {
      return (0 <= givenPt.x && givenPt.x < r && 0 <= givenPt.y && givenPt.y < c);
    }

    public bool GridContainsChar(char ch)
    {
      for (int i = 0; i < r; i++)
        for (int j = 0; j < c; j++)
          if (grid[i, j] == ch) return true;

      return false;
    }

    public List<Point> GetAllPointsOfCharFromGrid(char ch)
    {
      List<Point> pts = new List<Point>();

      for (int i = 0; i < r; i++)
        for (int j = 0; j < c; j++)
          if (grid[i, j] == ch) pts.Add(new Point(i, j));

      return pts;
    }
  }
}
