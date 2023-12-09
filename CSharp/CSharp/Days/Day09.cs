using System;
using System.Linq;

namespace AOC23.Days {
	public static class Day09 {
		public static string Part1() {
			var series = Utils.ReadAllLines(9).Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Split(' ').Select(long.Parse).ToArray());
			return $"{series.Select(EvaluateNextItem).Sum()}";
		}

		public static string Part2() {
			var series = Utils.ReadAllLines(9).Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Split(' ').Select(long.Parse).ToArray());
			return $"{series.Select(EvaluatePreviousItem).Sum()}";
		}

		private static long EvaluateNextItem(long[] numbers) {
			if (numbers.All(t => t == 0)) return 0;
			return numbers.Last() + EvaluateNextItem(numbers.Skip(1).Select((t, i) => t - numbers[i]).ToArray());
		}

		private static long EvaluatePreviousItem(long[] numbers) {
			if (numbers.All(t => t == 0)) return 0;
			return numbers[0] - EvaluatePreviousItem(numbers.Skip(1).Select((t, i) => t - numbers[i]).ToArray());
		}
	}
}