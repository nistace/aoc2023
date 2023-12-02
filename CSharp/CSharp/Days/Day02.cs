using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC23.Days {
	public static class Day02 {
		public static string Part1() {
			var bagConfiguration = new Dictionary<CubeColor, int> { { CubeColor.Red, 12 }, { CubeColor.Green, 13 }, { CubeColor.Blue, 14 } };
			var games = Utils.ReadAllLines(2).Select(t => Game.TryParse(t, out var game) ? (true, game) : (false, null)).Where(t => t.Item1).Select(t => t.Item2);
			var validGames = games.Where(game => game.RevealedSets.All(set => bagConfiguration.All(cubeCountInConfiguration => set[cubeCountInConfiguration.Key] <= cubeCountInConfiguration.Value)));
			var idSum = validGames.Sum(t => t.Id);
			return $"{idSum}";
		}

		public static string Part2() {
			return $"";
		}

		public enum CubeColor {
			Blue,
			Red,
			Green
		}

		private class Game {
			private static Regex gamePattern { get; } = new Regex("^Game (\\d+):");

			public int Id { get; }
			public IReadOnlyList<RevealedSet> RevealedSets { get; }

			private Game(int id, IReadOnlyList<RevealedSet> revealedSets) {
				Id = id;
				RevealedSets = revealedSets;
			}

			public static bool TryParse(string inputLine, out Game game) {
				game = default;
				var regexMatch = gamePattern.Match(inputLine);
				if (!regexMatch.Success) return false;
				var id = int.Parse(regexMatch.Groups[1].Value);
				var revealedSets = inputLine.Substring(regexMatch.Groups[0].Length).Split(';').Select(RevealedSet.Parse).ToArray();
				game = new Game(id, revealedSets);
				return true;
			}

			public class RevealedSet {
				private static Regex revealedSetPattern { get; } = new Regex($"(\\d+) +({string.Join("|", (CubeColor[])Enum.GetValues(typeof(CubeColor)))})", RegexOptions.IgnoreCase);

				public IReadOnlyDictionary<CubeColor, int> RevealedCubeCounts { get; }

				public int this[CubeColor cubeColor] => RevealedCubeCounts.TryGetValue(cubeColor, out var value) ? value : 0;

				private RevealedSet(IReadOnlyDictionary<CubeColor, int> revealedCubeCounts) {
					RevealedCubeCounts = revealedCubeCounts;
				}

				public static RevealedSet Parse(string revealedSetLine) {
					var revealedCubeCounts = new Dictionary<CubeColor, int>();
					var patternMatches = revealedSetPattern.Matches(revealedSetLine);
					foreach (Match match in patternMatches) {
						revealedCubeCounts.Add(Enum.Parse<CubeColor>(match.Groups[2].Value, true), int.Parse(match.Groups[1].Value));
					}
					return new RevealedSet(revealedCubeCounts);
				}
			}
		}
	}
}