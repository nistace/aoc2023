using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC23.Days {
	public static class Day05 {
		public static string Part1() {
			var inputLines = Utils.ReadAllLines(5).ToArray();
			var seeds = inputLines[0].Replace("seeds:", "").Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(long.Parse).ToArray();
			var maps = Map.ParseAll(inputLines.Skip(1));
			var locations = seeds.Select(t => maps.Aggregate(t, (cur, map) => map.Convert(cur)));
			return $"{locations.Min()}";
		}

		public static string Part2() {
			var inputLines = Utils.ReadAllLines(5).ToArray();
			var maps = Map.ParseAll(inputLines.Skip(1)).Reverse().ToArray();
			var seedRanges = SeedRange.ParseAll(inputLines[0]);

			for (var i = 0L;; i++) {
				var source = maps.Aggregate(i, (cur, map) => map.ReverseConvert(cur));
				if (seedRanges.Any(t => t.ContainsValue(source))) return $"{i}";
			}
		}

		private class SeedRange {
			private long Start { get; }
			private long Length { get; }
			private long End => Start + Length - 1;

			public static IReadOnlyList<SeedRange> ParseAll(string inputSeedLine) {
				var seedRanges = new List<SeedRange>();
				var values = inputSeedLine.Replace("seeds:", "").Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(long.Parse).ToArray();
				for (var i = 0; i < values.Length; i += 2) {
					seedRanges.Add(new SeedRange(values[i], values[i + 1]));
				}
				return seedRanges;
			}

			private SeedRange(long start, long length) {
				Start = start;
				Length = length;
			}

			public bool ContainsValue(long value) => value >= Start && value <= End;
		}

		private class Map {
			private static Regex mapTitleRegex { get; } = new Regex("\\w+-to-(?<dest>\\w+) +map:");

			private IReadOnlyList<Conversion> Conversions { get; }

			private Map(IEnumerable<Conversion> conversions) {
				Conversions = conversions.ToArray();
			}

			public static IReadOnlyList<Map> ParseAll(IEnumerable<string> inputMapLines) {
				var maps = new List<Map>();
				var currentMapConversionArray = new List<Conversion>();
				foreach (var inputLine in inputMapLines) {
					var mapTitleMatch = mapTitleRegex.Match(inputLine);
					if (mapTitleMatch.Success) {
						if (currentMapConversionArray.Count > 0) {
							maps.Add(new Map(currentMapConversionArray));
							currentMapConversionArray.Clear();
						}
						continue;
					}
					if (Conversion.Parse(inputLine, out var conversion)) {
						currentMapConversionArray.Add(conversion);
					}
				}
				if (currentMapConversionArray.Count > 0) {
					maps.Add(new Map(currentMapConversionArray));
				}
				return maps;
			}

			public long Convert(long value) {
				foreach (var conversion in Conversions) {
					if (conversion.ContainsSource(value)) return conversion.Convert(value);
				}
				return value;
			}

			public long ReverseConvert(long value) {
				foreach (var conversion in Conversions) {
					if (conversion.ContainsDestination(value)) return conversion.ReverseConvert(value);
				}
				return value;
			}

			private class Conversion {
				private static Regex regex { get; } = new Regex("(?<dest>\\d+) +(?<src>\\d+) +(?<length>\\d+)");
				private long SourceRangeStart { get; }
				private long SourceRangeEnd => SourceRangeStart + RangeLength - 1;
				private long DestinationRangeStart { get; }
				private long DestinationRangeEnd => DestinationRangeStart + RangeLength - 1;
				private long RangeLength { get; }

				private Conversion(long sourceRangeStart, long destinationRangeStart, long rangeLength) {
					SourceRangeStart = sourceRangeStart;
					DestinationRangeStart = destinationRangeStart;
					RangeLength = rangeLength;
				}

				public static bool Parse(string inputLine, out Conversion conversion) {
					conversion = default;
					var match = regex.Match(inputLine);
					if (!match.Success) return false;
					conversion = new Conversion(long.Parse(match.Groups["src"].Value), long.Parse(match.Groups["dest"].Value), long.Parse(match.Groups["length"].Value));
					return true;
				}

				public long Convert(long value) {
					if (value < SourceRangeStart) return value;
					if (value > SourceRangeEnd) return value;
					return value - SourceRangeStart + DestinationRangeStart;
				}

				public long ReverseConvert(long value) {
					if (value < DestinationRangeStart) return value;
					if (value > DestinationRangeEnd) return value;
					return value + SourceRangeStart - DestinationRangeStart;
				}

				public bool ContainsSource(long value) => value >= SourceRangeStart && value <= SourceRangeEnd;
				public bool ContainsDestination(long value) => value >= DestinationRangeStart && value <= DestinationRangeEnd;
			}
		}
	}
}