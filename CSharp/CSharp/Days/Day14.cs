using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day14 {
		public static string Part1() {
			var grid = Grid.Parse(Utils.ReadAllLines(14).ToArray());
			grid.SlideNorth();
			return $"{grid.TotalLoad}";
		}

		public static string Part2() {
			var grid = Grid.Parse(Utils.ReadAllLines(14).ToArray());
			return $"{EvaluatePostCycleTotalLoad(grid, 1000000000L)}";
		}

		private static long EvaluatePostCycleTotalLoad(Grid grid, long cyclesToSimulate) {
			var dispositionScores = new List<long>();
			var totalLoads = new List<long>();
			while (true) {
				grid.DoCycle();
				var currentDispositionScore = grid.DispositionScore;
				if (dispositionScores.Contains(currentDispositionScore)) {
					var preCycleLength = dispositionScores.IndexOf(currentDispositionScore);
					var cycleLength = dispositionScores.Count - preCycleLength;
					return totalLoads[(int)(preCycleLength + (cyclesToSimulate - preCycleLength - 1) % cycleLength)];
				}
				dispositionScores.Add(currentDispositionScore);
				totalLoads.Add(grid.TotalLoad);
			}
		}

		private class Grid {
			private Dictionary<(int x, int y), char> Content { get; }
			private int Width { get; }
			private int Height { get; }
			public long TotalLoad => Content.Where(t => t.Value == 'O').Sum(t => Height - t.Key.y);
			public long DispositionScore => Content.Where(t => t.Value == 'O').Sum(t => Width * (Height - t.Key.y) + (Width - t.Key.x));

			private Grid(Dictionary<(int x, int y), char> gridContent) {
				Content = gridContent;
				Width = gridContent.Keys.Max(t => t.x) + 1;
				Height = gridContent.Keys.Max(t => t.y) + 1;
			}

			public static Grid Parse(IEnumerable<string> inputLines) =>
				new Grid(inputLines.SelectMany((row, y) => row.Select((c, x) => (coord: (x, y), c)).Where(t => t.c != '.')).ToDictionary(t => t.coord, t => t.c));

			public void DoCycle() {
				SlideNorth();
				SlideWest();
				SlideSouth();
				SlideEast();
			}

			public void SlideNorth() => Slide(coord => coord.x, coord => coord.y, Height, (f, m) => (f, m), -1);
			private void SlideSouth() => Slide(coord => coord.x, coord => coord.y, Height, (f, m) => (f, m), 1);
			private void SlideWest() => Slide(coord => coord.y, coord => coord.x, Width, (f, m) => (m, f), -1);
			private void SlideEast() => Slide(coord => coord.y, coord => coord.x, Width, (f, m) => (m, f), 1);

			private void Slide(Func<(int x, int y), int> toFixedValue, Func<(int x, int y), int> toMovingValue, int movingAxisSize, Func<int, int, (int, int)> toCoord, int delta) {
				foreach (var itemInGrid in Content.Where(t => t.Value == 'O').ToArray()) {
					var f = toFixedValue(itemInGrid.Key);
					var emptyM = -1;
					for (var m = toMovingValue(itemInGrid.Key) + delta; m >= 0 && m < movingAxisSize && GetChar(toCoord(f, m)) != '#'; m += delta) {
						if (GetChar(toCoord(f, m)) == '.') emptyM = m;
					}
					if (emptyM >= 0) {
						Content.Add(toCoord(f, emptyM), 'O');
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