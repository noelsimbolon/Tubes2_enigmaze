## Overview

This is a .NET implementation on graph traversal using BFS and DFS written in C#.

The application accepts a `.txt` file as an input. It must contain a `K` character, at least one `T` character, `R` character(s), and `X` character(s) representing a starting node, treasure node(s), path node(s), and wall node(s) respectively. See one of the [example](https://github.com/noelsimbolon/Tubes2_enigmaze/blob/main/test/ludo.txt).

The application then uses breadth-first search or depth-first search algorithm to find a path from the starting node to collect all of the treasures.

If the `Go Back to Start` checkbox is checked, the algorithm will find extra path to go back to the starting node after collecting all of the treasures.

[![enigmaze.png](https://i.postimg.cc/d1QZ8kfT/enigmaze.png)](https://postimg.cc/NyV0tMBg)

## How To Use

1. Download the [`bin`](https://github.com/noelsimbolon/Tubes2_enigmaze/tree/main/bin) folder
2. Extract everything to a single directory
3. Run `Enigmaze.UI.exe`
4. Click <kbd>Open Maze</kbd> to open a `.txt` file
5. Select an algorithm (BFS/DFS) by clicking on one of the radio buttons
6. Check the `Go Back to Start` checkbox if you want the algorithm to find a path to go back to the starting node after collecting all of the treasures
7. Determine the step interval which is the interval between checking nodes in the visualization, by default it's `100 ms`
8. Click <kbd>Start</kbd> to start the path-finding algorithm and visualization

## Dependencies and Prerequisites

- [JetBrains Rider](https://www.jetbrains.com/rider/) (optional, for convenience) - This is an IDE for .NET developed by JetBrains.

- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) - This is required to build and run the project.

- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/) - This is required for running unit tests with the dotnet test command.

- [NUnit](https://www.nuget.org/packages/NUnit/) - This is the NUnit framework that is used for writing unit tests.

- [NUnit3TestAdapter](https://www.nuget.org/packages/NUnit3TestAdapter/) - This is the NUnit 3 Test Adapter that allows running NUnit tests in Visual Studio and on the command line.

- [NUnit.Analyzers](https://www.nuget.org/packages/NUnit.Analyzers/) - This is a set of Roslyn analyzers that provide additional checks and suggestions for writing NUnit tests.

- [coverlet.collector](https://www.nuget.org/packages/coverlet.collector/) - This is a code coverage tool that is used for measuring the code coverage of your tests.

## Directory Structure

```
├───bin     # contains executable file
├───doc     # contains report for this application
├───src     # contains source code for the application
└───test    # contains input text files for maze generation. some of the files here are invalid (which are used for unit testing)
```

## How To Build

Here are the steps to build the application using JetBrains Rider:
1. Download this repository
2. Open this repository as a solution with `/src` as its root
3. Open the <kbd>Build</kbd> menu
4. Click <kbd>Build Solution</kbd>

For more information on what is happening in the build process, refer to the JetBrains Rider's [documentation](https://www.jetbrains.com/help/rider/Build_Process.html).

## Author

| Name                   | GitHub                                            |
|------------------------|---------------------------------------------------|
| Noel Simbolon          | [noelsimbolon](https://github.com/noelsimbolon)   |
| Bintang Dwi Marthen    | [Marthenn](https://github.com/Marthenn)           |
| Austin Gabriel Pardosi | [AustinPardosi](https://github.com/AustinPardosi) |