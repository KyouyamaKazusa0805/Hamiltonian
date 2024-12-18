# Hamiltonian

Provides a way to operate with Hamiltonian graphs.

## Usage

### Create a puzzle

To create a 10x10 puzzle with 50%~60% filling rate (approximately), you can use type `Generator` to generate a new puzzle:

```csharp
using System;
using Hamiltonian.Generating;

// To create a puzzle with size 10 x 10, with approximately 50% - 60% filling rate.
// "Filling rate" is a value that describes how complex the puzzle is.
// It means how many cells are used in the puzzle from the full 10 x 10 grid.
var generator = new Generator(10, 10);
var puzzle = generator.Generate(new(0, 0), .5, .6);
if (puzzle.Success)
    Console.WriteLine(puzzle.Graph.ToString());
```

A generate puzzle can be:

```text
10:10:1111111111000001111100000000010000100001001110111101100011110110000001011110001101011011110111111111
```

where 0 is for empty cell, and 1 is for the puzzle.

### Verify the solution

To verify the path, you can use `Solver` type.

```csharp
using System;
using Hamiltonian.Concepts;
using Hamiltonian.Solving;
using System.Linq;

// To parse a grid.
var puzzle = Graph.Parse("10:10:1111111111000001111100000000010000100001001110111101100011110110000001011110001101011011110111111111");
var solver = new Solver();
var path = solver.Solve(puzzle, new(0, 0));

// To see the raw coordinates, you can just use 'ToString' method:
Console.WriteLine(path.ToString(", "));

// You can also output the solution as directions notation:
var directions = path.Directions;
Console.WriteLine(new string(from direction in path.Directions select direction.GetArrow()));
```

The only solution path will be displayed as both raw coordinates or directions:

```text
Raw coordinates:
Coordinate { X = 0, Y = 0 }, Coordinate { X = 0, Y = 1 }, Coordinate { X = 0, Y = 2 }, Coordinate { X = 0, Y = 3 }, Coordinate { X = 0, Y = 4 }, Coordinate { X = 0, Y = 5 }, Coordinate { X = 1, Y = 5 }, Coordinate { X = 1, Y = 6 }, Coordinate { X = 0, Y = 6 }, Coordinate { X = 0, Y = 7 }, Coordinate { X = 1, Y = 7 }, Coordinate { X = 1, Y = 8 }, Coordinate { X = 0, Y = 8 }, Coordinate { X = 0, Y = 9 }, Coordinate { X = 1, Y = 9 }, Coordinate { X = 2, Y = 9 }, Coordinate { X = 3, Y = 9 }, Coordinate { X = 4, Y = 9 }, Coordinate { X = 4, Y = 8 }, Coordinate { X = 4, Y = 7 }, Coordinate { X = 4, Y = 6 }, Coordinate { X = 5, Y = 6 }, Coordinate { X = 5, Y = 7 }, Coordinate { X = 5, Y = 8 }, Coordinate { X = 5, Y = 9 }, Coordinate { X = 6, Y = 9 }, Coordinate { X = 7, Y = 9 }, Coordinate { X = 7, Y = 8 }, Coordinate { X = 8, Y = 8 }, Coordinate { X = 8, Y = 9 }, Coordinate { X = 9, Y = 9 }, Coordinate { X = 9, Y = 8 }, Coordinate { X = 9, Y = 7 }, Coordinate { X = 8, Y = 7 }, Coordinate { X = 8, Y = 6 }, Coordinate { X = 9, Y = 6 }, Coordinate { X = 9, Y = 5 }, Coordinate { X = 9, Y = 4 }, Coordinate { X = 8, Y = 4 }, Coordinate { X = 7, Y = 4 }, Coordinate { X = 7, Y = 3 }, Coordinate { X = 8, Y = 3 }, Coordinate { X = 9, Y = 3 }, Coordinate { X = 9, Y = 2 }, Coordinate { X = 9, Y = 1 }, Coordinate { X = 8, Y = 1 }, Coordinate { X = 7, Y = 1 }, Coordinate { X = 7, Y = 2 }, Coordinate { X = 6, Y = 2 }, Coordinate { X = 6, Y = 1 }, Coordinate { X = 5, Y = 1 }, Coordinate { X = 5, Y = 2 }, Coordinate { X = 4, Y = 2 }, Coordinate { X = 4, Y = 3 }, Coordinate { X = 4, Y = 4 }, Coordinate { X = 3, Y = 4 }

Direction path:
→→→→→↓→↑→↓→↑→↓↓↓↓←←←↓→→→↓↓←↓→↓←←↑←↓←←↑↑←↓↓←←↑↑→↑←↑→↑→→↑
```

