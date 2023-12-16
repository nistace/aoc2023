using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day14 {
		public static string Part1() {
			var grid = Grid.Parse(Utils.ReadAllLines(14).ToArray());
			Console.WriteLine(grid);
			grid.SlideNorth();
			Console.WriteLine();
			Console.WriteLine(grid);
			return $"{grid.TotalLoad}";
		}

		public static string Part2() {
			return "";
		}

		private class Grid {
			private Dictionary<(int x, int y), char> Content { get; }
			private int Width { get; }
			private int Height { get; }
			public long TotalLoad => Content.Where(t => t.Value == 'O').Sum(t => Height - t.Key.y);

			private Grid(Dictionary<(int x, int y), char> gridContent) {
				Content = gridContent;
				Width = gridContent.Keys.Max(t => t.x) + 1;
				Height = gridContent.Keys.Max(t => t.y) + 1;
			}

			public static Grid Parse(IEnumerable<string> inputLines) =>
				new Grid(inputLines.SelectMany((row, y) => row.Select((c, x) => (coord: (x, y), c)).Where(t => t.c != '.')).ToDictionary(t => t.coord, t => t.c));

			public void SlideNorth() {
				foreach (var itemInGrid in Content.Where(t => t.Value == 'O').ToArray()) {
					var x = itemInGrid.Key.x;
					var emptyY = -1;
					for (var y = itemInGrid.Key.y - 1; y >= 0 && GetChar((x, y)) != '#'; --y) {
						if (GetChar((x, y)) == '.') emptyY = y;
					}
					if (emptyY >= 0) {
						Content.Add((x, emptyY), 'O');
						Content.Remove(itemInGrid.Key);
					}
				}
			}

			private char GetChar((int x, int y) coord) => Content.ContainsKey(coord) ? Content[coord] : '.';

			public override string ToString() =>
				string.Join(Environment.NewLine, Enumerable.Range(0, Height).Select(y => string.Join(string.Empty, Enumerable.Range(0, Width).Select(x => GetChar((x, y))))));
		}
	}
}