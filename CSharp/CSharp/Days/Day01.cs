using System.Linq;

namespace AOC23.Days {
	public static class Day01 {
		public static string Part1() {
			var sum = Utils.ReadAllLines(1).Where(t => t.Any(IsDigit)).Select(t => 10 * (t.First(IsDigit) - '0') + (t.Last(IsDigit) - '0')).Sum();
			return $"{sum}";
		}

		public static string Part2() => "";

		private static bool IsDigit(char c) => c >= '0' && c <= '9';
	}
}