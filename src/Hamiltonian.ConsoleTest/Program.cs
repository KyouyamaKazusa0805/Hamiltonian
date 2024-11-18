using System;
using System.IO;
using Hamiltonian.Generating;
using Hamiltonian.Measuring;

var generator = new Generator(9, 7);
var rng = Random.Shared;
using var sw = new StreamWriter($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\output.csv");
for (var i = 0; i < 200; i++)
{
	var (x, y) = (rng.Next(0, 5), rng.Next(0, 4));
	var puzzle = generator.Generate(new(x, y), out var end, out var path);
	var length = puzzle!.Length;
	var entropy = PathEntropy.GetEntropyValue(path!);
	var degreeAverage = Degree.GetDegreeSum(puzzle) / (double)puzzle.Length;
	var occupancy = puzzle.Length / (double)(puzzle.RowsLength * puzzle.ColumnsLength);
	sw.WriteLine($"{puzzle:bs}\t{x}\t{y}\t{end.X}\t{end.Y}\t{length}\t{entropy}\t{degreeAverage:#.00}\t{occupancy:P2}");
}
