using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day11 {
		public static string Part1() {
			var galaxies = ParseInput(Utils.ReadAllLines(11));
			Expand(galaxies);

			var lengthSum = 0;
			for (var i = 0; i < galaxies.Count; i++) {
				for (var j = i + 1; j < galaxies.Count; j++) {
					lengthSum += Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
				}
			}

			return $"{lengthSum}";
		}

		public static string Part2() {
			return $"";
		}

		private static void Display(IEnumerable<(int x, int y)> galaxies) {
			var asArray = galaxies as (int x, int y)[] ?? galaxies.ToArray();
			var width = asArray.Max(t => t.x);
			var height = asArray.Max(t => t.y);

			for (var y = 0; y <= height; ++y) {
				for (var x = 0; x <= width; ++x) {
					Console.Write(asArray.Contains((x, y)) ? '#' : '.');
				}
				Console.WriteLine();
			}
		}

		private static List<(int x, int y)> ParseInput(IEnumerable<string> inputLines) =>
			inputLines.SelectMany((row, y) => row.Select((c, x) => (c, x)).Where(t => t.c == '#').Select(t => (t.x, y))).ToList();

		private static void Expand(this List<(int x, int y)> grid) {
			var width = grid.Max(t => t.x);
			for (var x = 0; x < width; ++x) {
				if (grid.All(t => t.x != x)) {
					width++;
					x++;
					var movedItems = grid.Where(t => t.x >= x).ToDictionary(t => t, t => (t.x + 1, t.y));
					grid.RemoveAll(t => movedItems.ContainsKey(t));
					grid.AddRange(movedItems.Values);
				}
			}

			var height = grid.Max(t => t.y);
			for (var y = 0; y < height; ++y) {
				if (grid.All(t => t.y != y)) {
					height++;
					y++;
					var movedItems = grid.Where(t => t.y >= y).ToDictionary(t => t, t => (t.x, t.y + 1));
					grid.RemoveAll(t => movedItems.ContainsKey(t));
					grid.AddRange(movedItems.Values);
				}
			}
		}
	}
}