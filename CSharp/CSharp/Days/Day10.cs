using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day10 {
		public static string Part1() {
			ParseInput(out var pipeChunks, out var startingChunk);
			var loopLength = 0;
			var pipeChunk = startingChunk;
			var nextEntryDirection = startingChunk.AnyEntryDirection;
			do {
				loopLength++;
				nextEntryDirection = pipeChunk.GetExitDirection(nextEntryDirection);
				pipeChunk = pipeChunks[MoveCoordinates(pipeChunk.Coordinates, nextEntryDirection)];
			} while (pipeChunk != startingChunk);

			return $"{Math.Floor(loopLength * .5f)}";
		}

		public static string Part2() {
			ParseInput(out var pipeChunks, out var startingChunk);
			var loopChunks = RemoveUnnecessaryPipeChunks(pipeChunks, startingChunk);
			var widenedGrid = WidenPipeChunkGrid(loopChunks);
			FillInGridWithIs(widenedGrid);
			PropagateAndRemoveOs(widenedGrid, (0, 0));
			var unwidenedGrid = UnWidenGrid(widenedGrid);
			return $"{unwidenedGrid.Values.Count(t => t == 'I')}";
		}

		#region Part 2

		private static (int x, int y) WidenCoordinates((int x, int y) coordinates) => (coordinates.x * 2 + 1, coordinates.y * 2 + 1);
		private static (int x, int y) UnWidenCoordinates((int x, int y) coordinates) => (coordinates.x / 2, coordinates.y / 2);

		private static Dictionary<(int x, int y), char> WidenPipeChunkGrid(Dictionary<(int x, int y), PipeChunk> pipeChunks) {
			var widened = pipeChunks.ToDictionary(t => WidenCoordinates(t.Key), t => t.Value.PipeChar);
			var width = widened.Keys.Max(t => t.x) + 1;
			var height = widened.Keys.Max(t => t.y) + 1;
			var leftsLeadingToHorizontalPipe = new[] { 'L', '-', 'F' };
			var upsLeadingToVerticalPip = new[] { 'F', '|', '7' };
			for (var y = 0; y < height; y++) {
				for (var x = 0; x < width; ++x) {
					if (x % 2 == 0) {
						if (widened.TryGetValue((x - 1, y), out var left) && leftsLeadingToHorizontalPipe.Contains(left)) widened[(x, y)] = '-';
					}
					if (y % 2 == 0) {
						if (widened.TryGetValue((x, y - 1), out var up) && upsLeadingToVerticalPip.Contains(up)) widened[(x, y)] = '|';
					}
				}
			}
			return widened;
		}

		private static Dictionary<(int x, int y), PipeChunk> RemoveUnnecessaryPipeChunks(Dictionary<(int x, int y), PipeChunk> pipeChunks, PipeChunk startingChunk) {
			var usefulChunks = new HashSet<PipeChunk> { startingChunk };
			var pipeChunk = startingChunk;
			var nextEntryDirection = startingChunk.AnyEntryDirection;
			do {
				nextEntryDirection = pipeChunk.GetExitDirection(nextEntryDirection);
				pipeChunk = pipeChunks[MoveCoordinates(pipeChunk.Coordinates, nextEntryDirection)];
				usefulChunks.Add(pipeChunk);
			} while (pipeChunk != startingChunk);
			return pipeChunks.Where(t => usefulChunks.Contains(t.Value)).ToDictionary(t => t.Key, t => t.Value);
		}

		private static void FillInGridWithIs(Dictionary<(int x, int y), char> grid) {
			var width = grid.Keys.Max(t => t.x) + 2;
			var height = grid.Keys.Max(t => t.y) + 2;
			for (var y = 0; y < height; y++) {
				for (var x = 0; x < width; ++x) {
					if (!grid.ContainsKey((x, y))) grid[(x, y)] = 'I';
				}
			}
		}

		private static void PropagateAndRemoveOs(Dictionary<(int x, int y), char> grid, (int x, int y) firstOCoordinates) {
			grid.Remove(firstOCoordinates);
			var onesToPropagate = new HashSet<(int x, int y)> { firstOCoordinates };
			while (onesToPropagate.Count > 0) {
				var oCoord = onesToPropagate.First();
				grid.Remove(oCoord);
				onesToPropagate.Remove(oCoord);
				foreach (var neighbourCoordinates in new[] { (oCoord.x - 1, oCoord.y), (oCoord.x + 1, oCoord.y), (oCoord.x, oCoord.y - 1), (oCoord.x, oCoord.y + 1) }) {
					if (!onesToPropagate.Contains(neighbourCoordinates) && grid.TryGetValue(neighbourCoordinates, out var neighbour) && neighbour == 'I')
						onesToPropagate.Add(neighbourCoordinates);
				}
			}
		}

		private static Dictionary<(int x, int y), char> UnWidenGrid(Dictionary<(int x, int y), char> widenedGrid) =>
			widenedGrid.Where(t => t.Key.x % 2 == 1 && t.Key.y % 2 == 1).ToDictionary(t => UnWidenCoordinates(t.Key), t => t.Value);

		#endregion

		private static void ParseInput(out Dictionary<(int x, int y), PipeChunk> pipeChunks, out PipeChunk startingPoint) {
			pipeChunks = Utils.ReadAllLines(10).SelectMany((row, y) => row.Select((pipeChar, x) => (coordinates: (x, y), pipeChunk: new PipeChunk(pipeChar, (x, y)))))
				.ToDictionary(t => t.coordinates, t => t.pipeChunk);
			var startingPipeChunkCoordinates = pipeChunks.First(t => t.Value.PipeChar == 'S').Key;
			var startingPipeChunkNeighboursCoordinates = pipeChunks.Where(t => t.Value.HasExitTowards(startingPipeChunkCoordinates)).Select(t => t.Key).ToArray();
			var startingPointPipeChar = FindPipeChunkCharacter(startingPipeChunkCoordinates, startingPipeChunkNeighboursCoordinates[0], startingPipeChunkNeighboursCoordinates[1]);
			pipeChunks[startingPipeChunkCoordinates] = new PipeChunk(startingPointPipeChar, startingPipeChunkCoordinates);
			startingPoint = pipeChunks[startingPipeChunkCoordinates];
		}

		private static char FindPipeChunkCharacter((int x, int y) pipeChunk, (int x, int y) firstNeighbourCoordinates, (int x, int y) otherNeighbourCoordinates) {
			var allDirections = ((Direction[])Enum.GetValues(typeof(Direction)));
			var directions = allDirections.Where(t => firstNeighbourCoordinates == MoveCoordinates(pipeChunk, t) || otherNeighbourCoordinates == MoveCoordinates(pipeChunk, t)).ToArray();

			if (directions.Contains(Direction.Up) && directions.Contains(Direction.Down)) return '|';
			if (directions.Contains(Direction.Up) && directions.Contains(Direction.Left)) return 'J';
			if (directions.Contains(Direction.Up) && directions.Contains(Direction.Right)) return 'L';
			if (directions.Contains(Direction.Down) && directions.Contains(Direction.Left)) return '7';
			if (directions.Contains(Direction.Down) && directions.Contains(Direction.Right)) return 'F';
			if (directions.Contains(Direction.Left) && directions.Contains(Direction.Right)) return '-';
			return '.';
		}

		private static (int x, int y) MoveCoordinates((int x, int y) coordinates, Direction direction) {
			switch (direction) {
				case Direction.Left: return (coordinates.x - 1, coordinates.y);
				case Direction.Right: return (coordinates.x + 1, coordinates.y);
				case Direction.Up: return (coordinates.x, coordinates.y - 1);
				case Direction.Down: return (coordinates.x, coordinates.y + 1);
				default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		private enum Direction {
			Left = 0,
			Right = 1,
			Up = 2,
			Down = 3
		}

		private class PipeChunk {
			public char PipeChar { get; }
			public (int x, int y) Coordinates { get; }
			private Dictionary<Direction, Direction> ExitCoordinatesPerEntryDirection { get; } = new Dictionary<Direction, Direction>();
			public Direction AnyEntryDirection => ExitCoordinatesPerEntryDirection.Keys.First();

			public PipeChunk(char directionPipeChar, (int x, int y) coordinates) {
				PipeChar = directionPipeChar;
				Coordinates = coordinates;
				switch (directionPipeChar) {
					case '|':
						ExitCoordinatesPerEntryDirection.Add(Direction.Up, Direction.Up);
						ExitCoordinatesPerEntryDirection.Add(Direction.Down, Direction.Down);
						break;
					case '-':
						ExitCoordinatesPerEntryDirection.Add(Direction.Right, Direction.Right);
						ExitCoordinatesPerEntryDirection.Add(Direction.Left, Direction.Left);
						break;
					case 'F':
						ExitCoordinatesPerEntryDirection.Add(Direction.Left, Direction.Down);
						ExitCoordinatesPerEntryDirection.Add(Direction.Up, Direction.Right);
						break;
					case '7':
						ExitCoordinatesPerEntryDirection.Add(Direction.Right, Direction.Down);
						ExitCoordinatesPerEntryDirection.Add(Direction.Up, Direction.Left);
						break;
					case 'J':
						ExitCoordinatesPerEntryDirection.Add(Direction.Right, Direction.Up);
						ExitCoordinatesPerEntryDirection.Add(Direction.Down, Direction.Left);
						break;
					case 'L':
						ExitCoordinatesPerEntryDirection.Add(Direction.Left, Direction.Up);
						ExitCoordinatesPerEntryDirection.Add(Direction.Down, Direction.Right);
						break;
				}
			}

			public Direction GetExitDirection(Direction entryDirection) => ExitCoordinatesPerEntryDirection[entryDirection];
			public bool HasExitTowards((int x, int y) coordinates) => ExitCoordinatesPerEntryDirection.Any(t => MoveCoordinates(Coordinates, t.Value) == coordinates);
		}
	}
}