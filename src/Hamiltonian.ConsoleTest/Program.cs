using System;
using System.IO;
using System.Linq;
using Hamiltonian;
using Hamiltonian.Measuring;
using Path = Hamiltonian.Path;
using SpecialFolder = System.Environment.SpecialFolder;

var desktop = Environment.GetFolderPath(SpecialFolder.Desktop);

using var sw = new StreamWriter($@"{desktop}\一笔画_结果.csv");
foreach (var line in File.ReadLines($@"{desktop}\一笔画.txt"))
{
	var split = line.Split('\t');
	var graphStr = split[0];
	var graph = Graph.Parse(graphStr);
	var startX = split[1];
	var startY = split[2];
	var directions = split[5];
	var path = Path.Parse($"{startX}{startY}:{directions}");
	var pathString = string.Concat(from coordinate in path select $"{coordinate.X}{coordinate.Y}");
	var degree4Count = Degree.GetDegreeFrequency(graph).TryGetValue(4, out var c) ? c : 0;
	sw.WriteLine($"{graphStr},{startX}{startY},{pathString},{degree4Count}");
}
