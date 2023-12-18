using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day17 {
		public static string Part1() {
			var cellWeights = ParseCellWeights(Utils.ReadAllLines(17));
			var gridSize = (x: cellWeights.Keys.Max(t => t.x) + 1, y: cellWeights.Keys.Max(t => t.y) + 1);
			var destination = (gridSize.x - 1, gridSize.y - 1);
			var bestPath = FindPath((0, 0), destination, gridSize, cellWeights, false);
			return $"{bestPath.CurrentPathWeight}";
		}

		public static string Part2() {
			var cellWeights = ParseCellWeights(Utils.ReadAllLines(17));
			var gridSize = (x: cellWeights.Keys.Max(t => t.x) + 1, y: cellWeights.Keys.Max(t => t.y) + 1);
			var destination = (gridSize.x - 1, gridSize.y - 1);
			var bestPath = FindPath((0, 0), destination, gridSize, cellWeights, true);
			return $"{bestPath.CurrentPathWeight}";
		}

		private static Dictionary<(int x, int y ), int> ParseCellWeights(IEnumerable<string> inputLines) =>
			inputLines.SelectMany((row, y) => row.Select((cell, x) => ((x, y), cell - '0'))).ToDictionary(t => t.Item1, t => t.Item2);

		private static Path FindPath((int, int) origin, (int, int) destination, (int, int) gridSize, IReadOnlyDictionary<(int, int), int> cellWeights, bool part2) {
			var visitedCells = new HashSet<((int, int), Direction, int)>();
			var initialPath = new Path(origin, gridSize);
			var openPathsOrderedByWeight = new List<Path> { initialPath };
			while (openPathsOrderedByWeight.Count > 0) {
				var propagatingPath = openPathsOrderedByWeight[0];
				openPathsOrderedByWeight.RemoveAt(0);
				foreach (var propagationDirection in propagatingPath.AllowedDirections) {
					var nextCell = propagatingPath.NeighbourCell(propagationDirection);
					var nextPath = new Path(propagatingPath, propagationDirection, gridSize, cellWeights[nextCell], part2);
					if (!propagatingPath.HasAlreadyVisitedCell(nextCell)) {
						if (nextPath.CurrentPosition == destination) return nextPath;
						var pathID = (nextPath.CurrentPosition, nextPath.LatestMoveDirection, nextPath.CountLatestMoveDirection);
						if (!visitedCells.Contains(pathID)) {
							var insertIndex = openPathsOrderedByWeight.Count;
							while (insertIndex > 0 && openPathsOrderedByWeight[insertIndex - 1].ExpectedFinalPathWeight > nextPath.ExpectedFinalPathWeight) insertIndex--;
							openPathsOrderedByWeight.Insert(insertIndex, nextPath);
							visitedCells.Add(pathID);
						}
					}
				}
			}
			return null;
		}

		private enum Direction {
			None,
			Up,
			Left,
			Down,
			Right
		}

		private class Path {
			private Path ParentParentPath { get; }
			public (int x, int y ) CurrentPosition { get; }
			public int CurrentPathWeight { get; }
			public int ExpectedFinalPathWeight { get; }
			public Direction LatestMoveDirection { get; }
			public int CountLatestMoveDirection { get; }
			public IEnumerable<Direction> AllowedDirections { get; }

			public Path((int x, int y) initialCoordinates, (int x, int y) gridSize) {
				ParentParentPath = null;
				CurrentPosition = initialCoordinates;
				CurrentPathWeight = 0;
				ExpectedFinalPathWeight = Math.Abs(gridSize.x - CurrentPosition.x) + Math.Abs(gridSize.y - CurrentPosition.y);
				LatestMoveDirection = Direction.None;
				CountLatestMoveDirection = 0;
				AllowedDirections = EvaluateAllowedDirections(gridSize);
			}

			public Path(Path parentPath, Direction directionTaken, (int x, int y) gridSize, int cellWeight, bool part2) {
				ParentParentPath = parentPath;
				CurrentPosition = parentPath.NeighbourCell(directionTaken);
				CurrentPathWeight = parentPath.CurrentPathWeight + cellWeight;
				ExpectedFinalPathWeight = CurrentPathWeight + Math.Abs(gridSize.x - CurrentPosition.x) + Math.Abs(gridSize.y - CurrentPosition.y);
				LatestMoveDirection = directionTaken;
				if (directionTaken == parentPath.LatestMoveDirection) CountLatestMoveDirection = parentPath.CountLatestMoveDirection + 1;
				else {
					CountLatestMoveDirection = 1;
				}
				AllowedDirections = part2 ? EvaluateAllowedDirectionsPart2(gridSize) : EvaluateAllowedDirections(gridSize);
			}

			public bool HasAlreadyVisitedCell((int, int) cell) => CurrentPosition == cell || (ParentParentPath?.HasAlreadyVisitedCell(cell) ?? false);

			private IEnumerable<Direction> EvaluateAllowedDirections((int x, int y) gridSize) {
				var allowedDirections = new HashSet<Direction>();
				if (CurrentPosition.x > 0 && (LatestMoveDirection != Direction.Left || CountLatestMoveDirection < 3) && LatestMoveDirection != Direction.Right)
					allowedDirections.Add(Direction.Left);
				if (CurrentPosition.x < gridSize.x - 1 && (LatestMoveDirection != Direction.Right || CountLatestMoveDirection < 3) && LatestMoveDirection != Direction.Left)
					allowedDirections.Add(Direction.Right);
				if (CurrentPosition.y > 0 && (LatestMoveDirection != Direction.Up || CountLatestMoveDirection < 3) && LatestMoveDirection != Direction.Down)
					allowedDirections.Add(Direction.Up);
				if (CurrentPosition.y < gridSize.y - 1 && (LatestMoveDirection != Direction.Down || CountLatestMoveDirection < 3) && LatestMoveDirection != Direction.Up)
					allowedDirections.Add(Direction.Down);
				return allowedDirections;
			}

			private bool CanGoPart2(Direction direction, bool positionAllowsIt) {
				if (!positionAllowsIt) return false;
				if (LatestMoveDirection == direction && CountLatestMoveDirection < 4) return true;
				if (LatestMoveDirection != direction && CountLatestMoveDirection < 4) return false;
				if (LatestMoveDirection == direction && CountLatestMoveDirection >= 10) return false;
				return true;
			}

			private IEnumerable<Direction> EvaluateAllowedDirectionsPart2((int x, int y) gridSize) {
				var allowedDirections = new HashSet<Direction>();
				if (CanGoPart2(Direction.Left, CurrentPosition.x > 0 )) allowedDirections.Add(Direction.Left);
				if (CanGoPart2(Direction.Right, CurrentPosition.x < gridSize.x - 1)) allowedDirections.Add(Direction.Right);
				if (CanGoPart2(Direction.Up, CurrentPosition.y > 0)) allowedDirections.Add(Direction.Up);
				if (CanGoPart2(Direction.Down, CurrentPosition.y < gridSize.y - 1)) allowedDirections.Add(Direction.Down);
				return allowedDirections;
			}

			public (int, int) NeighbourCell(Direction direction) {
				switch (direction) {
					case Direction.Up: return (CurrentPosition.x, CurrentPosition.y - 1);
					case Direction.Left: return (CurrentPosition.x - 1, CurrentPosition.y);
					case Direction.Down: return (CurrentPosition.x, CurrentPosition.y + 1);
					case Direction.Right: return (CurrentPosition.x + 1, CurrentPosition.y);
					case Direction.None: return CurrentPosition;
					default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
				}
			}
		}
	}
}