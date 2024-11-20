using System;
using Hamiltonian;
using Hamiltonian.Measuring;

var graph = Graph.Parse("9:7:111111110000111011110111001101111110111111011001100001111111111");
var dictionary = Degree.GetDegreeFrequency(graph);
foreach (var (key, value) in dictionary)
{
	Console.WriteLine($"[{key}] = {value}");
}
