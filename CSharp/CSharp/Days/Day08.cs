using System;
using System.Linq;

namespace AOC23.Days {
	public static class Day08 {
		public static string Part1() {
			var inputLines = Utils.ReadAllLines(8).ToArray();
			var directions = inputLines[0].Where(t => t == 'L' || t == 'R').Select(t => (t, t == 'L' ? (Func<(string, string), string>)Left : Right)).ToArray();
			var nodes = inputLines.Skip(1).Where(t => !string.IsNullOrEmpty(t)).Select(ParseNodeInputLine).ToDictionary(t => t.from, t => t.dests);

			var stepCount = 0;
			for (var nodeName = "AAA"; nodeName != "ZZZ"; nodeName = directions[(stepCount - 1) % directions.Length].Item2(nodes[nodeName])) stepCount++;

			return $"{stepCount}";
		}

		public static string Part2() {
			return "";
		}

		private static (string from, (string left, string right) dests) ParseNodeInputLine(string inputLine) =>
			(inputLine.Substring(0, 3), (inputLine.Substring(inputLine.IndexOf('(') + 1, 3), inputLine.Substring(inputLine.IndexOf(',') + 2, 3)));

		private static T Left<T>((T, T) neighbours) => neighbours.Item1;
		private static T Right<T>((T, T) neighbours) => neighbours.Item2;
	}
}