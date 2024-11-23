# 一笔画游戏

提供一组 API 用于操作一笔画盘面。

## 使用方式

### 创建一个题目

你可以使用 `Generator` 类型来创建一个题目，规格为 10 × 10，并且用到其中 50% - 60% 的格子，并且一笔画的起点为单元格 (0, 0)。

```csharp
using System;
using Hamiltonian.Generating;

var generator = new Generator(10, 10);
var puzzle = generator.Generate(new(0, 0), .5, .6);
if (puzzle.Success)
    Console.WriteLine(puzzle.Graph.ToString());
```

一个生成的题目是这样的：

```text
10:10:1111111111000001111100000000010000100001001110111101100011110110000001011110001101011011110111111111
```

其中 0 表示空格，1 则表示使用的格子。

### 校验题目的解

如果要检查题目的一笔画路径，请使用类型 `Solver`。

```csharp
using System;
using Hamiltonian.Concepts;
using Hamiltonian.Solving;
using System.Linq;

// 使用字符串可以直接解析回一个盘面，然后使用 solver 来解题。
var puzzle = Graph.Parse("10:10:1111111111000001111100000000010000100001001110111101100011110110000001011110001101011011110111111111");
var solver = new Solver();
var path = solver.Solve(puzzle, new(0, 0));

// 如果你想要看原始的坐标的走向，请使用 ToString 方法：
Console.WriteLine(path.ToString(", "));

// 你也可以使用提供的 Directions 属性来获取前进路径的上下左右的箭头，使用 new string(ReadOnlySpan<char>) 构造器可以将序列改造为箭头记法的字符串。
var directions = path.Directions;
Console.WriteLine(new string(from direction in path.Directions select direction.GetArrow()));
```

这个题目的解会通过上述代码得到：

```text
使用坐标的原始输出：
Coordinate { X = 0, Y = 0 }, Coordinate { X = 0, Y = 1 }, Coordinate { X = 0, Y = 2 }, Coordinate { X = 0, Y = 3 }, Coordinate { X = 0, Y = 4 }, Coordinate { X = 0, Y = 5 }, Coordinate { X = 1, Y = 5 }, Coordinate { X = 1, Y = 6 }, Coordinate { X = 0, Y = 6 }, Coordinate { X = 0, Y = 7 }, Coordinate { X = 1, Y = 7 }, Coordinate { X = 1, Y = 8 }, Coordinate { X = 0, Y = 8 }, Coordinate { X = 0, Y = 9 }, Coordinate { X = 1, Y = 9 }, Coordinate { X = 2, Y = 9 }, Coordinate { X = 3, Y = 9 }, Coordinate { X = 4, Y = 9 }, Coordinate { X = 4, Y = 8 }, Coordinate { X = 4, Y = 7 }, Coordinate { X = 4, Y = 6 }, Coordinate { X = 5, Y = 6 }, Coordinate { X = 5, Y = 7 }, Coordinate { X = 5, Y = 8 }, Coordinate { X = 5, Y = 9 }, Coordinate { X = 6, Y = 9 }, Coordinate { X = 7, Y = 9 }, Coordinate { X = 7, Y = 8 }, Coordinate { X = 8, Y = 8 }, Coordinate { X = 8, Y = 9 }, Coordinate { X = 9, Y = 9 }, Coordinate { X = 9, Y = 8 }, Coordinate { X = 9, Y = 7 }, Coordinate { X = 8, Y = 7 }, Coordinate { X = 8, Y = 6 }, Coordinate { X = 9, Y = 6 }, Coordinate { X = 9, Y = 5 }, Coordinate { X = 9, Y = 4 }, Coordinate { X = 8, Y = 4 }, Coordinate { X = 7, Y = 4 }, Coordinate { X = 7, Y = 3 }, Coordinate { X = 8, Y = 3 }, Coordinate { X = 9, Y = 3 }, Coordinate { X = 9, Y = 2 }, Coordinate { X = 9, Y = 1 }, Coordinate { X = 8, Y = 1 }, Coordinate { X = 7, Y = 1 }, Coordinate { X = 7, Y = 2 }, Coordinate { X = 6, Y = 2 }, Coordinate { X = 6, Y = 1 }, Coordinate { X = 5, Y = 1 }, Coordinate { X = 5, Y = 2 }, Coordinate { X = 4, Y = 2 }, Coordinate { X = 4, Y = 3 }, Coordinate { X = 4, Y = 4 }, Coordinate { X = 3, Y = 4 }

箭头记号：
→→→→→↓→↑→↓→↑→↓↓↓↓←←←↓→→→↓↓←↓→↓←←↑←↓←←↑↑←↓↓←←↑↑→↑←↑→↑→→↑
```

