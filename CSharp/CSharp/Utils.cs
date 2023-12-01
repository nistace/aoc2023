using System;
using System.Collections.Generic;
using System.IO;

namespace AOC23 {
	public static class Utils {
		public static IEnumerable<string> ReadAllLines(int day) => File.ReadLines($"{Environment.CurrentDirectory}/../../../Inputs/Day{day:00}.txt");
	}
}