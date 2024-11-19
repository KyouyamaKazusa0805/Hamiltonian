using System;
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
while (true)
{
	var startCoordinate = new Coordinate(rng.Next(0, 5), rng.Next(0, 4));
	var (_, graph, (sx, sy), (ex, ey), path) = generator.Generate(startCoordinate, .7, .8);
	var degreeFreq = Degree.GetDegreeFrequency(graph!);
	var directions = string.Concat(from element in path!.Directions select element.GetArrow().ToString());
	sw.WriteLine($"{graph:bs}\t{sx}\t{sy}\t{ex}\t{ey}\t{directions}\t{(degreeFreq.TryGetValue(3, out var r) ? r : 0)}");
}
