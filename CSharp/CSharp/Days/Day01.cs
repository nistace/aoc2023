using System;
using System.Linq;

namespace AOC23.Days {
	public static class Day01 {
		private static readonly string[] digitsAsStrings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

		public static string Part1() {
			var sum = Utils.ReadAllLines(1).Where(t => t.Any(IsDigit)).Select(t => 10 * (t.First(IsDigit) - '0') + (t.Last(IsDigit) - '0')).Sum();
			return $"{sum}";
		}

		public static string Part2() {
			var sum = Utils.ReadAllLines(1).Select(t => FindFirstDigit(t) * 10 + FindLastDigit(t)).Sum();
			return $"{sum}";
		}

		private static bool IsDigit(char c) => c >= '0' && c <= '9';

		private static int FindFirstDigit(string line) {
			for (var i = 0; i < line.Length; ++i) {
				if (line[i] >= '0' && line[i] <= '9') return line[i] - '0';
				var restOfLine = line.Substring(i);
				var digitAsString = digitsAsStrings.FirstOrDefault(t => restOfLine.StartsWith(t));
				if (!string.IsNullOrEmpty(digitAsString)) return Array.IndexOf(digitsAsStrings, digitAsString);
			}
			return 0;
		}

		private static int FindLastDigit(string line) {
			for (var i = line.Length - 1; i >= 0; --i) {
				if (line[i] >= '0' && line[i] <= '9') return line[i] - '0';
				var restOfLine = line.Substring(i);
				var digitAsString = digitsAsStrings.FirstOrDefault(t => restOfLine.StartsWith(t));
				if (!string.IsNullOrEmpty(digitAsString)) return Array.IndexOf(digitsAsStrings, digitAsString);
			}
			return 0;
		}
	}
}