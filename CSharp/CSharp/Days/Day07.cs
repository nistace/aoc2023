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
			return $"";
		}

		private class HandBid {
			private const string orderedFigures = "23456789TJQKA";

			private static IReadOnlyList<int[]> typesConfigs { get; } = new List<int[]> {
				new[] { 5 },
				new[] { 4 },
				new[] { 3, 2 },
				new[] { 3 },
				new[] { 2, 2 },
				new[] { 2 }
			};

			private string Hand { get; }
			public int Bid { get; }
			public int TypeScore { get; }
			public long LabelScore { get; }

			public HandBid(string hand, int bid) {
				Hand = hand;
				var cardsInHand = hand.ToList();
				var cardsCounts = cardsInHand.Distinct().ToDictionary(t => t, t => cardsInHand.Count(card => card == t));
				TypeScore = EvaluateHandTypeScore(cardsCounts);
				Bid = bid;
				LabelScore = long.Parse(string.Join("", cardsInHand.Select(t => $"{orderedFigures.IndexOf(t):00}")));
			}

			private static int EvaluateHandTypeScore(IReadOnlyDictionary<char, int> figureCounts) {
				for (var typeIndex = 0; typeIndex < typesConfigs.Count; ++typeIndex) {
					if (IsType(figureCounts, typesConfigs[typeIndex])) return typesConfigs.Count - typeIndex;
				}
				return 0;
			}

			private static bool IsType(IReadOnlyDictionary<char, int> figureCounts, IEnumerable<int> typeConfig) {
				var usedFigures = new HashSet<char>();
				foreach (var figureCount in typeConfig) {
					var satisfyingFigure = figureCounts.FirstOrDefault(t => !usedFigures.Contains(t.Key) && t.Value == figureCount);
					if (satisfyingFigure.Key == default) return false;
					usedFigures.Add(satisfyingFigure.Key);
				}
				return true;
			}

			public override string ToString() => $"{Hand}\t{Bid}\t{TypeScore}\t{LabelScore}";
		}
	}
}