using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hamiltonian;
using Hamiltonian.Generating;
using Hamiltonian.Measuring;
using Hamiltonian.Solving;

var generator = new Generator(9, 7);
var rng = Random.Shared;
var solver = new Solver();
using var sw = new StreamWriter($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\output.csv");
var stopwatch = new Stopwatch();
stopwatch.Start();
for (var count = 1; count <= 200; count++)
{
	var startCoordinate = new Coordinate(rng.Next(0, 5), rng.Next(0, 4));
	var graph = generator.Generate(startCoordinate, out var endCoordinate, out var path);
	var (sx, sy) = startCoordinate;
	var (ex, ey) = endCoordinate;
	var degreeFreq = Degree.GetDegreeFrequency(graph!);
	var directions = string.Concat(from element in path!.Directions select element.GetArrow().ToString());
	sw.WriteLine($"{graph:bs}\t{sx}\t{sy}\t{ex}\t{ey}\t{directions}\t{(degreeFreq.TryGetValue(3, out var r) ? r : 0)}");

	Console.Clear();
	Console.WriteLine($@"Progress: {count}/200, Elapsed: {stopwatch.Elapsed:hh\:mm\:ss\.fff}");
}
stopwatch.Stop();
Console.WriteLine("Finished.");
