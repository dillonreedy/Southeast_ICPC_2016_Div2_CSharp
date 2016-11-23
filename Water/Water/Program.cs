using System;
using System.Collections.Generic;

namespace Water
{
  class Program
  {
    #region Private Variables
    private int n, p, k;
    private int source = 0;
    private int sink = 1;
    private int[,] flows;
    #endregion

    static void Main(string[] args)
    {
      new Program().doit();
    }

    public int FordFulkerson()
    {
      int[,] rGraph = new int[n, n];
      int[] parent = new int[n];
      int max_flow = 0;
      
      for (int u = 0; u < n; u++)
        for (int v = 0; v < n; v++)
          rGraph[u, v] = flows[u, v];
      
      while (IsPath(rGraph, source, sink, ref parent))
      {
        int minPathFlow = GetMinFlowOnPath(parent, rGraph);
        ReverseFlowsOnResidualGraph(ref rGraph, parent, minPathFlow);
        max_flow += minPathFlow;
      }

      return max_flow;
    }

    public void ReverseFlowsOnResidualGraph(ref int[,] rGraph, int[] parents, int minPathFlow)
    {
      for (int v = sink; v != source; v = parents[v])
      {
        int u = parents[v];
        rGraph[u, v] -= minPathFlow;
        rGraph[v, u] += minPathFlow;
      }
    }

    public int GetMinFlowOnPath(int[] parents, int[,] rGraph)
    {
      int minPathFlow = Int32.MaxValue;
      for (int v = sink; v != source; v = parents[v])
      {
        int u = parents[v];
        minPathFlow = Math.Min(minPathFlow, rGraph[minPathFlow, rGraph[u, v]]);
      }

      return minPathFlow;
    }

    public bool IsPath(int[,] rGraph, int s, int t, ref int[] parent)
    {
      bool[] visited = new bool[n];
      Queue<int> q = new Queue<int>();
      q.Enqueue(s);
      visited[s] = true;
      parent[s] = -1;

      while (q.Count != 0)
      {
        int u = q.Dequeue();

        for (int v = 0; v < n; v++)
        {
          if (!visited[v] && 0 < rGraph[u, v])
          {
            q.Enqueue(v);
            parent[v] = u;
            visited[v] = true;
          }
        }
      }

      return visited[t];
    }
    
    public void doit()
    {
      String[] line = Console.ReadLine().Split(' ');
      n = Int32.Parse(line[0]);
      p = Int32.Parse(line[1]);
      k = Int32.Parse(line[2]);
      flows = new int[n, n];
      
      for (int i = 0; i < p; i++)
      {
        line = Console.ReadLine().Split(' ');
        int a = Int32.Parse(line[0]) - 1;
        int b = Int32.Parse(line[1]) - 1;
        int c = Int32.Parse(line[2]);

        if (a == source) flows[a, b] = c;
        else if (a == sink) flows[b, a] = c;
        else { flows[a, b] = c; flows[b, a] = c; }
      }

      Console.WriteLine(FordFulkerson());

      for (int i = 0; i < k; i++)
      {
        line = Console.ReadLine().Split(' ');
        int a = Int32.Parse(line[0]) - 1;
        int b = Int32.Parse(line[1]) - 1;
        int c = Int32.Parse(line[2]);

        if (a == source) flows[a, b] = c;
        else if (a == sink) flows[b, a] = c;
        else { flows[a, b] = c;  flows[b, a] = c; }

        Console.WriteLine(FordFulkerson());
      }
    }
  }
}
