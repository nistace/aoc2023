using System;
using System.Collections.Generic;
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
			var inputLines = Utils.ReadAllLines(8).ToArray();
			var directions = inputLines[0].Where(t => t == 'L' || t == 'R').Select(t => (t, t == 'L' ? (Func<(string, string), string>)Left : Right)).ToArray();
			var nodes = inputLines.Skip(1).Where(t => !string.IsNullOrEmpty(t)).Select(ParseNodeInputLine).ToDictionary(t => t.from, t => t.dests);

			var cycleLengthBetweenEndingNodes = nodes.Keys.Where(t => t.EndsWith('Z')).ToDictionary(t => t, t => 0L);
			foreach (var endingNode in nodes.Keys.Where(t => t.EndsWith('Z'))) {
				var nextNode = endingNode;
				do {
					nextNode = directions[cycleLengthBetweenEndingNodes[endingNode]++ % directions.Length].Item2(nodes[nextNode]);
				} while (!nextNode.EndsWith('Z'));
			}

			return $"{LCM(cycleLengthBetweenEndingNodes.Values)}";
		}

		private static long LCM(IEnumerable<long> numbers) => numbers.Aggregate(LCM);
		private static long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);
		private static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

		private static (string from, (string left, string right) dests) ParseNodeInputLine(string inputLine) =>
			(inputLine.Substring(0, 3), (inputLine.Substring(inputLine.IndexOf('(') + 1, 3), inputLine.Substring(inputLine.IndexOf(',') + 2, 3)));

		private static T Left<T>((T, T) neighbours) => neighbours.Item1;
		private static T Right<T>((T, T) neighbours) => neighbours.Item2;
	}
}