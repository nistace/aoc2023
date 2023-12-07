using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day07 {
		public static string Part1() {
			var handBids = Utils.ReadAllLines(7).Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Split(' ')).Select(t => new HandBid(t[0], int.Parse(t[1])));
			var orderedHandBidsAndRanks = handBids.OrderBy(t => t.TypeScore).ThenBy(t => t.LabelScore).Select((t, i) => (handBid: t, rank: i + 1));
			return $"{orderedHandBidsAndRanks.Select(t => (long)t.handBid.Bid * t.rank).Sum()}";
		}

		public static string Part2() {
			var handBids = Utils.ReadAllLines(7).Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Split(' ')).Select(t => new HandBid(t[0], int.Parse(t[1]), true));
			var orderedHandBidsAndRanks = handBids.OrderBy(t => t.TypeScore).ThenBy(t => t.LabelScore).Select((t, i) => (handBid: t, rank: i + 1)).ToArray();
			return $"{orderedHandBidsAndRanks.Select(t => (long)t.handBid.Bid * t.rank).Sum()}";
		}

		private class HandBid {
			private const string orderedFigures = "23456789TJQKA";
			private const string orderedFiguresWithJokers = "J23456789TQKA";

			private static IReadOnlyList<int[]> typesConfigs { get; } = new List<int[]> {
				new[] { 5 },
				new[] { 4 },
				new[] { 3, 2 },
				new[] { 3 },
				new[] { 2, 2 },
				new[] { 2 }
			};

			public int Bid { get; }
			public int TypeScore { get; }
			public long LabelScore { get; }

			public HandBid(string hand, int bid, bool withJokers = false) {
				var cardsInHand = hand.ToList();
				var cardsCounts = cardsInHand.Distinct().ToDictionary(t => t, t => cardsInHand.Count(card => card == t));
				TypeScore = EvaluateHandTypeScore(cardsCounts, withJokers);
				Bid = bid;
				var labelScoreOrderedFigures = withJokers ? orderedFiguresWithJokers : orderedFigures;
				LabelScore = long.Parse(string.Join("", cardsInHand.Select(t => $"{labelScoreOrderedFigures.IndexOf(t):00}")));
			}

			private static int EvaluateHandTypeScore(IReadOnlyDictionary<char, int> figureCounts, bool withJokers) {
				for (var typeIndex = 0; typeIndex < typesConfigs.Count; ++typeIndex) {
					if (IsType(figureCounts, typesConfigs[typeIndex], withJokers)) return typesConfigs.Count - typeIndex;
				}
				return 0;
			}

			private static bool IsType(IReadOnlyDictionary<char, int> figureCounts, IEnumerable<int> typeConfig, bool withJokers) {
				var usedFigures = new HashSet<char>();
				if (withJokers) {
					var jokers = figureCounts.TryGetValue('J', out var numberOfJokers) ? numberOfJokers : 0;
					if (jokers < 5) {
						var modifiedDictionary = figureCounts.Where(t => t.Key != 'J').ToDictionary(t => t.Key, t => t.Value);
						var max = modifiedDictionary.Values.Max();
						modifiedDictionary[modifiedDictionary.Keys.FirstOrDefault(t => modifiedDictionary[t] == max)] += jokers;
						figureCounts = modifiedDictionary;
					}
				}
				foreach (var figureCount in typeConfig) {
					var satisfyingFigure = figureCounts.FirstOrDefault(t => !usedFigures.Contains(t.Key) && t.Value == figureCount);
					if (satisfyingFigure.Key == default) return false;
					usedFigures.Add(satisfyingFigure.Key);
				}
				return true;
			}
		}
	}
}