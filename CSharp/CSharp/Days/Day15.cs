using System.Collections.Generic;
using System.Linq;

namespace AOC23.Days {
	public static class Day15 {
		public static string Part1() => $"{Utils.ReadAllLines(15).SelectMany(t => t.Split(",")).Sum(Instruction.GetHash)}";

		public static string Part2() {
			var boxes = Enumerable.Range(0, 256).Select(t => new List<BoxItem>()).ToArray();
			var instructions = Utils.ReadAllLines(15).SelectMany(t => t.Split(",")).Select(Instruction.Parse);
			foreach (var instruction in instructions) instruction.Apply(boxes);
			return $"{boxes.SelectMany((box, boxIndex) => box.Select((slot, slotIndex) => (boxIndex + 1) * (slotIndex + 1) * slot.FocalLength)).Sum()}";
		}

		private class BoxItem {
			public string Label { get; }
			public int FocalLength { get; set; }

			public BoxItem(string label, int focalLength) {
				Label = label;
				FocalLength = focalLength;
			}
		}

		private class Instruction {
			private int Hash { get; }
			private string Label { get; }
			private char Operation { get; }
			private int FocalLength { get; }

			private Instruction(string label, char operation, int focalLength) {
				Hash = GetHash(label);
				Label = label;
				Operation = operation;
				FocalLength = focalLength;
			}

			public static int GetHash(string label) => label.Aggregate(0, (curr, next) => 17 * (curr + next) % 256);

			public static Instruction Parse(string inputItem) {
				if (inputItem.Contains('=')) {
					return new Instruction(inputItem.Split('=')[0], '=', int.Parse(inputItem.Split('=')[1]));
				}
				return new Instruction(inputItem.Split('-')[0], '-', 0);
			}

			public void Apply(List<BoxItem>[] boxes) {
				if (Operation == '=') AddLens(boxes);
				else if (Operation == '-') RemoveLens(boxes);
			}

			private void AddLens(List<BoxItem>[] boxes) {
				var box = boxes[Hash];
				if (box.Any(t => t.Label == Label)) box.Single(t => t.Label == Label).FocalLength = FocalLength;
				else box.Add(new BoxItem(Label, FocalLength));
			}

			private void RemoveLens(List<BoxItem>[] boxes) => boxes[Hash].RemoveAll(t => t.Label == Label);
		}
	}
}