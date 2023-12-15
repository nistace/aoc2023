using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day13 {
		public static string Part1() {
			return $"{Grid.ParseAll(Utils.ReadAllLines(13)).Sum(t => t.GetScore())}";
		}

		public static string Part2() {
			return $"{Grid.ParseAll(Utils.ReadAllLines(13)).Sum(t => t.GetScoreWithSmudge())}";
		}

		private class Grid {
			private IReadOnlyList<string> Rows { get; }
			private IReadOnlyList<string> FlippedRows => Enumerable.Range(0, Rows[0].Length).Select(t => string.Join("", Rows.Select(r => r[t]))).ToArray();

			private Grid(IEnumerable<string> rows) {
				Rows = rows.ToArray();
			}

			public static IEnumerable<Grid> ParseAll(IEnumerable<string> inputLines) {
				var result = new List<Grid>();
				var singleMapRows = new List<string>();
				foreach (var inputLine in inputLines) {
					if (string.IsNullOrEmpty(inputLine)) {
						if (singleMapRows.Count > 0) {
							result.Add(new Grid(singleMapRows));
							singleMapRows.Clear();
						}
					}
					else {
						singleMapRows.Add(inputLine);
					}
				}
				if (singleMapRows.Count > 0) result.Add(new Grid(singleMapRows));
				return result;
			}

			public override string ToString() => string.Join(Environment.NewLine, Rows);

			public int GetScore() {
				if (TryFindMirrorAxisIndex(Rows, out var axisIndex)) return 100 * axisIndex;
				if (TryFindMirrorAxisIndex(FlippedRows, out axisIndex)) return axisIndex;
				return 0;
			}

			private static bool TryFindMirrorAxisIndex(IReadOnlyList<string> rows, out int axisIndex) {
				for (axisIndex = 1; axisIndex < rows.Count; ++axisIndex) {
					var mirrored = true;
					for (var aboveRowIndex = 0; mirrored && aboveRowIndex < axisIndex; ++aboveRowIndex) {
						var belowRowIndex = 2 * axisIndex - aboveRowIndex - 1;
						mirrored &= belowRowIndex >= rows.Count || rows[belowRowIndex] == rows[aboveRowIndex];
					}
					if (mirrored) return true;
				}
				return false;
			}

			public int GetScoreWithSmudge() {
				if (TryFindMirrorAxisIndexWithSmudge(Rows, out var axisIndex)) return 100 * axisIndex;
				if (TryFindMirrorAxisIndexWithSmudge(FlippedRows, out axisIndex)) return axisIndex;
				return 0;
			}

			private static bool TryFindMirrorAxisIndexWithSmudge(IReadOnlyList<string> rows, out int axisIndex) {
				for (axisIndex = 1; axisIndex < rows.Count; ++axisIndex) {
					var differentCharacters = 0;
					for (var aboveRowIndex = 0; differentCharacters < 2 && aboveRowIndex < axisIndex; ++aboveRowIndex) {
						var belowRowIndex = 2 * axisIndex - aboveRowIndex - 1;
						if (belowRowIndex < rows.Count) {
							for (var charIndex = 0; differentCharacters < 2 && charIndex < rows[belowRowIndex].Length; ++charIndex) {
								if (rows[belowRowIndex][charIndex] != rows[aboveRowIndex][charIndex]) differentCharacters++;
							}
						}
					}
					if (differentCharacters == 1) return true;
				}
				return false;
			}
		}
	}
}