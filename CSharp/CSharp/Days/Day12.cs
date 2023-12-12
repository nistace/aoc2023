using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOC23.Days {
	public static class Day12 {
		private static Dictionary<string, BigInteger> scorePerSubInput { get; } = new Dictionary<string, BigInteger>();

		public static string Part1() {
			var inputLines = ParseInput(Utils.ReadAllLines(12));
			var sum = inputLines.Select(t => CountPossibilities(t.springStates, t.contiguousSizes)).Aggregate(new BigInteger(0), (current, count) => current + count);
			return $"{sum}";
		}

		public static string Part2() {
			var inputLines = ParseInput(Utils.ReadAllLines(12));
			var unfoldedInputLines = inputLines.Select(t => UnfoldInputLine(t, 5));
			var sum = unfoldedInputLines.Select(t => CountPossibilities(t.springStates, t.contiguousSizes)).Aggregate(new BigInteger(0), (current, count) => current + count);
			return $"{sum}";
		}

		private static (string springStates, int[] contiguousSizes) UnfoldInputLine((string springStates, int[] contiguousSizes) inputLine, int unfoldingLength) {
			return (string.Join("?", Enumerable.Repeat(inputLine.springStates, unfoldingLength)),
				Enumerable.Repeat(inputLine.contiguousSizes, unfoldingLength).Aggregate(Array.Empty<int>(), (cur, next) => cur.Concat(next).ToArray()));
		}

		private static IEnumerable<(string springStates, int[] contiguousSizes)> ParseInput(IEnumerable<string> inputLines) =>
			inputLines.Where(t => !string.IsNullOrEmpty(t)).Select(t => (t.Split(' ')[0], t.Split(' ')[1].Split(',').Select(int.Parse).ToArray())).ToArray();

		private static BigInteger CountPossibilities(string springStates, IReadOnlyList<int> contiguousSizes) =>
			CountPossibilities(springStates, 0, contiguousSizes, 0, contiguousSizes.Sum(t => t + 1) - 1);

		private static BigInteger CountPossibilities(string springStates, int springStateStartIndex, IReadOnlyList<int> contiguousSizes, int contiguousSizeStartIndex, int minSpringStateLength) {
			var subInput = springStates.Substring(springStateStartIndex) + " " + string.Join(",", contiguousSizes.Skip(contiguousSizeStartIndex));
			if (scorePerSubInput.ContainsKey(subInput)) return scorePerSubInput[subInput];
			var result = new BigInteger(0);
			if (springStates.Length - springStateStartIndex < minSpringStateLength) result = 0;
			else if (contiguousSizeStartIndex >= contiguousSizes.Count) result = springStates.LastIndexOf('#') >= springStateStartIndex ? 0 : 1;
			else {
				var currentChunkSize = contiguousSizes[contiguousSizeStartIndex];
				if (springStateStartIndex >= springStates.Length) result = 0;
				else if (springStates.Length - springStateStartIndex < currentChunkSize) result = 0;
				else if (springStates[springStateStartIndex] == '.') result = CountPossibilities(springStates, springStateStartIndex + 1, contiguousSizes, contiguousSizeStartIndex, minSpringStateLength);
				else if (springStates[springStateStartIndex] == '#') {
					result = CountPossibilitiesConsideringFirstAsDamaged(springStates, springStateStartIndex, contiguousSizes, contiguousSizeStartIndex, minSpringStateLength);
				}
				else if (springStates[springStateStartIndex] == '?') {
					result = CountPossibilities(springStates, springStateStartIndex + 1, contiguousSizes, contiguousSizeStartIndex, minSpringStateLength)
								+ CountPossibilitiesConsideringFirstAsDamaged(springStates, springStateStartIndex, contiguousSizes, contiguousSizeStartIndex, minSpringStateLength);
				}
			}
			scorePerSubInput[subInput] = result;
			return result;
		}

		private static BigInteger CountPossibilitiesConsideringFirstAsDamaged(string springStates, int springStateStartIndex, IReadOnlyList<int> contiguousSizes, int contiguousSizeStartIndex,
			int minSpringStateLength) {
			var currentChunkSize = contiguousSizes[contiguousSizeStartIndex];
			for (var i = 1; i < currentChunkSize; ++i) {
				if (springStates[springStateStartIndex + i] == '.') return 0;
			}
			if (springStates.Length - springStateStartIndex == currentChunkSize) return contiguousSizeStartIndex == contiguousSizes.Count - 1 ? 1 : 0;
			if (springStates[springStateStartIndex + currentChunkSize] == '#') return 0;
			return CountPossibilities(springStates, springStateStartIndex + currentChunkSize + 1, contiguousSizes, contiguousSizeStartIndex + 1, minSpringStateLength - currentChunkSize - 1);
		}
	}
}