using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC23.Days {
	public static class Day03 {
		public static string Part1() {
			var inputLines = Utils.ReadAllLines(3).ToArray();
			var specialCharacters = FindSpecialCharacters(inputLines);
			var numbers = FindNumbers(inputLines);
			var numbersToTake = numbers.Where(number => specialCharacters.Any(special => special.IsNeighbouring(number)));
			return $"{numbersToTake.Sum(t => t.Value)}";
		}

		public static string Part2() {
			var inputLines = Utils.ReadAllLines(3).ToArray();
			var potentialGears = FindSpecialCharacters(inputLines).Where(t => t.Character == '*');
			var numbers = FindNumbers(inputLines);
			var gearAdjacentNumbers = potentialGears.Select(gear => numbers.Where(gear.IsNeighbouring).ToArray()).Where(t => t.Length == 2);
			return $"{gearAdjacentNumbers.Sum(t => t.Aggregate(1, (cur, next) => cur * next.Value))}";
		}

		private static HashSet<SpecialCharacter> FindSpecialCharacters(IReadOnlyList<string> inputLines) {
			var result = new HashSet<SpecialCharacter>();
			var regex = new Regex("[^\\d\\.]");
			for (var y = 0; y < inputLines.Count; ++y) {
				var matches = regex.Matches(inputLines[y]);
				foreach (Match match in matches) {
					result.Add(new SpecialCharacter(match.Groups[0].Value[0], y, match.Index));
				}
			}
			return result;
		}

		private static HashSet<Number> FindNumbers(IReadOnlyList<string> inputLines) {
			var result = new HashSet<Number>();
			var regex = new Regex("\\d+");
			for (var y = 0; y < inputLines.Count; ++y) {
				var matches = regex.Matches(inputLines[y]);
				foreach (Match match in matches) {
					result.Add(new Number(int.Parse(match.Groups[0].Value), y, match.Index, match.Length));
				}
			}

			return result;
		}

		private readonly struct Number {
			public int Value { get; }
			public int Line { get; }
			public int StartRow { get; }
			private int Length { get; }
			public int EndRow => StartRow + Length - 1;

			public Number(int value, int line, int startRow, int length) {
				Value = value;
				Line = line;
				StartRow = startRow;
				Length = length;
			}
		}

		private readonly struct SpecialCharacter {
			public char Character { get; }
			private int Line { get; }
			private int Row { get; }

			public SpecialCharacter(char character, int line, int row) {
				Character = character;
				Line = line;
				Row = row;
			}

			public bool IsNeighbouring(Number number) {
				if (Line < number.Line - 1) return false;
				if (Line > number.Line + 1) return false;
				if (Row < number.StartRow - 1) return false;
				if (Row > number.EndRow + 1) return false;
				return true;
			}
		}
	}
}