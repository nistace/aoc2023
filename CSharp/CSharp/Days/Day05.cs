using System;
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
			return $"";
		}

		private class Map {
			private static Regex mapTitleRegex { get; } = new Regex("\\w+-to-(?<dest>\\w+) +map:");

			public string Destination { get; }
			private IReadOnlyList<Conversion> Conversions { get; }

			private Map(string destination, IEnumerable<Conversion> conversions) {
				Destination = destination;
				Conversions = conversions.ToArray();
			}

			public static IReadOnlyList<Map> ParseAll(IEnumerable<string> inputMapLines) {
				var maps = new List<Map>();
				var currentMapDestination = string.Empty;
				var currentMapConversionArray = new List<Conversion>();
				foreach (var inputLine in inputMapLines) {
					var mapTitleMatch = mapTitleRegex.Match(inputLine);
					if (mapTitleMatch.Success) {
						if (currentMapConversionArray.Count > 0) {
							maps.Add(new Map(currentMapDestination, currentMapConversionArray));
							currentMapConversionArray.Clear();
						}
						currentMapDestination = mapTitleMatch.Groups["dest"].Value;
						continue;
					}
					if (Conversion.Parse(inputLine, out var conversion)) {
						currentMapConversionArray.Add(conversion);
					}
				}
				if (currentMapConversionArray.Count > 0) {
					maps.Add(new Map(currentMapDestination, currentMapConversionArray));
				}
				return maps;
			}

			public long Convert(long value) => Conversions.FirstOrDefault(t => t.ContainsSource(value))?.Convert(value) ?? value;

			private class Conversion {
				private static Regex regex { get; } = new Regex("(?<dest>\\d+) +(?<src>\\d+) +(?<length>\\d+)");
				private long SourceRangeStart { get; }
				private long DestinationRangeStart { get; }
				private long RangeLength { get; }
				private long SourceRangeEnd => SourceRangeStart + RangeLength - 1;

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

				public bool ContainsSource(long value) => value >= SourceRangeStart && value <= SourceRangeEnd;
			}
		}
	}
}