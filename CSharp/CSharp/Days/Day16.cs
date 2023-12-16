using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day16 {
		public static string Part1() {
			var grid = ParseGrid(Utils.ReadAllLines(16));
			var visitedCells = new Dictionary<(int x, int y), HashSet<Direction>>();
			PropagateBeam((0, 0), Direction.Right, grid, ref visitedCells);
			return $"{visitedCells.Count}";
		}

		public static string Part2() {
			return "";
		}

		private static void PropagateBeam((int x, int y) coordinates, Direction direction, Grid grid, ref Dictionary<(int x, int y), HashSet<Direction>> visitedCells) {
			if (!grid.Inside(coordinates)) return;
			if (visitedCells.ContainsKey(coordinates)) {
				if (visitedCells[coordinates].Contains(direction)) return;
			}
			else visitedCells.Add(coordinates, new HashSet<Direction>());
			visitedCells[coordinates].Add(direction);
			foreach (var nextDirection in grid.Cells[coordinates].NextDirections[direction]) {
				PropagateBeam(Move(coordinates, nextDirection), nextDirection, grid, ref visitedCells);
			}
		}

		private static Grid ParseGrid(IEnumerable<string> inputLines) =>
			new Grid(inputLines.SelectMany((row, y) => row.Select((cell, x) => (coord: (x, y), cell: Cell.CellTypes[cell]))).ToDictionary(t => t.coord, t => t.cell));

		private static (int x, int y) Move((int x, int y) coords, Direction direction) {
			switch (direction) {
				case Direction.Up: return (coords.x, coords.y - 1);
				case Direction.Down: return (coords.x, coords.y + 1);
				case Direction.Right: return (coords.x + 1, coords.y);
				case Direction.Left: return (coords.x - 1, coords.y);
				default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		private enum Direction {
			Up = 0,
			Down = 1,
			Right = 2,
			Left = 3
		}

		private class Grid {
			public IReadOnlyDictionary<(int x, int y), Cell> Cells { get; }
			private int Width { get; }
			private int Height { get; }

			public Grid(IReadOnlyDictionary<(int x, int y), Cell> cells) {
				Cells = cells.ToDictionary(t => t.Key, t => t.Value);
				Width = cells.Keys.Max(t => t.x) + 1;
				Height = cells.Keys.Max(t => t.y) + 1;
			}

			public bool Inside((int x, int y) coords) => coords.x >= 0 && coords.x < Width && coords.y >= 0 && coords.y < Height;
		}

		private class Cell {
			private static Cell Empty { get; } = new Cell(new Dictionary<Direction, IReadOnlyList<Direction>> {
				{ Direction.Up, new List<Direction> { Direction.Up } },
				{ Direction.Down, new List<Direction> { Direction.Down } },
				{ Direction.Right, new List<Direction> { Direction.Right } },
				{ Direction.Left, new List<Direction> { Direction.Left } },
			});

			private static Cell RightMirror { get; } = new Cell(new Dictionary<Direction, IReadOnlyList<Direction>> {
				{ Direction.Up, new List<Direction> { Direction.Right } },
				{ Direction.Down, new List<Direction> { Direction.Left } },
				{ Direction.Right, new List<Direction> { Direction.Up } },
				{ Direction.Left, new List<Direction> { Direction.Down } },
			});

			private static Cell LeftMirror { get; } = new Cell(new Dictionary<Direction, IReadOnlyList<Direction>> {
				{ Direction.Up, new List<Direction> { Direction.Left } },
				{ Direction.Down, new List<Direction> { Direction.Right } },
				{ Direction.Right, new List<Direction> { Direction.Down } },
				{ Direction.Left, new List<Direction> { Direction.Up } },
			});

			private static Cell VerticalSplitter { get; } = new Cell(new Dictionary<Direction, IReadOnlyList<Direction>> {
				{ Direction.Up, new List<Direction> { Direction.Up } },
				{ Direction.Down, new List<Direction> { Direction.Down } },
				{ Direction.Right, new List<Direction> { Direction.Up, Direction.Down } },
				{ Direction.Left, new List<Direction> { Direction.Up, Direction.Down } },
			});

			private static Cell HorizontalSplitter { get; } = new Cell(new Dictionary<Direction, IReadOnlyList<Direction>> {
				{ Direction.Up, new List<Direction> { Direction.Left, Direction.Right } },
				{ Direction.Down, new List<Direction> { Direction.Left, Direction.Right } },
				{ Direction.Right, new List<Direction> { Direction.Right } },
				{ Direction.Left, new List<Direction> { Direction.Left } },
			});

			public static IReadOnlyDictionary<char, Cell> CellTypes { get; } = new Dictionary<char, Cell> {
				{ '.', Empty },
				{ '/', RightMirror },
				{ '\\', LeftMirror },
				{ '|', VerticalSplitter },
				{ '-', HorizontalSplitter }
			};

			public IReadOnlyDictionary<Direction, IReadOnlyList<Direction>> NextDirections { get; }

			private Cell(IReadOnlyDictionary<Direction, IReadOnlyList<Direction>> nextDirections) {
				NextDirections = nextDirections;
			}
		}
	}
}