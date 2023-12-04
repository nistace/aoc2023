﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC23.Days {
	public static class Day04 {
		private static readonly Regex inputLineRegex = new Regex("Card +\\d+: (?<winning>[\\d ]+)\\|(?<played>[\\d ]+)");

		public static string Part1() {
			var input = Utils.ReadAllLines(4).Where(t => !string.IsNullOrEmpty(t));
			var lineNumbers = input.Select(t => inputLineRegex.Match(t)).Select(t => (winning: GetListOfNumbers(t.Groups["winning"].Value), played: GetListOfNumbers(t.Groups["played"].Value)));
			var scores = lineNumbers.Select(t => t.played.Count(p => t.winning.Contains(p))).Select(t => t == 0 ? 0 : Math.Pow(2, t - 1));
			return $"{Math.Round(scores.Sum())}";
		}

		public static string Part2() {
			return $"";
		}

		private static IEnumerable<int> GetListOfNumbers(string stringOfNumbers) => stringOfNumbers.Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(int.Parse);
	}
}