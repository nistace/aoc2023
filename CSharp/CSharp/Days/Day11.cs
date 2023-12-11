using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOC23.Days {
	public static class Day11 {
		public static string Part1() {
			var galaxies = ParseInput(Utils.ReadAllLines(11));
			EvaluateRowAndColumnSizes(galaxies, 2, out var rowSizes, out var columnSizes);
			return $"{EvaluateSumOfDistances(galaxies, columnSizes, rowSizes)}";
		}

		public static string Part2() {
			var galaxies = ParseInput(Utils.ReadAllLines(11));
			EvaluateRowAndColumnSizes(galaxies, 1000000, out var rowSizes, out var columnSizes);
			return $"{EvaluateSumOfDistances(galaxies, columnSizes, rowSizes)}";
		}

		private static BigInteger EvaluateSumOfDistances(List<(int x, int y)> galaxies, IReadOnlyList<int> columnSizes, IReadOnlyList<int> rowSizes) {
			var lengthSum = new BigInteger(0);
			for (var i1 = 0; i1 < galaxies.Count; i1++) {
				for (var i2 = i1 + 1; i2 < galaxies.Count; i2++) {
					lengthSum += Enumerable.Range(Math.Min(galaxies[i1].x, galaxies[i2].x), Math.Abs(galaxies[i1].x - galaxies[i2].x)).Sum(t => columnSizes[t]);
					lengthSum += Enumerable.Range(Math.Min(galaxies[i1].y, galaxies[i2].y), Math.Abs(galaxies[i1].y - galaxies[i2].y)).Sum(t => rowSizes[t]);
				}
			}
			return lengthSum;
		}

		private static void EvaluateRowAndColumnSizes(List<(int x, int y)> galaxies, int expendSize, out IReadOnlyList<int> rowSizes, out IReadOnlyList<int> columnSizes) {
			columnSizes = Enumerable.Range(0, galaxies.Max(t => t.x) + 1).Select(t => galaxies.Any(u => u.x == t) ? 1 : expendSize).ToArray();
			rowSizes = Enumerable.Range(0, galaxies.Max(t => t.y) + 1).Select(t => galaxies.Any(u => u.y == t) ? 1 : expendSize).ToArray();
		}

		private static List<(int x, int y)> ParseInput(IEnumerable<string> inputLines) =>
			inputLines.SelectMany((row, y) => row.Select((c, x) => (c, x)).Where(t => t.c == '#').Select(t => (t.x, y))).ToList();
	}
}