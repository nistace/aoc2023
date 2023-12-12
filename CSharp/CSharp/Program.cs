using System;
using AOC23.Days;

namespace AOC23 {
	public static class Program {
		private static void Main() {
			var watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			Console.WriteLine($"Part 1: {Day12.Part1()}");
			watch.Stop();
			Console.WriteLine($"[Execution time: {watch.ElapsedMilliseconds / 1000f:0.000}s]");

			watch.Restart();
			Console.WriteLine($"Part 2: {Day12.Part2()}");
			watch.Stop();
			Console.WriteLine($"[Execution time: {watch.ElapsedMilliseconds / 1000f:0.000}s]");
		}
	}
}