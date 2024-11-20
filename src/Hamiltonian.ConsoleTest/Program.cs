using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Hamiltonian;
using Hamiltonian.Solving;

var sb = new StringBuilder();
var solver = new Solver();
using var sw = new StreamWriter(@"C:\Users\admin\Desktop\output.txt");
var stopwatch = new Stopwatch();
stopwatch.Start();
var i = 1;
foreach (var line in File.ReadLines(@"C:\Users\admin\Desktop\一笔画.txt"))
{
	var split = line.Split('\t');
	var graph = Graph.Parse(split[0]);
	var path = solver.Solve(graph, new(int.Parse(split[1]), int.Parse(split[2])));

	sb.Clear();
	foreach (var coordinate in path)
	{
		sb.Append($"{coordinate.X}{coordinate.Y}{graph.GetDegreeAt(coordinate)}");
	}
	sw.WriteLine($"{graph:bs}\t{sb}");

	Console.Clear();
	Console.WriteLine($@"Progress: {i / (40350.0 - 27139):P3}, Elapsed: {stopwatch.Elapsed:hh\:mm\:ss\.fff}");
	i++;
}
