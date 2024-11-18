using System;
using System.IO;
using System.Linq;
using Hamiltonian;
using Hamiltonian.Generating;

var generator = new Generator(9, 7);
var rng = Random.Shared;
var solver = new Solver();
using var sw = new StreamWriter($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\output.csv");
var isFirstLine = true;
foreach (var line in File.ReadLines($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\input.csv"))
{
	if (isFirstLine)
	{
		isFirstLine = false;
		continue;
	}

	var split = line.Split(',');
	var startCoordinate = new Coordinate(int.Parse(split[1]), int.Parse(split[2]));
	var endCoordinate = new Coordinate(int.Parse(split[3]), int.Parse(split[4]));
	var puzzle = Graph.Parse(split[0].AsSpan());
	solver.IsValid(puzzle, startCoordinate, endCoordinate, out var path);

	var (sx, sy) = startCoordinate;
	var (ex, ey) = endCoordinate;
	var directions = string.Concat(from element in path!.Directions select element.GetArrow().ToString());
	sw.WriteLine($"{puzzle:bs}\t{sx}\t{sy}\t{ex}\t{ey}\t{directions}");
}
