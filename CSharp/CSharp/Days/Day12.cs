using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day12 {
		public static string Part1() {
			return $"{ParseInput(Utils.ReadAllLines(12)).Sum(t => CountPossibilities(t.sprintStates, 0, t.contiguousSizes, 0))}";
		}

		public static string Part2() {
			return "";
		}

		private static IEnumerable<(string sprintStates, int[] contiguousSizes)> ParseInput(IEnumerable<string> inputLines) =>
			inputLines.Where(t => !string.IsNullOrEmpty(t)).Select(t => (t.Split(' ')[0], t.Split(' ')[1].Split(',').Select(int.Parse).ToArray())).ToArray();

		private static int CountPossibilities(string springStates, int springStateStartIndex, int[] contiguousSizes, int contiguousSizeStartIndex) {
			if (contiguousSizeStartIndex >= contiguousSizes.Length) return springStates.LastIndexOf('#') >= springStateStartIndex ? 0 : 1;
			var currentChunkSize = contiguousSizes[contiguousSizeStartIndex];
			if (springStateStartIndex >= springStates.Length) return 0;
			if (springStates.Length - springStateStartIndex < currentChunkSize) return 0;
			if (springStates[springStateStartIndex] == '.') return CountPossibilities(springStates, springStateStartIndex + 1, contiguousSizes, contiguousSizeStartIndex);
			if (springStates[springStateStartIndex] == '#') {
				return CountPossibilitiesConsideringFirstAsDamaged(springStates, springStateStartIndex, contiguousSizes, contiguousSizeStartIndex);
			}
			if (springStates[springStateStartIndex] == '?') {
				return CountPossibilities(springStates, springStateStartIndex + 1, contiguousSizes, contiguousSizeStartIndex)
						+ CountPossibilitiesConsideringFirstAsDamaged(springStates, springStateStartIndex, contiguousSizes, contiguousSizeStartIndex);
			}
			throw new ArgumentException($"{springStates[springStateStartIndex]} is not a known state");
		}

		private static int CountPossibilitiesConsideringFirstAsDamaged(string springStates, int springStateStartIndex, int[] contiguousSizes, int contiguousSizeStartIndex) {
			var currentChunkSize = contiguousSizes[contiguousSizeStartIndex];
			for (var i = 1; i < currentChunkSize; ++i) {
				if (springStates[springStateStartIndex + i] == '.') return 0;
			}
			if (springStates.Length - springStateStartIndex == currentChunkSize) return contiguousSizeStartIndex == contiguousSizes.Length - 1 ? 1 : 0;
			if (springStates[springStateStartIndex + currentChunkSize] == '#') return 0;
			return CountPossibilities(springStates, springStateStartIndex + currentChunkSize + 1, contiguousSizes, contiguousSizeStartIndex + 1);
		}
	}
}