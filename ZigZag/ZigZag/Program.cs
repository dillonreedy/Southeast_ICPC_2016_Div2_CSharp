﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag
{
  class Program
  {
    public int[] nums;
    public int n;
    public static Random r1 = new Random(7);
    public static Random r2 = new Random(1000000);


    static void Main(string[] args)
    {
      int n = r1.Next(2, 10);
      int[] nums = getRandomNumbers(n);
      new Program().doit(n, nums);
      //new Program().doit();
    }

    public static int[] getRandomNumbers(int n)
    {
      int[] nums = new int[n];
      for (int i = 0; i < n; i++)
        nums[i] = r2.Next(1,1000000);

      return nums;
    }

    public void doit()
    {
      n = Int32.Parse(Console.ReadLine());
      nums = new int[n];

      for (int i = 0; i < n; i++) nums[i] = Int32.Parse(Console.ReadLine());

      int[] lessThan = new int[n];
      int[] greaterThan = new int[n];

      for (int i = n; i >= 0; i--)
      {

        for (int j = i + 1; j < n; j++)
        {
          if (nums[i] < nums[j]) lessThan[i] = Math.Max(lessThan[i], greaterThan[j] + 1);
          else if (nums[i] > nums[j]) greaterThan[i] = Math.Max(greaterThan[i], lessThan[j] + 1);
        }
      }

      int maxVal = 0;
      for (int i = 0; i < n; i++)
      {
        maxVal = Math.Max(maxVal, lessThan[i]);
        maxVal = Math.Max(maxVal, greaterThan[i]);
      }

      Console.WriteLine(maxVal + 1);
      Console.ReadKey();

    }

    public void doit(int n, int[] nums)
    {
      int[] lessThan = new int[n];
      int[] greaterThan = new int[n];

      for (int i = n; i >= 0; i--)
      {

        for (int j = i + 1; j < n; j++)
        {
          if (nums[i] < nums[j]) lessThan[i] = Math.Max(lessThan[i], greaterThan[j]+1);
          else if (nums[i] > nums[j]) greaterThan[i] = Math.Max(greaterThan[i], lessThan[j]+1);
        }
      }

      int maxVal = 0;
      for (int i = 0; i < n; i++)
      {
        maxVal = Math.Max(maxVal, lessThan[i]);
        maxVal = Math.Max(maxVal, greaterThan[i]);
      }

      displayArray(nums);
      displayArray(greaterThan);
      displayArray(lessThan);
      Console.WriteLine("The answer is: " + (maxVal + 1));
      Console.ReadKey();

    }

    public void displayArray(int[] nums)
    {
      Console.Write("The array: [");

      for (int i = 0; i < nums.Length - 1; i++)
        Console.Write(nums[i] + ", ");
      
      Console.WriteLine(nums[nums.Length - 1] + "]");
    }
  }
}
