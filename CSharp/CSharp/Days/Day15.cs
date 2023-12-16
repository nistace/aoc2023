using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day15 {
		public static string Part1() => $"{Utils.ReadAllLines(15).SelectMany(t => t.Split(",")).Sum(t => t.Aggregate(0L, (curr, next) => 17 * (curr + next) % 256))}";
		public static string Part2() => "";
	}
}