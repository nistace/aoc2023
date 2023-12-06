using System.Linq;

namespace AOC23.Days {
	public static class Day06 {
		public static string Part1() {
			var input = Utils.ReadAllLines(6).ToArray();
			var times = input[0].Replace("Time:", "").Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(int.Parse).ToArray();
			var distances = input[1].Replace("Distance:", "").Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(int.Parse).ToArray();
			var numberOfWaysToBeat = times.Select((t, i) => Enumerable.Range(0, t + 1).Count(x => IsBetterScore(x, t, distances[i]))).ToArray();
			return $"{numberOfWaysToBeat.Aggregate(1, (cur, next) => cur * next)}";
		}

		public static string Part2() {
			var input = Utils.ReadAllLines(6).ToArray();
			var time = long.Parse(input[0].Replace("Time:", "").Replace(" ", ""));
			var distance = long.Parse(input[1].Replace("Distance:", "").Replace(" ", ""));

			var min = 0;
			var max = time;

			while (!IsBetterScore(min, time, distance)) min++;
			while (!IsBetterScore(max, time, distance)) max--;

			return $"{max - min + 1}";
		}

		private static bool IsBetterScore(long x, long time, long distance) => time * x - x * x > distance;
	}
}