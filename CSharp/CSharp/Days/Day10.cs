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
			return "";
		}

		private static void ParseInput(out Dictionary<(int x, int y), PipeChunk> pipeChunks, out PipeChunk startingPoint) {
			pipeChunks = Utils.ReadAllLines(10).SelectMany((row, y) => row.Select((pipeChar, x) => (coordinates: (x, y), pipeChunk: new PipeChunk(pipeChar, (x, y)))))
				.ToDictionary(t => t.coordinates, t => t.pipeChunk);
			var startingPipeChunkCoordinates = pipeChunks.First(t => t.Value.PipeChar == 'S').Key;
			var startingPipeChunkNeighboursCoordinates = pipeChunks.Where(t => t.Value.HasExitTowards(startingPipeChunkCoordinates)).Select(t => t.Key).ToArray();
			var startingPointPipeChar = FindPipeChunkCharacter(startingPipeChunkCoordinates, startingPipeChunkNeighboursCoordinates[0], startingPipeChunkNeighboursCoordinates[1]);
			pipeChunks[startingPipeChunkCoordinates] = new PipeChunk(startingPointPipeChar, startingPipeChunkCoordinates, 'S');
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

			public PipeChunk(char directionPipeChar, (int x, int y) coordinates, char? overridePipeChar = null) {
				PipeChar = overridePipeChar ?? directionPipeChar;
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